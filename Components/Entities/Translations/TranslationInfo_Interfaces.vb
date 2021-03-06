' 
' Copyright (c) 2004-2011 DNN-Europe, http://www.dnn-europe.net
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this 
' software and associated documentation files (the "Software"), to deal in the Software 
' without restriction, including without limitation the rights to use, copy, modify, merge, 
' publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
' to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or 
' substantial portions of the Software.

' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
' INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
' PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
' FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
' ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
' 
Imports System
Imports System.Data
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Tokens

Namespace Entities.Translations
  <Serializable(), XmlRoot("Translation"), DataContract()>
  Partial Public Class TranslationInfo
   Implements IHydratable
   Implements IPropertyAccess
   Implements IXmlSerializable

#Region " IHydratable Implementation "
 ''' -----------------------------------------------------------------------------
 ''' <summary>
 ''' Fill hydrates the object from a Datareader
 ''' </summary>
 ''' <remarks>The Fill method is used by the CBO method to hydrtae the object
 ''' rather than using the more expensive Refection  methods.</remarks>
 ''' <history>
 ''' 	[pdonker]	08/07/2011  Created
 ''' </history>
 ''' -----------------------------------------------------------------------------
 Public Sub Fill(ByVal dr As IDataReader) Implements IHydratable.Fill

  LastModified = CDate(Null.SetNull(dr.Item("LastModified"), LastModified))
  LastModifiedUserId = Convert.ToInt32(Null.SetNull(dr.Item("LastModifiedUserId"), LastModifiedUserId))
  Locale = Convert.ToString(Null.SetNull(dr.Item("Locale"), Locale))
  TextId = Convert.ToInt32(Null.SetNull(dr.Item("TextId"), TextId))
  TextValue = Convert.ToString(Null.SetNull(dr.Item("TextValue"), TextValue))

 End Sub
 ''' -----------------------------------------------------------------------------
 ''' <summary>
 ''' Gets and sets the Key ID
 ''' </summary>
 ''' <remarks>The KeyID property is part of the IHydratble interface.  It is used
 ''' as the key property when creating a Dictionary</remarks>
 ''' <history>
 ''' 	[pdonker]	08/07/2011  Created
 ''' </history>
 ''' -----------------------------------------------------------------------------
 Public Property KeyID() As Integer Implements IHydratable.KeyID
  Get
   Return Nothing
  End Get
  Set(ByVal value As Integer)
  End Set
 End Property
#End Region

#Region " IPropertyAccess Implementation "
 Public Function GetProperty(ByVal strPropertyName As String, ByVal strFormat As String, ByVal formatProvider As System.Globalization.CultureInfo, ByVal AccessingUser As DotNetNuke.Entities.Users.UserInfo, ByVal AccessLevel As DotNetNuke.Services.Tokens.Scope, ByRef PropertyNotFound As Boolean) As String Implements DotNetNuke.Services.Tokens.IPropertyAccess.GetProperty
  Dim OutputFormat As String = String.Empty
  Dim portalSettings As DotNetNuke.Entities.Portals.PortalSettings = DotNetNuke.Entities.Portals.PortalController.GetCurrentPortalSettings()
  If strFormat = String.Empty Then
   OutputFormat = "D"
  Else
   OutputFormat = strFormat
  End If
  Select Case strPropertyName.ToLower
   Case "lastmodified"
    Return (Me.LastModified.ToString(OutputFormat, formatProvider))
   Case "lastmodifieduserid"
    Return (Me.LastModifiedUserId.ToString(OutputFormat, formatProvider))
   Case "locale"
    Return PropertyAccess.FormatString(Me.Locale, strFormat)
   Case "textid"
    Return (Me.TextId.ToString(OutputFormat, formatProvider))
   Case "textvalue"
    Return PropertyAccess.FormatString(Me.TextValue, strFormat)
   Case Else
    PropertyNotFound = True
  End Select

  Return Null.NullString
 End Function

 Public ReadOnly Property Cacheability() As DotNetNuke.Services.Tokens.CacheLevel Implements DotNetNuke.Services.Tokens.IPropertyAccess.Cacheability
  Get
   Return CacheLevel.fullyCacheable
  End Get
 End Property
#End Region

