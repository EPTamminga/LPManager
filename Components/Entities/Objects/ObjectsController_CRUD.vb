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

Namespace Entities.Objects

 Partial Public Class ObjectsController

  Public Shared Function GetObject(ByVal ObjectId As Int32) As ObjectInfo

   Return CType(CBO.FillObject(DataProvider.Instance().GetObject(ObjectId), GetType(ObjectInfo)), ObjectInfo)

  End Function

  Public Shared Function AddObject(ByVal objObject As ObjectInfo) As Integer

   objObject.ObjectId = CType(DataProvider.Instance().AddObject(objObject.FriendlyName, objObject.ModuleId, objObject.InstallPath, objObject.ObjectName, objObject.PackageType), Integer)
   Return objObject.ObjectId

  End Function

  Public Shared Sub DeleteObject(ByVal ObjectId As Int32)

   DataProvider.Instance().DeleteObject(ObjectId)

  End Sub

 End Class
End Namespace

