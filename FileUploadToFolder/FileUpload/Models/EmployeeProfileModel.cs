using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FileUpload.Models
{
    public class EmployeeProfileModel
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public string ProfileImageFilename { get; set; }

        [Required(ErrorMessage = "Please choose profile image")]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfileImage { get; set; }
    }
}
