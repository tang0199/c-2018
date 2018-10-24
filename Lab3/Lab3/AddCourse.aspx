<%@ Page Title="" Language="C#" MasterPageFile="~/ACMasterPage.master" AutoEventWireup="true" CodeFile="AddCourse.aspx.cs" Inherits="AddCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="App_Themes/SiteStyles.css" rel="stylesheet" />
    <h1>Add New Courses</h1>
    <div>
        <asp:Label ID="lblCourseNum" runat="server" Text="Course Number:" Height="30px" Width="150px"></asp:Label>
            <asp:TextBox ID="txtCourseNum" runat="server" Height="25px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valCourseNum" runat="server" ControlToValidate="txtCourseNum" ErrorMessage="Course Number cannot be empty" CssClass="error" Display="Dynamic" Height="30px"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblCourseName" runat="server" Text="Course Name:" Width="150px" Height="30px"></asp:Label>
            <asp:TextBox ID="txtCourseName" runat="server" Height="25px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valCourseName" runat="server" ErrorMessage="Course Name cannot be empty" ControlToValidate="txtCourseName" CssClass="error" Display="Dynamic" Height="30px"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="submit" runat="server" Text="Submit Course Information" OnClick="submit_Click" />
    </div>
    <div>
        <h3>The following courses are currently in the system:</h3>
        <asp:Table runat="server" ID="tblCourseRecord" CssClass="table">
            <asp:TableRow>
                <asp:TableHeaderCell><a href="AddCourse.aspx?sort=code" />Course Code</asp:TableHeaderCell>
                <asp:TableHeaderCell><a href="AddCourse.aspx?sort=title" />Course Title</asp:TableHeaderCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>

