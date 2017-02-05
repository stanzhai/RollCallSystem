<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    关于我们
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>关于</h2>
    <p>
        首先感谢您对点名系统的支持！<br />点名系统的设计主要是为了充分利用计算机资源规范点名，方便数据统计。</p>
    <p>
        如果您是学生，请尝试使用学号作为用户名，使用您的姓名拼音首字母作为密码，登录本系统，选择登录身份为“学生”，以查看班级管理员上传的点名数据。<br />
        如：用户名为“2008145616”，密码为“dsd”。</p>
    <p>
        如果您是教师，则您登录本系统的用户名为班级拼音首字母，登录密码为您所教课程的拼音首字母，选择登录身份为“教师”，以查看某个班级所教课程的点名记录。<br />
        如：用户名为“08jrg2b”(08级软工2班)，密码为“byyl”(编译原理)。</p>
    <p>
        如果您是班级管理员，可使用手机号作为用户名，使用点名系统软件设置的密码作为登录密码，登录本系统，选择登录身份为“管理员”，以查看本班级的所有点名记录。</p>
    <p>
        点名系统作者：翟士丹 @ 曲阜师范大学 zYz Team 火山软件小组<br />
        作者联系邮箱：jazzdan@jazzdan.co.cc<br />
        恳请提交改进意见！</p>
</asp:Content>
