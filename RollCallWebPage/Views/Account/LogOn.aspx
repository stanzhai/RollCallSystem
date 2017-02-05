<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RollCallWebPage.Models.UserValidate>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    登录
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
<script src="../../Scripts/MicrosoftAjax.js" type="text/javascript"></script>
<script src="../../Scripts/MicrosoftMvcValidation.js" type="text/javascript"></script>
<% Html.EnableClientValidation(); %>

    <h2>登录</h2>
     <%:Html.ValidationSummary("请更正以下错误信息：") %>
    <%Html.BeginForm(); %>
    <div>
        <fieldset>
            <legend>登录</legend>

            <div class="editor-label">
            用户名：
            </div>
            <div class="editor-field">
            <%:Html.TextBox("UserName") %>
            <%: Html.ValidationMessage("UserName") %>
            </div>

            <div class="editor-label">
            密码：
            </div>
            <div class="editor-field">
            <%:Html.Password("Password") %>
            <%: Html.ValidationMessage("Password") %>
            </div>    

            <div class="editor-label">
            身份：
            <%:Html.DropDownList("Identity", 
                from i in 
                    (new SelectListItem[] {
                        new SelectListItem { Text = "学生", Value = "Student", Selected = true}, 
                        new SelectListItem { Text = "教师", Value = "Teacher"},
                        new SelectListItem { Text = "管理员", Value = "Admin"}}) 
                select i)%>
            </div>  
            <div class="editor-label">
                    <%: Html.CheckBoxFor(m => m.RememberMe) %>
                    <%: Html.LabelFor(m => m.RememberMe) %>
            </div>  
            <p>
                <input type="submit" value="登录" />
            </p>                               
        </fieldset>
    </div>

    <%Html.EndForm(); %>
</asp:Content>
