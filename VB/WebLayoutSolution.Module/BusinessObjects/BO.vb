Imports DevExpress.ExpressApp.Model
Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo

Namespace WebLayoutSolution.Module.BusinessObjects

    <NavigationItem>
    Public Class BusinessObject
        Inherits BaseObject

        Public Sub New(ByVal s As Session)
            MyBase.New(s)
        End Sub

        <ModelDefault("Caption", "Abcd")>
        Public Property FieldA As String

        <ModelDefault("Caption", "Efghijklmn")>
        Public Property FieldB As String

        Public Property FieldC As String

        Public Property FieldD As String

        Public Property FieldE As String

        <ModelDefault("Caption", "A long caption")>
        Public Property FieldF As String

        Public Property FieldG As String

        Public Property FieldH As String

        <ModelDefault("Caption", "Another long caption")>
        Public Property FieldI As String

        Public Property FieldJ As String

        Public Property FieldK As String

        Public Property FieldL As String

        Public Property FieldM As String

        <ModelDefault("Caption", "A very very very very very long caption")>
        Public Property FieldN As String
    End Class
End Namespace
