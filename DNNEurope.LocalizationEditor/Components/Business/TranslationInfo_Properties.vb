Imports System
Imports System.Runtime.Serialization

Namespace Business
  Partial Public Class TranslationInfo

#Region " Private Members "
#End Region
	
#Region " Constructors "
  Public Sub New()
  End Sub

  Public Sub New(ByVal TextId As Int32, ByVal Locale As String, ByVal LastModified As Date, ByVal LastModifiedUserId As Int32, ByVal TextValue As String)
   Me.LastModified = LastModified
   Me.LastModifiedUserId = LastModifiedUserId
   Me.Locale = Locale
   Me.TextId = TextId
   Me.TextValue = TextValue
  End Sub
#End Region
	
#Region " Public Properties "
  <DataMember()>
  Public Property LastModified() As Date
  <DataMember()>
  Public Property LastModifiedUserId() As Int32
  <DataMember()>
  Public Property Locale() As String
  <DataMember()>
  Public Property TextId() As Int32
  <DataMember()>
  Public Property TextValue() As String
#End Region

 End Class
End Namespace


