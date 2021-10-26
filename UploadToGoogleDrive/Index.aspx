<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="UploadToGoogleDrive.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <p>
        <br />
    </p>
    <p>
    </p>
    <p>
        <asp:FileUpload ID="fileUpload" runat="server" />
    </p>
    <p>
        <asp:Button ID="BtnAddToDrive" runat="server" Text="Upload To Drive" OnClick="BtnAddToDrive_Click" />
    </p>
    <p>
        <asp:Label ID="lblOutput" runat="server" ></asp:Label>
    </p>
    
</asp:Content>
