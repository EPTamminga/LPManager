Imports System
Imports System.Runtime.Serialization

Namespace Business
  Partial Public Class ObjectCoreVersionInfo

#Region " Private Members "
#End Region
	
#Region " Constructors "
  Public Sub New()
  End Sub

  Public Sub New(ByVal ObjectId As Int32, ByVal Version As String, ByVal CoreVersion As String, ByVal InstalledByDefault As Boolean)
   Me.CoreVersion = CoreVersion
   Me.InstalledByDefault = InstalledByDefault
   Me.ObjectId = ObjectId
   Me.Version = Version
  End Sub
#End Region
	
#Region " Public Properties "
  <DataMember()>
  Public Property CoreVersion() As String
  <DataMember()>
  Public Property InstalledByDefault() As Boolean
  <DataMember()>
  Public Property ObjectId() As Int32
  <DataMember()>
  Public Property Version() As String
#End Region

 End Class
End Namespace


