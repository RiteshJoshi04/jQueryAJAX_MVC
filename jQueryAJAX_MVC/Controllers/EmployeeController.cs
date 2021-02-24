using jQueryAJAX_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jQueryAJAX_MVC.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllEmployees());
        }

        IEnumerable<Employee> GetAllEmployees()
        {
            using (EmployeeDbContext db = new EmployeeDbContext())
            {
                return db.Employees.ToList<Employee>();
            }
        }

        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            Employee employee = new Employee();

            //if id != 0 => Update operation

            if (id != 0)
            {
                using (EmployeeDbContext db = new EmployeeDbContext())
                {
                    employee = db.Employees.Where(x => x.EmployeeId == id).FirstOrDefault<Employee>();
                }
            }
            return View(employee);
        }
        [HttpPost]
        public ActionResult AddOrEdit(Employee emp)
        {
            try
            {
                if (emp.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    string extension = Path.GetExtension(emp.ImageUpload.FileName);

                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    emp.ImagePath = "~/AppFiles/Images/" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (EmployeeDbContext db = new EmployeeDbContext())
                {
                    if(emp.EmployeeId == 0)
                    {
                        db.Employees.Add(emp);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(emp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    
                }
                return Json(new
                {
                    success = true,
                    html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployees()),
                    message = "Submitted Successfully"
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = e.Message
                },  JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                using(EmployeeDbContext db = new EmployeeDbContext())
                {
                    Employee employee = db.Employees.Where(x => x.EmployeeId == id).FirstOrDefault<Employee>();
                    db.Employees.Remove(employee);
                    db.SaveChanges();
                }
                return Json(new
                {
                    success = true,
                    html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployees()),
                    message = "Deleted Successfully"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = e.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}