#Region " IXmlSerializable Implementation "
  ''' -----------------------------------------------------------------------------
  ''' <summary>
  ''' GetSchema returns the XmlSchema for this class
  ''' </summary>
  ''' <remarks>GetSchema is implemented as a stub method as it is not required</remarks>
  ''' <history>
  ''' 	[pdonker]	08/07/2011  Created
  ''' </history>
  ''' -----------------------------------------------------------------------------
  Public Function GetSchema() As XmlSchema Implements IXmlSerializable.GetSchema
   Return Nothing
  End Function

  Private Function readElement(ByVal reader As XmlReader, ByVal ElementName As String) As String
   If (Not reader.NodeType = XmlNodeType.Element) OrElse reader.Name <> ElementName Then
    reader.ReadToFollowing(ElementName)
   End If
   If reader.NodeType = XmlNodeType.Element Then
    Return reader.ReadElementContentAsString
   Else
    Return ""
   End If
  End Function

  ''' -----------------------------------------------------------------------------
  ''' <summary>
  ''' ReadXml fills the object (de-serializes it) from the XmlReader passed
  ''' </summary>
  ''' <remarks></remarks>
  ''' <param name="reader">The XmlReader that contains the xml for the object</param>
  ''' <history>
  ''' 	[pdonker]	08/07/2011  Created
  ''' </history>
  ''' -----------------------------------------------------------------------------
  Public Sub ReadXml(ByVal reader As XmlReader) Implements IXmlSerializable.ReadXml
   Try

   Date.TryParse(readElement(reader, "LastModified"), LastModified)
    If Not Int32.TryParse(readElement(reader, "LastModifiedUserId"), LastModifiedUserId) Then
     LastModifiedUserId = Null.NullInteger
    End If
    TextValue = readElement(reader, "TextValue")
   Catch ex As Exception
    ' log exception as DNN import routine does not do that
    DotNetNuke.Services.Exceptions.LogException(ex)
    ' re-raise exception to make sure import routine displays a visible error to the user
    Throw New Exception("An error occured during import of an Translation", ex)
   End Try

  End Sub

  ''' -----------------------------------------------------------------------------
  ''' <summary>
  ''' WriteXml converts the object to Xml (serializes it) and writes it using the XmlWriter passed
  ''' </summary>
  ''' <remarks></remarks>
  ''' <param name="writer">The XmlWriter that contains the xml for the object</param>
  ''' <history>
  ''' 	[pdonker]	08/07/2011  Created
  ''' </history>
  ''' -----------------------------------------------------------------------------
  Public Sub WriteXml(ByVal writer As XmlWriter) Implements IXmlSerializable.WriteXml
   writer.WriteStartElement("Translation")
   writer.WriteElementString("TextId",  TextId.ToString())
   writer.WriteElementString("Locale",  Locale)
   writer.WriteElementString("LastModified",  LastModified.ToString())
   writer.WriteElementString("LastModifiedUserId",  LastModifiedUserId.ToString())
   writer.WriteElementString("TextValue",  TextValue)
   writer.WriteEndElement()
  End Sub
#End Region

#Region " ToXml Methods "
  Public Function ToXml() As String
   Return ToXml("Translation")
  End Function

  Public Function ToXml(ByVal elementName As String) As String
   Dim xml As New StringBuilder
   xml.Append("<")
   xml.Append(elementName)
   AddAttribute(xml, "TextId", TextId.ToString())
   AddAttribute(xml, "Locale", Locale)
   AddAttribute(xml, "LastModified", LastModified.ToUniversalTime.ToString("u"))
   AddAttribute(xml, "LastModifiedUserId", LastModifiedUserId.ToString())
   AddAttribute(xml, "TextValue", TextValue)
   xml.Append(" />")
   Return xml.ToString
  End Function

  Private Sub AddAttribute(ByRef xml As StringBuilder, ByVal attributeName As String, ByVal attributeValue As String)
   xml.Append(" " & attributeName)
   xml.Append("=""" & attributeValue & """")
  End Sub
#End Region

#Region " JSON Serialization "
  Public Sub WriteJSON(ByRef s As Stream)
   Dim ser As New DataContractJsonSerializer(GetType(TranslationInfo))
   ser.WriteObject(s, Me)
  End Sub
#End Region

 End Class
End Namespace


