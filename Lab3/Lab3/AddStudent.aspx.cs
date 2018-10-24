using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AlgonquinCollege.Registration.Entities;

public partial class AddStudent : System.Web.UI.Page
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
        topMenuButtonList.Click += (s, a) => Response.Redirect("AddCourse.aspx");
        topMenuButtonList.Items[1].Enabled = false;

        if (Session["courses"] != null)
        {
            List<Course> courses = Session["courses"] as List<Course>;
            courses.Sort((a, b) => int.Parse(a.CourseNumber).CompareTo(int.Parse(b.CourseNumber)));
            if (!IsPostBack)
            {
                foreach (Course course in courses)
                {
                    ddlCourse.Items.Add(course.CourseNumber + " " + course.CourseName);
                }
            }
        }
    }

    protected void addCourse_Click(object sender, EventArgs e)
    {
        Student student = new Student(txtStudentNum.Text, txtStudentName.Text);
        List<Course> courses = Session["courses"] as List<Course>;
        int selectedCourse = ddlCourse.SelectedIndex;

        courses[selectedCourse - 1].AddAcademicRecord(student, 0, 0.ToString(), int.Parse(txtGrade.Text));

        ShowStudentInCourse(courses[selectedCourse - 1].AcademicRecords);
        txtStudentNum.Text = "";
        txtStudentName.Text = "";
        txtGrade.Text = "";
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<Course> courses = Session["courses"] as List<Course>;

        if (ddlCourse.SelectedIndex != 0)
        {
            int selectedCourse = ddlCourse.SelectedIndex;
            ShowStudentInCourse(courses[selectedCourse - 1].AcademicRecords);
        }
    }

    private void ShowStudentInCourse(List<AcademicRecord> records)
    {
        for (int i = tblStudentRecord.Rows.Count - 1; i > 0; i--)
        {
            tblStudentRecord.Rows.RemoveAt(i);
        }

        if (records == null)
        {
            TableRow row = new TableRow();
            TableCell cell = new TableCell();
            cell.Text = "No student in the system yet";
            cell.ForeColor = System.Drawing.Color.Red;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Cells.Add(cell);
            tblStudentRecord.Rows.Add(row);
            return;
        }

        records.Sort((a, b) => CompareRecord(a, b));

        foreach (AcademicRecord record in records)
        {
            TableRow row = new TableRow();

            TableCell cell = new TableCell();
            cell.Text = record.Student.Id;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = record.Student.Name;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = record.Grade.ToString();
            row.Cells.Add(cell);

            tblStudentRecord.Rows.Add(row);
        }
    }

    public int CompareRecord(AcademicRecord record1, AcademicRecord record2)
    {
        if (record1 == null && record2 != null)
            return -1;
        if (record1 != null && record2 == null)
            return 1;
        if (record1 == null && record2 == null)
            return 0;

        string[] record1Name;
        string[] record2Name;
        record1Name = record1.Student.Name.Split(' ');
        record2Name = record2.Student.Name.Split(' ');

        if (record1Name[1].CompareTo(record2Name[1]) > 0)
        {
            return 1;
        }
        else if (record1Name[1].CompareTo(record2Name[1]) < 0)
        {
            return -1;
        }
        else
        {
            if (record1Name[0].CompareTo(record2Name[0]) > 0)
            {
                return 1;
            }
            else if (record1Name[0].CompareTo(record2Name[0]) < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
