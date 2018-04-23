Imports System
Imports System.Linq
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.BaseImpl
Imports WebLayoutSolution.Module.BusinessObjects

Namespace WebLayoutSolution.Module.DatabaseUpdate
    ' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub
        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            If ObjectSpace.FindObject(Of BusinessObject)(Nothing) Is Nothing Then
                Dim obj = ObjectSpace.CreateObject(Of BusinessObject)()
                obj.FieldA = "aaa"
                obj.FieldB = "bbb"
                obj.FieldC = "ccc"
                obj.FieldD = "ddd"
                obj.FieldE = "eee"
                obj.FieldF = "fff"
                obj.FieldG = "ggg"
                obj.FieldH = "hhh"
                obj.FieldI = "iii"
                obj.FieldJ = "jjj"
                obj.FieldK = "kkk"
                obj.FieldL = "lll"
                obj.FieldM = "mmm"
                obj.FieldN = "nnn"
            End If
        End Sub
        Public Overrides Sub UpdateDatabaseBeforeUpdateSchema()
            MyBase.UpdateDatabaseBeforeUpdateSchema()
            'if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            '    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            '}
        End Sub
    End Class
End Namespace
