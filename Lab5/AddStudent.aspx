<%@ Page Title="" Language="C#" MasterPageFile="~/ACMasterPage.master" AutoEventWireup="true" CodeFile="AddStudent.aspx.cs" Inherits="AddStudent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="App_Themes/SiteStyles.css" rel="stylesheet" />
    <h1>Add Student Records</h1>
    <div>
        <asp:Label ID="lblCourse" runat="server" Text="Course:" Height="30px" Width="150px"></asp:Label>
        <asp:DropDownList ID="ddlCourse" runat="server" Height="25px" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
            <asp:ListItem Value="0">Please select one course</asp:ListItem>
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="requiredValidatorCourseList" runat="server" ControlToValidate="ddlCourse" InitialValue="-1" ErrorMessage="RequiredFieldValidator" ForeColor="Red"></asp:RequiredFieldValidator>
        <br />
        <asp:Label ID="lblStudentNum" runat="server" Text="Student Number:" Height="30px" Width="150px"></asp:Label>
        <asp:TextBox ID="txtStudentNum" runat="server" Height="25px" Width="200px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="requiredValidatorStudentNum" runat="server" ControlToValidate="txtStudentNum" ErrorMessage="Student Number cannot be empty" CssClass="error" Display="Dynamic" Height="30px"></asp:RequiredFieldValidator>
        <asp:Label ID="lblStudentExist" runat="server" ForeColor="Red" Text="The system already has record of this student for the selected course" Enabled="True" Visible="False"></asp:Label>
        <br />
        <asp:Label ID="lblStudentName" runat="server" Text="Student Name:" Width="150px" Height="30px"></asp:Label>
        <asp:TextBox ID="txtStudentName" runat="server" Height="25px" Width="200px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="requiredValidatorStudentName" runat="server" ErrorMessage="Student Name cannot be empty" ControlToValidate="txtStudentName" CssClass="error" Display="Dynamic" Height="30px"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="regularExpValidatorStudentName" ValidationExpression="[a-zA-Z]+\s+[a-zA-Z]+" ControlToValidate="txtStudentName" CssClass="error" Display="Dynamic" ErrorMessage="Must be in first_name last_name!" runat="server" Height="30px"/>
        <br />
        <asp:Label ID="lblGrade" runat="server" Text="Grade(0-100):" Width="150px" Height="30px"></asp:Label>
        <asp:TextBox ID="txtGrade" runat="server" Height="25px" Width="200px"></asp:TextBox>
        <asp:RequiredFieldValidator ID="requiredValidatorGrade" runat="server" ErrorMessage="Grade cannot be empty" ControlToValidate="txtGrade" CssClass="error" Display="Dynamic" Height="30px"></asp:RequiredFieldValidator>
        <asp:RangeValidator ID="rangeValidatorGrade" runat="server" ErrorMessage="Grade must be an integer from 0 to 100 inclusive" ControlToValidate="txtGrade" CssClass="error" Height="30px" MaximumValue="100" MinimumValue="0" Type="Integer"></asp:RangeValidator>
        <br />
        <asp:Button ID="addCourse" runat="server" Text="Add to Course" OnClick="addCourse_Click" />
    </div>
    <div>
        <p>The selected course has the following student records:</p>
        <asp:Table runat="server" ID="tblStudentRecord" CssClass="table">
            <asp:TableRow>
                <asp:TableHeaderCell>ID</asp:TableHeaderCell>
                <asp:TableHeaderCell>Name</asp:TableHeaderCell>    
                <asp:TableHeaderCell>Grade</asp:TableHeaderCell>
                <asp:TableHeaderCell></asp:TableHeaderCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" Runat="Server"> 
</asp:Content>

