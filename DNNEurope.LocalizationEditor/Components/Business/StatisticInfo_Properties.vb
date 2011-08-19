Imports System
Imports System.Runtime.Serialization

Namespace Business
  Partial Public Class StatisticInfo

#Region " Private Members "
#End Region
	
#Region " Constructors "
  Public Sub New()
  End Sub

  Public Sub New(ByVal TextId As Int32, ByVal Locale As String, ByVal UserId As Int32, ByVal Total As Int32)
   Me.Locale = Locale
   Me.TextId = TextId
   Me.Total = Total
   Me.UserId = UserId
  End Sub
#End Region
	
#Region " Public Properties "
  <DataMember()>
  Public Property Locale() As String
  <DataMember()>
  Public Property TextId() As Int32
  <DataMember()>
  Public Property Total() As Int32
  <DataMember()>
  Public Property UserId() As Int32
#End Region

 End Class
End Namespace


