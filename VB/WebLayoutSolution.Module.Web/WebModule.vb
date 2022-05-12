Imports System
Imports System.ComponentModel
Imports DevExpress.ExpressApp
Imports System.Collections.Generic
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Updating

Namespace WebLayoutSolution.Module.Web

    <ToolboxItemFilter("Xaf.Platform.Web")>
    ' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppModuleBasetopic.
    Public NotInheritable Partial Class WebLayoutSolutionAspNetModule
        Inherits ModuleBase

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Overrides Function GetModuleUpdaters(ByVal objectSpace As IObjectSpace, ByVal versionFromDB As Version) As IEnumerable(Of ModuleUpdater)
            Return ModuleUpdater.EmptyModuleUpdaters
        End Function

        Public Overrides Sub Setup(ByVal application As XafApplication)
            MyBase.Setup(application)
        ' Manage various aspects of the application UI and behavior at the module level.
        End Sub

        Public Overrides Sub ExtendModelInterfaces(ByVal extenders As ModelInterfaceExtenders)
            MyBase.ExtendModelInterfaces(extenders)
            extenders.Add(Of IModelLayoutGroup, IModelWebLayoutGroup)()
            extenders.Add(Of IModelLayoutViewItem, IModelWebLayoutItem)()
        End Sub
    End Class
End Namespace
