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
    public partial class DepartmentDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                this.GetDepartment();
            }
        }
        
        /**
         * <summary>
         * This method gets the editted department info from DB
         * </summary>
         * 
         * @method GetDepartment
         * @return void
         */
        protected void GetDepartment()
        {
            //populate the form with existing data from DB
            int DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

            //connect to EF DB
            using (DefaultConnection db = new DefaultConnection())
            {
                //populate a department instance from DB with the department ID from url string
                Department updatedDepartment = (from department in db.Departments
                                                where department.DepartmentID == DepartmentID
                                                select department).FirstOrDefault();

                //map the department info to the form control
                if (updatedDepartment != null)
                {
                    DepartmentNameTextBox.Text = updatedDepartment.Name;
                    BudgetTextBox.Text = Convert.ToString(updatedDepartment.Budget);
                }
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // redirect back to department page
            Response.Redirect("~/Departments.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // connect to EF DB
            using (DefaultConnection db = new DefaultConnection())
            {
                //create a new Deparment instance and save it
                Department newDepartment = new Department();

                int DepartmentID = 0;
                if (Request.QueryString.Count > 0)
                {
                    //get the department id from url
                    DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                    // get the department from DB
                    newDepartment = (from department in db.Departments
                                     where department.DepartmentID == DepartmentID
                                     select department).FirstOrDefault();
                }

                // add form data to new department record
                newDepartment.Name = DepartmentNameTextBox.Text;
                newDepartment.Budget = Convert.ToInt32(BudgetTextBox.Text);

                // add new department into the DB
                if (DepartmentID == 0)
                {
                    db.Departments.Add(newDepartment);
                }

                //save new instance/update info
                db.SaveChanges();

                //redirect back to updated department page
                Response.Redirect("~/Departments.aspx");
            }
        }
    }
}