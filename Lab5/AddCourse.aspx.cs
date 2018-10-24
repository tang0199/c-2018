using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentRecordDal;

public partial class AddCourse : BasePage
{
    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        BulletedList topMenu = (BulletedList)Master.FindControl("topMenu");
        topMenu.Items[0].Enabled = false;

        if (!IsPostBack)
        {            
            txtCourseNum.Text = "";
            txtCourseNum.ReadOnly = false;
            txtCourseName.Text = "";
            Session["selectedCourseIndex"] = null;
        }

        string sort = Request.Params["sort"];
        string action = Request.Params["action"];

        using (StudentRecordEntities entityContext = new StudentRecordEntities())
        {
            List<Course> courses = entityContext.Courses.ToList<Course>();

            if (!string.IsNullOrEmpty(sort))
            {
                ShowCourseInfo(courses, sort);
            }
            else
            {
                ShowCourseInfo(courses, "null");
            }

            if (!string.IsNullOrEmpty(action))
            {
                courseAction(courses, action);
            }
        }
    }

    protected void submit_Click(object sender, EventArgs e)
    {
        using (StudentRecordEntities entityContext = new StudentRecordEntities())
        {
            List<Course> courses = entityContext.Courses.ToList<Course>();
            Course course = new Course();
            course.Code = txtCourseNum.Text.Trim();
            course.Title = txtCourseName.Text.Trim();
            bool existCourse = false;

            foreach (Course c in courses)
            {
                existCourse = (String.Compare(c.Code, course.Code) == 0);
                if (existCourse)
                {
                    existCourse = true;
                    break;
                }
            }
            
            if (existCourse == true)
            {
                lblCourseExist.Visible = true;
            }
            else
            {
                lblCourseExist.Visible = false;
                entityContext.Courses.Add(course);
                entityContext.SaveChanges();
                Response.Redirect("AddCourse.aspx");
            }
        }
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
                courses.Sort((a, b) => a.Code.CompareTo(b.Code));
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
                courses.Sort((a, b) => a.Title.CompareTo(b.Title));
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
            cell.Text = c.Code;
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = c.Title;
            row.Cells.Add(cell);

            cell = new TableCell();
            HyperLink hl1 = new HyperLink()
            {
                Text = "Edit",
                NavigateUrl = "AddCourse.aspx?action=edit" + c.Code
            };
            Literal l = new Literal();
            l.Text = " | ";
            HyperLink hl2 = new HyperLink()
            {
                Text = "Delete",
                NavigateUrl = "AddCourse.aspx?action=delete" + c.Code
            };
            hl2.Attributes.Add("onClick", "return confirm('Selected course and its student records will be deleted!');");
            cell.Controls.Add(hl1);
            cell.Controls.Add(l);
            cell.Controls.Add(hl2);
            row.Cells.Add(cell);

            tblCourseRecord.Rows.Add(row);
        }
    } 

    private void courseAction(List<Course> courses, string action)
    {
        foreach (Course c in courses)
        {
            if (action == ("edit" + c.Code))
            {
                using (StudentRecordEntities entityContext = new StudentRecordEntities())
                {
                    Course course = (from co in entityContext.Courses
                                     where co.Code == c.Code
                                     select co).FirstOrDefault<Course>();
                    if (course != null)
                    {
                        txtCourseNum.Text = course.Code;
                        txtCourseNum.ReadOnly = true;
                        if (!IsPostBack)
                        {
                            txtCourseName.Text = course.Title;
                        }

                        submit.Click -= submit_Click;
                        submit.Click += submitEdit_Click;
                        break;
                    }
                }
            }

            else if (action == ("delete" + c.Code))
            {
                using (StudentRecordEntities entityContext = new StudentRecordEntities())
                {
                    Course course = (from co in entityContext.Courses
                                     where co.Code == c.Code
                                     select co).FirstOrDefault<Course>();
                    if (course != null)
                    {
                        for (int i = course.AcademicRecords.Count() - 1; i >= 0; i--)
                        {
                            var ar = course.AcademicRecords.ElementAt<AcademicRecord>(i);
                            course.AcademicRecords.Remove(ar);
                        }

                        entityContext.Courses.Remove(course);
                        entityContext.SaveChanges();
                        Response.Redirect("AddCourse.aspx");
                    }
                }
            }
        }
    }

    protected void submitEdit_Click(object sender, EventArgs e)
    {
        string code = txtCourseNum.Text.Trim();

        using (StudentRecordEntities entityContext = new StudentRecordEntities())
        {
            Course course = (from co in entityContext.Courses
                             where co.Code == code
                             select co).FirstOrDefault<Course>();
            if (course != null)
            {
                course.Title = txtCourseName.Text;
                entityContext.Entry(course).State = EntityState.Modified;
                entityContext.SaveChanges();

                Response.Redirect("AddCourse.aspx");
            }
        }
    }
}