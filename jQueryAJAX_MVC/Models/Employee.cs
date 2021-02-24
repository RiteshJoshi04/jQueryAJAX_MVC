using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace jQueryAJAX_MVC.Models
{
    [Table("tblEmployee")]
    public class Employee
    {
        public Employee()
        {
            ImagePath = "~/AppFiles/Images/default.jpg";
        }
        public int EmployeeId { get; set; }
        [Required(ErrorMessage ="This Field is required.")]
        public string Name { get; set; }
        public string Position { get; set; }
        public string Office { get; set; }
        public int? Salary { get; set; }
        [DisplayName("Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }

    }
}