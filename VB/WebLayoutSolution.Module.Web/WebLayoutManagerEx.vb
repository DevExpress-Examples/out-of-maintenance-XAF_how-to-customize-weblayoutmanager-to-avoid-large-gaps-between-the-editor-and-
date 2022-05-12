Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Layout
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Web.Layout
Imports DevExpress.ExpressApp.Web.SystemModule
Imports DevExpress.Persistent.Base
Imports System.Collections.Generic
Imports System.ComponentModel

'#pragma warning disable 109
Namespace WebLayoutSolution.Module.Web

    Public Enum TextAlignModeGroup
        UseParentOptions = 0
        AlignLocal = 1
        AutoSize = 2
        'CustomSize = 3,
        AlignWithChildren = 4
    End Enum

    Public Enum TextAlignModeItem
        UseParentOptions = 0
        'AlignLocal = 1,
        AutoSize = 2
    'CustomSize = 3,
    'AlignWithChildren = 4
    End Enum

    Public Interface IModelWebLayoutGroup

        <Category("Behavior"), DefaultValue(TextAlignModeGroup.UseParentOptions)>
        Property TextAlignMode As TextAlignModeGroup

    End Interface

    Public Interface IModelWebLayoutItem

        <Category("Behavior"), DefaultValue(TextAlignModeItem.UseParentOptions)>
        Property TextAlignMode As TextAlignModeItem

    End Interface

    Public Class WebLayoutManagerEx
        Inherits WebLayoutManager

        Public Sub New(ByVal simple As Boolean, ByVal delayedItemsInitialization As Boolean)
            MyBase.New(simple, delayedItemsInitialization)
        End Sub

        Protected Overrides Function LayoutItem(ByVal viewItems As ViewItemsCollection, ByVal layoutItemModel As IModelLayoutViewItem) As LayoutItemTemplateContainer
            Dim templateContainer As LayoutItemTemplateContainer = New LayoutItemTemplateContainer(Me, viewItems, layoutItemModel)
            templateContainer.Template = LayoutItemTemplate
            templateContainer.ID = WebIdHelper.GetCorrectedLayoutItemId(layoutItemModel)
            Dim viewItem As ViewItem = FindViewItem(viewItems, layoutItemModel)
            templateContainer.ViewItem = viewItem
            If viewItem IsNot Nothing Then
                Dim args As MarkRequiredFieldCaptionEventArgs = New MarkRequiredFieldCaptionEventArgs(viewItem, False)
                OnMarkRequiredFieldCaption(args)
                templateContainer.Caption = BuildItemCaption(viewItem, args.NeedMarkRequiredField, args.RequiredFieldMark)
            End If

            templateContainer.ShowCaption = GetIsLayoutItemCaptionVisible(layoutItemModel, viewItem)
            templateContainer.CaptionWidth = CalculateCaptionWidth(viewItem, viewItems, layoutItemModel)
            templateContainer.CaptionLocation = GetCaptionLocation(layoutItemModel)
            templateContainer.CaptionHorizontalAlignment = GetCaptionHorizontalAlignment(layoutItemModel)
            templateContainer.CaptionVerticalAlignment = GetCaptionVerticalAlignment(layoutItemModel)
            OnLayoutItemCreatedNew(templateContainer, layoutItemModel, viewItem)
            Return templateContainer
        End Function

        Private Sub OnLayoutItemCreatedNew(ByVal templateContainer As LayoutItemTemplateContainerBase, ByVal layoutItemModel As IModelViewLayoutElement, ByVal viewItem As ViewItem)
            If Not DelayedItemsInitialization Then
                templateContainer.Instantiate()
            End If

            OnLayoutItemCreated(New ItemCreatedEventArgs(layoutItemModel, viewItem, templateContainer))
            OnCustomizeAppearance(New CustomizeAppearanceEventArgs(layoutItemModel.Id, New WebLayoutItemAppearanceAdapter(templateContainer), Nothing))
        End Sub

        Private Function GetCaptionLocation(ByVal layoutItemModel As IModelLayoutViewItem) As DevExpress.Utils.Locations
            Dim captionLocation As Locations = layoutItemModel.CaptionLocation
            Return If(Equals(captionLocation, Locations.Default), DefaultLayoutItemCaptionLocation, CType(captionLocation, DevExpress.Utils.Locations))
        End Function

        Private Function GetCaptionHorizontalAlignment(ByVal layoutItemModel As IModelLayoutViewItem) As DevExpress.Utils.HorzAlignment
            Dim captionHorizontalAlignment As DevExpress.Utils.HorzAlignment = layoutItemModel.CaptionHorizontalAlignment
            Return If(Equals(captionHorizontalAlignment, DevExpress.Utils.HorzAlignment.Default), DefaultLayoutItemCaptionHorizontalAlignment, captionHorizontalAlignment)
        End Function

        Private Function GetCaptionVerticalAlignment(ByVal layoutItemModel As IModelLayoutViewItem) As DevExpress.Utils.VertAlignment
            Dim captionVerticalAlignment As DevExpress.Utils.VertAlignment = layoutItemModel.CaptionVerticalAlignment
            Return If(Equals(captionVerticalAlignment, DevExpress.Utils.VertAlignment.Default), DefaultLayoutItemCaptionVerticalAlignment, captionVerticalAlignment)
        End Function

        Private Shared Function FindViewItem(ByVal viewItems As ViewItemsCollection, ByVal layoutItemModel As IModelLayoutViewItem) As ViewItem
            Dim viewItem As IModelViewItem = layoutItemModel.ViewItem
            Dim viewItemId As String = If(viewItem IsNot Nothing, viewItem.Id, layoutItemModel.Id)
            Return viewItems(viewItemId)
        End Function

        Private Function CalculateCaptionWidth(ByVal viewItem As ViewItem, ByVal viewItems As ViewItemsCollection, ByVal layoutItemModel As IModelLayoutViewItem) As System.Web.UI.WebControls.Unit
            Dim item = TryCast(layoutItemModel, IModelWebLayoutItem)
            If item IsNot Nothing Then
                If item.TextAlignMode = TextAlignModeItem.AutoSize Then
                    Return GetMaxStringWidth(New String() {EnsureCaptionColon(viewItem.Caption)})
                Else
                    Dim current As IModelViewLayoutElement = layoutItemModel
                    While current IsNot Nothing
                        Dim group = TryCast(current.Parent, IModelWebLayoutGroup)
                        If group IsNot Nothing Then
                            If group.TextAlignMode = TextAlignModeGroup.AutoSize Then
                                Return GetMaxStringWidth(New String() {EnsureCaptionColon(viewItem.Caption)})
                            End If

                            If group.TextAlignMode = TextAlignModeGroup.AlignLocal Then
                                Return CalculateLayoutItemCaptionWidthNew(CType(group, IModelLayoutGroup), viewItems, False)
                            End If

                            If group.TextAlignMode = TextAlignModeGroup.AlignWithChildren Then
                                Return CalculateLayoutItemCaptionWidthNew(CType(group, IModelLayoutGroup), viewItems, True)
                            End If
                        End If

                        current = TryCast(current.Parent, IModelViewLayoutElement)
                    End While
                End If
            End If

            Return Me.LayoutItemCaptionWidth
        End Function

        Private Function CalculateLayoutItemCaptionWidthNew(ByVal layoutInfo As IEnumerable(Of IModelViewLayoutElement), ByVal viewItems As ViewItemsCollection, ByVal recursively As Boolean) As System.Web.UI.WebControls.Unit
            Dim list As List(Of String) = New List(Of String)()
            Call CollectLayoutItemVisibleCaptions(Of IModelViewLayoutElement)(list, layoutInfo, viewItems, recursively)
            Return GetMaxStringWidth(list)
        End Function

        Private Overloads Sub CollectLayoutItemVisibleCaptions(Of T)(ByVal captions As IList(Of String), ByVal layoutInfo As IEnumerable(Of T), ByVal viewItems As ViewItemsCollection, ByVal recursively As Boolean)
            For Each itemInfo As T In layoutInfo
                If TypeOf itemInfo Is IModelLayoutViewItem Then
                    Dim layoutItemModel As IModelLayoutViewItem = CType(itemInfo, IModelLayoutViewItem)
                    Dim viewItem As ViewItem = FindViewItem(viewItems, layoutItemModel)
                    If viewItem IsNot Nothing AndAlso GetIsLayoutItemCaptionVisible(layoutItemModel, viewItem) AndAlso GetIsItemForCaptionCalculation(layoutItemModel, viewItem) Then
                        Dim args As MarkRequiredFieldCaptionEventArgs = New MarkRequiredFieldCaptionEventArgs(viewItem, False)
                        OnMarkRequiredFieldCaption(args)
                        captions.Add(BuildItemCaption(viewItem, args.NeedMarkRequiredField, args.RequiredFieldMark))
                    End If
                ElseIf recursively Then
                    If TypeOf itemInfo Is IEnumerable(Of IModelViewLayoutElement) Then
                        Call CollectLayoutItemVisibleCaptions(Of IModelViewLayoutElement)(captions, CType(itemInfo, IEnumerable(Of IModelViewLayoutElement)), viewItems, recursively)
                    ElseIf TypeOf itemInfo Is IEnumerable(Of IModelLayoutGroup) Then
                        Call CollectLayoutItemVisibleCaptions(Of IModelLayoutGroup)(captions, CType(itemInfo, IEnumerable(Of IModelLayoutGroup)), viewItems, recursively)
                    End If
                End If
            Next
        End Sub
    End Class
End Namespace
