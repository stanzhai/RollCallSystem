<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	主页内容编辑
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>主页内容编辑</h2>

    <script src="../../Content/ckeditor/ckeditor.js" type="text/javascript"></script>
    <script src="../../Content/ckfinder/ckfinder.js" type="text/javascript"></script>

    <% Html.BeginForm(); %>

    <%: Html.TextArea("Content") %>

    <% Html.EndForm(); %>

    <script type="text/javascript">
        var editor = CKEDITOR.replace('Content');
        CKFinder.setupCKEditor(editor, '../content/ckfinder/');
    </script>

</asp:Content>
