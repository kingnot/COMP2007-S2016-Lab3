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

                // add form data to the new student record
                newStudent.LastName = LastNameTextBox.Text;
                newStudent.FirstMidName = FirstNameTextBox.Text;
                newStudent.EnrollmentDate = Convert.ToDateTime(EnrollmentDateTextBox.Text);

                // use LINQ to ADO.Net to add / insert new student into the database
                db.Students.Add(newStudent);

                //save our changes
                db.SaveChanges();

                //redirect back to updated student page
                Response.Redirect("~/Students.aspx");
            }
        }
    }
}