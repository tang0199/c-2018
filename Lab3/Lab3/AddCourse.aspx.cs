using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgonquinCollege.Registration.Entities;

public partial class AddCourse : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LinkButton homeTopButton = (LinkButton)Master.FindControl("btnHome");
        homeTopButton.Click += (s, a) => Response.Redirect("Default.aspx");

        BulletedList topMenuButtonList = (BulletedList)Master.FindControl("topMenu");
        if (!IsPostBack)
        {
            topMenuButtonList.Items.Add(new ListItem("Add Course"));
            topMenuButtonList.Items.Add(new ListItem("Add Student Records"));
        }
        topMenuButtonList.Items[0].Enabled = false;
        topMenuButtonList.Click += (s, a) => Response.Redirect("AddStudent.aspx");

        List<Course> courses = Session["courses"] as List<Course>;

        string sort = Request.Params["sort"];

        if (!string.IsNullOrEmpty(sort))
        {
            ShowCourseInfo(courses, sort);
        }

        ShowCourseInfo(courses, "null");
    }


    protected void submit_Click(object sender, EventArgs e)
    {
        List<Course> courseList;
        if (Session["courses"] == null)
        {
            courseList = new List<Course>();
            Session["courses"] = courseList;
        }
        else
        {
            courseList = (List<Course>)Session["courses"];
        }
        courseList.Add(new Course(txtCourseNum.Text, txtCourseName.Text));
        Session["courses"] = courseList;

        for (int i = tblCourseRecord.Rows.Count - 1; i > 0; i--)
        {
            tblCourseRecord.Rows.RemoveAt(i);
        }

        foreach (Course c in courseList)
        {
            TableRow row = new TableRow();

            TableCell cell = new TableCell();
            cell.Text = c.CourseNumber;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = c.CourseName;
            row.Cells.Add(cell);

            tblCourseRecord.Rows.Add(row);
        }

        txtCourseNum.Text = "";
        txtCourseName.Text = "";
    }

    private void ShowCourseInfo(List<Course> courses, string sort)
    {
        bool reverseCode = false;
        bool reverseTitle = false;

        if (Session["reverseCode"] == null)
        {
            Session["reverseCode"] = reverseCode;
        }
        else
        {
            reverseCode = (bool)Session["reverseCode"];
        }

        if (Session["reverseTitle"] == null)
        {
            Session["reverseTitle"] = reverseTitle;
        }
        else
        {
            reverseTitle = (bool)Session["reverseTitle"];
        }

        for (int i = tblCourseRecord.Rows.Count - 1; i > 0; i--)
        {
            tblCourseRecord.Rows.RemoveAt(i);
        }

        if ((courses == null) || (courses.Count == 0))
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "No course in the system yet";
            cell.ForeColor = System.Drawing.Color.Red;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Cells.Add(cell);
            tblCourseRecord.Rows.Add(row);
            return;
        }

        if (sort == "code")
        {
            if (reverseCode == false)
            {
                courses.Sort((a, b) => int.Parse(a.CourseNumber).CompareTo(int.Parse(b.CourseNumber)));
                reverseCode = true;
            }
            else
            {
                courses.Reverse();
                reverseCode = false;
            }
            Session["reverseCode"] = reverseCode;
        }
        else if (sort == "title")
        {
            if (reverseTitle == false)
            {
                courses.Sort((a, b) => a.CourseName.CompareTo(b.CourseName));
                reverseTitle = true;
            }
            else
            {
                courses.Reverse();
                reverseTitle = false;
            }
            Session["reverseTitle"] = reverseTitle;
        }

        foreach (Course c in courses)
        {
            TableRow row = new TableRow();

            TableCell cell = new TableCell();
            cell.Text = c.CourseNumber;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = c.CourseName;
            row.Cells.Add(cell);

            tblCourseRecord.Rows.Add(row);
        }
    }
}