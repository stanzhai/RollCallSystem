<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<RollCallWebPage.Models.ClassInfo>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%:ViewData["Title"] %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%:ViewData["Title"] %></h2>

    <table>
        <tr>
            <th>
                班级名称
            </th>
            <th>
                点名负责人
            </th>
            <th>
                负责人联系方式
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr class="trContent">
            <td>
                <%: Html.ActionLink(item.ClassName, "DetailInfo", new { id = item.ID, id2 = ViewData["StudentNo"]}) %>
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

