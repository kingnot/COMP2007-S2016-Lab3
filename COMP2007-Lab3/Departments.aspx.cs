using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

// using statements that are required to coneect to EF DB
using COMP2007_Lab3.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace COMP2007_Lab3
{
    public partial class Departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading the page for the first time, populate the student grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "DepartmentID";
                Session["SortDirection"] = "ASC";
                //get the student data
                this.GetDepartments();
            }
        }

        /**
         * <summary>
         * This method gets department data from the DB
         * </summary>
         * 
         * @method GetDepartment
         * @return {void}
         */
        protected void GetDepartments()
        {
            string sortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

            // connect to EF
            using (DefaultConnection db = new DefaultConnection())
            {
                // query the departments from table using EF and LINQ
                var departments = (from allDepartments in db.Departments
                                   select allDepartments);

                // bind the results to GridView
                DepartmentsGridView.DataSource = departments.AsQueryable().OrderBy(sortString).ToList();
                DepartmentsGridView.DataBind();
            }
        }

        protected void DepartmentsGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // get which row was clicked
            int rowSelected = e.RowIndex;

            // get the selected DepartmentID using Grid's data collection
            int DepartmentID = Convert.ToInt32(DepartmentsGridView.DataKeys[rowSelected].Values["DepartmentID"]);

            // using EF to find selected departmentID from DB and delete it
            using (DefaultConnection db = new DefaultConnection())
            {
                Department deletedDepartment = (from departmentRecords in db.Departments
                                                where departmentRecords.DepartmentID == DepartmentID
                                                select departmentRecords).FirstOrDefault();

                // remove the record from DB
                db.Departments.Remove(deletedDepartment);
                db.SaveChanges();
                // refresh the grid
                this.GetDepartments();
            }
        }

        protected void DepartmentsGridView_Sorting(object sender, GridViewSortEventArgs e)
        {
            // get the column to sort by
            Session["SortColumn"] = e.SortExpression;

            //toggle the direction
            Session["SortDirection"] = Session["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";

            //refresh the grid
            this.GetDepartments();
        }

        protected void DepartmentsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                //check to see if the click is on the header row
                if(e.Row.RowType == DataControlRowType.Header)
                {
                    LinkButton linkbutton = new LinkButton();

                    for(int index = 0; index < DepartmentsGridView.Columns.Count; index++)
                    {
                        if(DepartmentsGridView.Columns[index].SortExpression == Session["SortColumn"].ToString())
                        {
                            if(Session["SortDirection"].ToString() == "ASC")
                            {
                                linkbutton.Text = "<i class='fa fa-caret-up fa-lg'></i>";
                            }
                            else
                            {
                                linkbutton.Text = "<i class='fa fa-caret-down fa-lg'></i>";
                            }

                            e.Row.Cells[index].Controls.Add(linkbutton);
                        }
                    }
                }
            }
        }
    }
}