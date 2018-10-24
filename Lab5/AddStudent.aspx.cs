using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentRecordDal;

public partial class AddStudent : BasePage
{
    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        BulletedList topMenu = (BulletedList)Master.FindControl("topMenu");
        topMenu.Items[1].Enabled = false;

        if (!IsPostBack)
        {
            ddlCourse.SelectedIndex = 0;
            txtStudentNum.ReadOnly = false;
            txtStudentNum.Text = "";
            txtStudentNum.ReadOnly = false;
            txtStudentName.Text = "";
            txtStudentName.ReadOnly = false;
            txtGrade.Text = "";
        }

        string action = Request.Params["action"];

        using (StudentRecordEntities entityContext = new StudentRecordEntities())
        {
            List<Course> courses = entityContext.Courses.ToList<Course>();
            courses.Sort((a, b) => a.Code.CompareTo(b.Code));
            if (!IsPostBack)
            {                
                foreach (Course course in courses)
                {
                    ddlCourse.Items.Add(course.Code + " " + course.Title);
                }

                if (Session["selectedCourseIndex"] != null)
                {
                    ddlCourse.SelectedIndex = (int)Session["selectedCourseIndex"];
                    ShowStudentInCourse(courses[(int)Session["selectedCourseIndex"] - 1].AcademicRecords.ToList());
                }
            }

            if (!string.IsNullOrEmpty(action))
            {
                studentAction(courses, action);
            }        
        }
    }

    protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (StudentRecordEntities entityContext = new StudentRecordEntities())
        {
            List<Course> courses = entityContext.Courses.ToList<Course>();
            if (ddlCourse.SelectedIndex != 0)
            {
                int selectedCourse = ddlCourse.SelectedIndex;
                Session["selectedCourseIndex"] = selectedCourse;
                ShowStudentInCourse(courses[selectedCourse - 1].AcademicRecords.ToList());
            }
        }
    }

    private void ShowStudentInCourse(List<AcademicRecord> records)
    {
        for (int i = tblStudentRecord.Rows.Count - 1; i > 0; i--)
        {
            tblStudentRecord.Rows.RemoveAt(i);
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

            cell = new TableCell();
            HyperLink hl1 = new HyperLink()
            {
                Text = "Change Grade",
                NavigateUrl = "AddStudent.aspx?action=change" + record.StudentId
            };
            Literal l = new Literal();
            l.Text = " | ";
            HyperLink hl2 = new HyperLink()
            {
                Text = "Delete",
                NavigateUrl = "AddStudent.aspx?action=deleteRecord" + record.StudentId
            };
            hl2.Attributes.Add("onClick", "return confirm('Selected academic record will be deleted!');");
            cell.Controls.Add(hl1);
            cell.Controls.Add(l);
            cell.Controls.Add(hl2);
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

        string record1Id;
        string record2Id;
        record1Id = record1.Student.Id;
        record2Id = record2.Student.Id;

        if (record1Id.CompareTo(record2Id) > 0)
        {
            return 1;
        }
        else if (record1Id.CompareTo(record2Id) < 0)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    protected void addCourse_Click(object sender, EventArgs e)
    {
        using (StudentRecordEntities entityContext = new StudentRecordEntities())
        {            
            List<Course> courses = entityContext.Courses.ToList<Course>();
            int selectedCourse = ddlCourse.SelectedIndex;
            bool existStudent = false;

            List<AcademicRecord> ar = courses[selectedCourse - 1].AcademicRecords.ToList<AcademicRecord>();
            foreach (AcademicRecord record in ar)
            {
                if ((record.StudentId == txtStudentNum.Text)&&
                    (record.CourseCode == courses[selectedCourse - 1].Code))
                {
                    existStudent = true;
                    break;
                }
            }

            if (existStudent == true)
            {
                lblStudentExist.Visible = true;
                ShowStudentInCourse(ar);
            }
            else
            {
                lblStudentExist.Visible = false;
                Student student = new Student();
                student.Id = txtStudentNum.Text;
                student.Name = txtStudentName.Text;
                AcademicRecord record = new AcademicRecord();
                record.StudentId = txtStudentNum.Text;
                record.CourseCode = courses[selectedCourse - 1].Code;
                record.Grade = int.Parse(txtGrade.Text);

                List<Student> students = entityContext.Students.ToList<Student>();
                bool flag = false;
                foreach (Student s in students)
                {
                    if (record.StudentId == s.Id)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag == false)
                {
                    record.Student = student;
                }
                
                record.Course = courses[selectedCourse - 1];

                entityContext.AcademicRecords.Add(record);
                entityContext.SaveChanges();

                Session["selectedCourseIndex"] = selectedCourse;
                Response.Redirect("AddStudent.aspx");
            }
        }
    }

    private void studentAction(List<Course> courses, string action)
    {
        int selectedCourse = ddlCourse.SelectedIndex;

        if (selectedCourse != 0)
        {
            List<AcademicRecord> records = courses[selectedCourse - 1].AcademicRecords.ToList<AcademicRecord>();
            Session["selectedCourseIndex"] = selectedCourse;

            foreach (AcademicRecord a in records)
            {
                if (action == ("change" + a.StudentId))
                {
                    using (StudentRecordEntities entityContext = new StudentRecordEntities())
                    {
                        AcademicRecord record = (from ar in entityContext.AcademicRecords
                                                 where ar.StudentId == a.StudentId && ar.CourseCode == a.CourseCode
                                                 select ar).FirstOrDefault<AcademicRecord>();

                        if (record != null)
                        {
                            ddlCourse.SelectedIndex = selectedCourse;
                            ddlCourse.Enabled = false;
                            txtStudentNum.Text = record.StudentId;
                            txtStudentNum.ReadOnly = true;
                            txtStudentName.Text = record.Student.Name;
                            txtStudentName.ReadOnly = true;
                            if (!IsPostBack)
                            {
                                txtGrade.Text = record.Grade.ToString(); 
                            }
                            
                            addCourse.Click -= addCourse_Click;
                            addCourse.Click += addCourseEdit_Click;
                            break;
                        }
                    }
                }
                else if (action == ("deleteRecord" + a.StudentId))
                {
                    using (StudentRecordEntities entityContext = new StudentRecordEntities())
                    {
                        AcademicRecord record = (from ar in entityContext.AcademicRecords
                                                 where ar.StudentId == a.StudentId
                                                 select ar).FirstOrDefault<AcademicRecord>();

                        if (record != null)
                        {
                            entityContext.AcademicRecords.Remove(record);
                            entityContext.SaveChanges();                            

                            Response.Redirect("AddStudent.aspx");
                        }
                    }
                }
            }
        }        
    }

    protected void addCourseEdit_Click(object sender, EventArgs e)
    {
        string id = txtStudentNum.Text.Trim();
        int selectedCourse = ddlCourse.SelectedIndex;

        using (StudentRecordEntities entityContext = new StudentRecordEntities())
        {
            List<Course> courses = entityContext.Courses.ToList<Course>();

            string code = courses[selectedCourse - 1].Code;

            AcademicRecord record = (from ar in entityContext.AcademicRecords
                                     where ar.StudentId == id && ar.CourseCode == code
                                     select ar).FirstOrDefault<AcademicRecord>();

            if (record != null)
            {
                record.Grade = int.Parse(txtGrade.Text);
                entityContext.Entry(record).State = EntityState.Modified;
                entityContext.SaveChanges();

                Response.Redirect("AddStudent.aspx");
            }
        }
    }
}