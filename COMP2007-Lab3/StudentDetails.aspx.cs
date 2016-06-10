using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements required for EF DB access
using COMP2007_Lab3.Models;
using System.Web.ModelBinding;

namespace COMP2007_Lab3
{
    public partial class StudentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Max and Min value for Range Validator here
            //to avoid Date Conversion issue with system date format
            RangeValidator1.MinimumValue = DateTime.Today.AddYears(-16).ToShortDateString();
            RangeValidator1.MaximumValue = DateTime.Today.ToShortDateString();

            if((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetStudent();
            }
        }

        protected void GetStudent()
        {
            // populate the form with existing data from the database
            int StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

            // connect to the EF DB
            using (DefaultConnection db = new DefaultConnection())
            {
                // populate a student object instance with the StudentID from the URL Parameter
                Student updateStudent = (from student in db.Students
                                         where student.StudentID == StudentID
                                         select student).FirstOrDefault();

                //map the student properties to the form control
                if (updateStudent != null)
                {
                    LastNameTextBox.Text = updateStudent.LastName;
                    FirstNameTextBox.Text = updateStudent.FirstMidName;
                    EnrollmentDateTextBox.Text = updateStudent.EnrollmentDate.ToString("MM-dd-yyyy");
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // Redirect back to Student Page
            Response.Redirect("~/Students.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Use EF to connect to the server
            using(DefaultConnection db = new DefaultConnection())
            {
                // use the Student model to create a new student object
                // and save a new record
                Student newStudent = new Student();

                int StudentID = 0;
                if(Request.QueryString.Count > 0) // our URL has a StudentID in it
                {
                    // get the id from url
                    StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

                    //ge the current student from EF db
                    newStudent = (from student in db.Students
                                  where student.StudentID == StudentID
                                  select student).FirstOrDefault();
                }

                // add form data to the new student record
                newStudent.LastName = LastNameTextBox.Text;
                newStudent.FirstMidName = FirstNameTextBox.Text;
                newStudent.EnrollmentDate = Convert.ToDateTime(EnrollmentDateTextBox.Text);

                // use LINQ to ADO.Net to add / insert new student into the database

                //check to see if new student is being added
                if (StudentID == 0)
                {
                    db.Students.Add(newStudent);
                }

                //save our changes - run an update
                db.SaveChanges();

                //redirect back to updated student page
                Response.Redirect("~/Students.aspx");
            }
        }
    }
}