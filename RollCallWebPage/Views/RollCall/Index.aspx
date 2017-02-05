<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<RollCallWebPage.Models.ClassInfo>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: ViewData["Title"] %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: ViewData["Title"] %></h2>

    <table>
        <tr>
            <th>
                已有班级
            </th>
            <th>
                负责人
            </th>
            <th>
                手机号
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr class="trContent">
            <td>
                <%: Html.ActionLink(item.ClassName, "ListInfo", new { id = item.ID}) %>
            </td>
            <td>
                <%: item.Admin %>
            </td>
            <td>
                <%: item.Phone %>
            </td>
        </tr>
    
    <% } %>

    </table>

</asp:Content>
