File image upload into a folder
-----------------------------------
This exercise is to show how to enable the capability for users to  
upload an image (file) from their local drive, and store it into a web server folder (website root/image folder). 

The key concepts are 

    * IFormFile interface in the Model (EmployeeProfileModel) and 
    * use a <input type="file">.

Important ideas:

* Create an entity that has the image file defintion as string (ProfileImageFileName).
  It will hold teh filename with no path.

      public string ProfileImageFileName { get; set; }

* Create a model that has both, a string to hold the full filename and 
  a IFormFile to retreve a new selected file at the HTML page, which will be initally null

        public string ProfileImageFilename { get; set; }
        public IFormFile ProfileImage { get; set; }

* OnGet() :The operation will use the domain entity Filename (no path) and
  will complete the full path to the file location in teh server. 
  This will be added into the the image source in the HTML page.
  The IFormFile will be null.

         var imgFullFilename = Path.Combine("uploads", domainEmpProf.ProfileImageFileName);

         empProfileModel.ProfileImageFilename = imgFullFilename
         empProfileModel.ProfileImage = null

* The razor controller, the OnPost must be ASYNC

* Post(): The operation will receive the IFormFile property in the Model and this will 
  contain the file loaded in memory. Using the IFormFile object, copy the file from 
  memory to the targe location in the server. Also this operation changes the filename in the 
  Entity and Model (with no path)

         var file = Path.Combine(_environment.WebRootPath, "images", "uploads", empProfileModel.ProfileImage.FileName);

        // Create a file in the target location at the server, and save the file in memory
        using (var fileStream = new FileStream(file, FileMode.Create))
        {
          await empProfileModel.ProfileImage.CopyToAsync(fileStream);
        }

        empProfileModel.ProfileImageFilename = empProfileModel.ProfileImage.FileName;

        domainEmpProf.ProfileImageFileName = empProfileModel.ProfileImage.FileName;


* JavaScript to select the image: The HTML page uses JS to handle the user 
  selection for the image, using an <Input type="file"> tag. Also an IMG tag
  taht will be used to render teh image stored and visualize the new selected
  image. 

  The code uses a FileLIts, File, and FileReader classes and two event listeners.
  This code can be expanded to add a drag-n-drop area.

  This exrcise offer a good example how Razor notations and JS can be used together
  to improve functionality.


References
----------------
Selecting images: https://web.dev/read-files/


