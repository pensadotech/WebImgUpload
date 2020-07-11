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

        // Methods .........................
        public IActionResult OnGet()
        {
            // From Domain
            EmployeeProfile domainEmpProf = _empPrifileRepo.GetById(1);

            // Convert domian file name inot a full file name at server
            var imgFullFilename = Path.Combine("uploads", domainEmpProf.ProfileImageFileName);

            // Domain to Model - assign full file name to property for HTML page
            empProfileModel = new EmployeeProfileModel
            {
                Id = domainEmpProf.Id,
                EmployeeName = domainEmpProf.EmployeeName,
                ProfileImageFilename = imgFullFilename,
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

                // add the folder path for storing the image with the loaded filename(no path)
                // contained in teh IFormFile object. In this case the path is "root/images" folder
                var file = Path.Combine(_environment.WebRootPath, "images", "uploads", empProfileModel.ProfileImage.FileName);

                // IDEA: an option for thhe file name is to convert teh name into something more practical 
                //       for example teh userID name, this way any new file will override the same file in
                //       the server, simplifying teh amount of files stored in teh server disk

                // SAVE-FILE: Create a file in the target location at the server, and save teh file in memory
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await empProfileModel.ProfileImage.CopyToAsync(fileStream);
                }

                // MAP Model to domain 
                // Get record from Domain
                EmployeeProfile domainEmpProf = _empPrifileRepo.GetById(empProfileModel.Id);
                // Update the image filename in the Domain entity
                domainEmpProf.ProfileImageFileName = empProfileModel.ProfileImage.FileName;
                // SAVE - Update Domain Entity
                domainEmpProf = _empPrifileRepo.Update(domainEmpProf);

                // Saved, send it back to home
                return RedirectToPage("../Index");
            }

            // Refresh the page by redirecting it to itself
            // Let the user hit CANCEL to return to home
            return RedirectToPage("");
        }

    }
}