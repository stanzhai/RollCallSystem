<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<RollCallWebPage.Models.ListInfoModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%: ViewData["Title"] %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%: ViewData["Title"] %></h2>
    <table id="table">
        <tr>
            <th>
                学号
            </th>
            <th>
                姓名
            </th>
            <th>
                迟到
            </th>
            <th>
                旷课
            </th>
            <th>
                请假
            </th>
            <th>
                早退
            </th>
        </tr>
        <% foreach (var item in Model)
           { %>
        <tr class="trContent">
            <td>
                    <%: Html.ActionLink(item.StudentNo.ToString(), "DetailInfo", new { id = ViewData["ClassID"], id2 = item.StudentNo} )%>
            </td>
            <td>
                <%: item.Name %>
            </td>
            <td>
                <%: item.ChiDaoCount %>
            </td>
            <td>
                <%: item.KuangKeCount %>
            </td>
            <td>
                <%: item.QingJiaCount %>
            </td>
            <td>
                <%: item.ZaoTuiCount %>
            </td>
        </tr>
        <% } %>
    </table>
    <p>
        <%if (Page.User.IsInRole("Teacher"))
          {
        %>
       
        <%
            }
          else
          {
        %>
        <%: Html.ActionLink("下载", "DownloadListInfo", new { id = ViewData["ClassID"]})%>
        <%
            } %>
    </p>
</asp:Content>
