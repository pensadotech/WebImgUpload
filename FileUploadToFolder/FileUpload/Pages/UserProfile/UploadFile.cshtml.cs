using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileUpload.Domain;
using FileUpload.Models;
using FileUpload.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FileUpload.Pages.UserProfile
{
    public class UploadFileModel : PageModel
    {
        // Private memebers .........................
        private IWebHostEnvironment _environment;

        private IEmployeeRepository _empPrifileRepo;

        [BindProperty]
        public EmployeeProfileModel empProfileModel { get; set; }

        // Constructors ..........................
        public UploadFileModel(IWebHostEnvironment environment, IEmployeeRepository empProfRepo)
        {
            _environment = environment;
            _empPrifileRepo = empProfRepo;
        }

        // Methods ..................................
        public IActionResult OnGet()
        {
            // From Domain
            EmployeeProfile domainEmpProf = _empPrifileRepo.GetById(1);

            string imageBase64Data = null;
            string imageDataURL = "https://via.placeholder.com/200";
               
            if (domainEmpProf.ProfileImageData != null)
            {
                imageBase64Data = Convert.ToBase64String(domainEmpProf.ProfileImageData);
                imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }

            // Domain to Model - assign full file name to property for HTML page
            empProfileModel = new EmployeeProfileModel
            {
                Id = domainEmpProf.Id,
                EmployeeName = domainEmpProf.EmployeeName,
                ProfileImageFilename = imageDataURL,
                ProfileImage = null
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // does ProfileImage has contents
            if (empProfileModel.ProfileImage != null)
            {
                // Note: empProfile.ProfileImage is an IFormFile   

                // sync new filenane selected to the string representation in the model
                // using teh IFormFile.FileName property
                empProfileModel.ProfileImageFilename = empProfileModel.ProfileImage.FileName;

                // MAP model to domain
                // From Domain
                EmployeeProfile domainEmpProf = _empPrifileRepo.GetById(empProfileModel.Id);
                // Update the image filename in the Domain entity
                domainEmpProf.ProfileImageFileName = empProfileModel.ProfileImage.FileName;

                // Convert image into a memory string and into the Entity byte array
                using(MemoryStream ms = new MemoryStream())
                {
                    await empProfileModel.ProfileImage.CopyToAsync(ms);
                    domainEmpProf.ProfileImageData = ms.ToArray();
                }

                // SAVE - Update Domain Entity
                domainEmpProf = _empPrifileRepo.Update(domainEmpProf);

                // Saved, send it back to home
                return RedirectToPage("../Index");
            }

            // Refresh the page by redirecting it to itself
            return RedirectToPage("");
        }

    }
}