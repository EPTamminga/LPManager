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
Imports DotNetNuke.Common.Utilities
Imports DNNEurope.Modules.LocalizationEditor.Data

Namespace Entities.PartnerPacks

 Partial Public Class PartnerPacksController

  Public Shared Function GetPartnerPack(ByVal PartnerId As Int32, ByVal ObjectId As Int32, ByVal Version As String, ByVal Locale As String) As PartnerPackInfo

   Return CType(CBO.FillObject(DataProvider.Instance().GetPartnerPack(PartnerId, ObjectId, Version, Locale), GetType(PartnerPackInfo)), PartnerPackInfo)

  End Function

  Public Shared Sub AddPartnerPack(ByVal objPartnerPack As PartnerPackInfo)

   DataProvider.Instance().AddPartnerPack(objPartnerPack.LastModified, objPartnerPack.Locale, objPartnerPack.ObjectId, objPartnerPack.PartnerId, objPartnerPack.PercentComplete, objPartnerPack.RemoteObjectId, objPartnerPack.Version)

  End Sub

  Public Shared Sub UpdatePartnerPack(ByVal objPartnerPack As PartnerPackInfo)

   DataProvider.Instance().UpdatePartnerPack(objPartnerPack.LastModified, objPartnerPack.Locale, objPartnerPack.ObjectId, objPartnerPack.PartnerId, objPartnerPack.PercentComplete, objPartnerPack.RemoteObjectId, objPartnerPack.Version)

  End Sub

  Public Shared Sub DeletePartnerPack(ByVal PartnerId As Int32, ByVal ObjectId As Int32, ByVal Version As String, ByVal Locale As String)

   DataProvider.Instance().DeletePartnerPack(PartnerId, ObjectId, Version, Locale)

  End Sub

 End Class
End Namespace

