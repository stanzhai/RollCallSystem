<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<RollCallWebPage.Models.DetailInfoModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%: ViewData["Title"] %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: ViewData["Title"] %></h2>

    <table>
        <tr>
            <th>
                日期
            </th>
            <th>
                课程
            </th>
            <th>
                记录
            </th>
            <th>
                备注
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr class="trContent">
            <td>
                <%: String.Format("{0:F}", item.Date) %>
            </td>
            <td>
                <%: item.Course %>
            </td>
            <td>
                <%: item.Record %>
            </td>
            <td>
                <%: item.Remark %>
            </td>
        </tr>
    
    <% } %>

    </table>
        <p>
        <%: Html.ActionLink("下载", "DownloadDetailInfo", new { id = ViewData["StudentNo"] })%>
    </p>
</asp:Content>

