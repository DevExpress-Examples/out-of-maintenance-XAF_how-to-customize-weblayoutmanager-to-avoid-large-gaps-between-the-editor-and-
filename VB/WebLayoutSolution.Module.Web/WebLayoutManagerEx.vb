Imports DevExpress.ExpressApp.Editors
Imports DevExpress.ExpressApp.Layout
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Web.Layout
Imports DevExpress.ExpressApp.Web.SystemModule
Imports DevExpress.Utils
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Text

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
        <Category("Behavior"), DefaultValue(TextAlignModeGroup.UseParentOptions)> _
        Property TextAlignMode() As TextAlignModeGroup
    End Interface
    Public Interface IModelWebLayoutItem
        <Category("Behavior"), DefaultValue(TextAlignModeItem.UseParentOptions)> _
        Property TextAlignMode() As TextAlignModeItem
    End Interface
    Public Class WebLayoutManagerEx
        Inherits WebLayoutManager

        Public Sub New(ByVal simple As Boolean, ByVal delayedItemsInitialization As Boolean)
            MyBase.New(simple, delayedItemsInitialization)
        End Sub
        Protected Overrides Function LayoutItem(ByVal detailViewItems As ViewItemsCollection, ByVal layoutItemInfo As IModelLayoutViewItem) As LayoutItemTemplateContainer
            Dim key As String = If(layoutItemInfo.ViewItem IsNot Nothing, layoutItemInfo.ViewItem.Id, layoutItemInfo.Id)
            Dim viewItem As ViewItem = detailViewItems(key)
            Dim layoutItemTemplateContainer As New LayoutItemTemplateContainer(Me, detailViewItems, layoutItemInfo)
            layoutItemTemplateContainer.ID = WebIdHelper.GetCorrectedLayoutItemId(layoutItemInfo)
            layoutItemTemplateContainer.ShowCaption = Me.GetIsLayoutItemCaptionVisible(layoutItemInfo, viewItem)
            If viewItem IsNot Nothing Then
                layoutItemTemplateContainer.Caption = Me.EnsureCaptionColon(viewItem.Caption)
            End If
            layoutItemTemplateContainer.CaptionWidth = CalculateCaptionWidth(viewItem, detailViewItems, layoutItemInfo)
            Dim captionLocation As Locations = layoutItemInfo.CaptionLocation
            Dim captionHorizontalAlignment As HorzAlignment = layoutItemInfo.CaptionHorizontalAlignment
            Dim captionVerticalAlignment As VertAlignment = layoutItemInfo.CaptionVerticalAlignment
            layoutItemTemplateContainer.CaptionLocation = (If(Equals(captionLocation, Locations.Default), Me.DefaultLayoutItemCaptionLocation, captionLocation))
            layoutItemTemplateContainer.CaptionHorizontalAlignment = (If(Equals(captionHorizontalAlignment, HorzAlignment.Default), Me.DefaultLayoutItemCaptionHorizontalAlignment, captionHorizontalAlignment))
            layoutItemTemplateContainer.CaptionVerticalAlignment = (If(Equals(captionVerticalAlignment, VertAlignment.Default), Me.DefaultLayoutItemCaptionVerticalAlignment, captionVerticalAlignment))
            Me.LayoutItemTemplate.InstantiateIn(layoutItemTemplateContainer)
            Dim args As New ItemCreatedEventArgs(layoutItemInfo, viewItem, layoutItemTemplateContainer)
            Me.OnLayoutItemCreated(args)
            Me.OnCustomizeAppearance(New CustomizeAppearanceEventArgs(layoutItemInfo.Id, New WebLayoutItemAppearanceAdapter(layoutItemTemplateContainer), Nothing))
            Return layoutItemTemplateContainer
        End Function
        Private Function CalculateCaptionWidth(ByVal viewItem As ViewItem, ByVal detailViewItems As ViewItemsCollection, ByVal layoutItemInfo As IModelLayoutViewItem) As System.Web.UI.WebControls.Unit
            Dim item = TryCast(layoutItemInfo, IModelWebLayoutItem)
            If item IsNot Nothing Then
                If item.TextAlignMode = TextAlignModeItem.AutoSize Then
                    Return Me.GetMaxStringWidth(New String() { Me.EnsureCaptionColon(viewItem.Caption) })
                Else
                    Dim current As IModelViewLayoutElement = layoutItemInfo
                    Do While current IsNot Nothing
                        Dim group = TryCast(current.Parent, IModelWebLayoutGroup)
                        If group IsNot Nothing Then
                            If group.TextAlignMode = TextAlignModeGroup.AutoSize Then
                                Return Me.GetMaxStringWidth(New String() { Me.EnsureCaptionColon(viewItem.Caption) })
                            End If
                            If group.TextAlignMode = TextAlignModeGroup.AlignLocal Then
                                Return CalculateLayoutItemCaptionWidth(DirectCast(group, IModelLayoutGroup), detailViewItems, False)
                            End If
                            If group.TextAlignMode = TextAlignModeGroup.AlignWithChildren Then
                                Return CalculateLayoutItemCaptionWidth(DirectCast(group, IModelLayoutGroup), detailViewItems, True)
                            End If
                        End If
                        current = TryCast(current.Parent, IModelViewLayoutElement)
                    Loop
                End If
            End If
            Return Me.LayoutItemCaptionWidth
        End Function
        Private Shadows Function CalculateLayoutItemCaptionWidth(ByVal layoutInfo As IEnumerable(Of IModelViewLayoutElement), ByVal detailViewItems As ViewItemsCollection, ByVal recursively As Boolean) As System.Web.UI.WebControls.Unit
            Dim list As New List(Of String)()
            CollectLayoutItemVisibleCaptions(Of IModelViewLayoutElement)(list, layoutInfo, detailViewItems, recursively)
            Return Me.GetMaxStringWidth(list)
        End Function
        Private Shadows Sub CollectLayoutItemVisibleCaptions(Of T)(ByVal targetList As IList(Of String), ByVal layoutInfo As IEnumerable(Of T), ByVal detailViewItems As ViewItemsCollection, ByVal recursively As Boolean)
            For Each current As T In layoutInfo
                If TypeOf current Is IModelLayoutViewItem Then
                    Dim modelLayoutViewItem As IModelLayoutViewItem = DirectCast(DirectCast(current, Object), IModelLayoutViewItem)
                    Dim key As String = If(modelLayoutViewItem.ViewItem IsNot Nothing, modelLayoutViewItem.ViewItem.Id, modelLayoutViewItem.Id)
                    Dim viewItem As ViewItem = detailViewItems(key)
                    If viewItem IsNot Nothing AndAlso Me.GetIsLayoutItemCaptionVisible(DirectCast(DirectCast(current, Object), IModelLayoutViewItem), viewItem) AndAlso Me.GetIsItemForCaptionCalculation(DirectCast(DirectCast(current, Object), IModelLayoutViewItem), viewItem) Then
                        targetList.Add(Me.EnsureCaptionColon(viewItem.Caption))
                    End If
                ElseIf recursively Then
                    If TypeOf current Is IEnumerable(Of IModelViewLayoutElement) Then
                        CollectLayoutItemVisibleCaptions(Of IModelViewLayoutElement)(targetList, DirectCast(DirectCast(current, Object), IEnumerable(Of IModelViewLayoutElement)), detailViewItems, recursively)
                    End If
                    If TypeOf current Is IEnumerable(Of IModelLayoutGroup) Then
                        CollectLayoutItemVisibleCaptions(Of IModelLayoutGroup)(targetList, DirectCast(DirectCast(current, Object), IEnumerable(Of IModelLayoutGroup)), detailViewItems, recursively)
                    End If
                End If
            Next current
        End Sub

    End Class
End Namespace
