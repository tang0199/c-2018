using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LinkButton homeTopButton = (LinkButton)Master.FindControl("btnHome");
        homeTopButton.Enabled = false;

        BulletedList topMenuButtonList = (BulletedList)Master.FindControl("topMenu");
        if (!IsPostBack)
        {
            topMenuButtonList.Items.Add(new ListItem("Add Courses"));
            topMenuButtonList.Items.Add(new ListItem("Add Student Records"));
        }
        topMenuButtonList.Click += TopMenuButtonList_Click;
    }

    private void TopMenuButtonList_Click(object sender, BulletedListEventArgs e)
    {
        switch (e.Index)
        {
            case 0:
                Response.Redirect("AddCourse.aspx");
                break;
            case 1:
                Response.Redirect("AddStudent.aspx");
                break;
        }
    }
}