File image upload into the DB
-----------------------------------
This ASP.NET Core exercise is to show how to enable teh capability for users to 
upload an image (file) from their loca drive, and store it into a SQL datbase.

The key concepts

    * IFormFile interface in the Model (EmployeeProfileModel) and 
    * use a <input type="file"> in teh HTML page.
    * Use byte[] in the entity that will store into the DB

Importnat ideas: 

 * Create an entity that has the image file defintion as string (ProfileImageFileName).
   It will hold the filename with no path, and a field to hold the image data
   as a binarry array

      public string ProfileImageFileName { get; set; }
      public byte[] ProfileImageData { get; set; }

* Create a model that has both, a string to hold the full filename and 
  a IFormFile to retreve a new selected file at the HTML page, which will be initally null

     public string ProfileImageFilename { get; set; }
     public IFormFile ProfileImage { get; set; }

* OnGet() :The operation will use the domain Entity Filename (no path) and the byte array
  to render the image in the HTML page. The Byte array will converter using base64 and 
  set as URL, hich will be assigned to the image source in the HTML page.
  
  This will be added into the property that represents the image in the HTML page.
  The IFormFile will be null.

     if (domainEmpProf.ProfileImageData != null)
     {
        imageBase64Data = Convert.ToBase64String(domainEmpProf.ProfileImageData);
        imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
     }

     empProfileModel.ProfileImageFilename = imageDataURL
     empProfileModel.ProfileImage = null

* The razor controller, the OnPost must be ASYNC

* Post(): The operation will receive the IFormFile property in the Model and this will 
  contain the file loaded in memory. Using the IFormFile object, copy the file from 
  memory and into a Memory String, which is converted into an array and store in the
  Entity byte array field. Also this operation changes the filename in the 
  Entity and Model (with no path)

     using(MemoryStream ms = new MemoryStream())
     {
        await empProfileModel.ProfileImage.CopyToAsync(ms);
        domainEmpProf.ProfileImageData = ms.ToArray();
     }

     empProfileModel.ProfileImageFilename = empProfileModel.ProfileImage.FileName;

     domainEmpProf.ProfileImageFileName = empProfileModel.ProfileImage.FileName;

* Note: This exercise prepare the data inside teh Entity, used inside an In-Memory 
  repository to simplify and explain teh mechanics. The developer can add EF and
  define SQL repository that can store teh entity in the DB. 

  Code first in EF will initalize the proper variable for teh byte array. However, it
  is important to convey the reader that a convenient field type definition for a 
  byte array will be 
  
    * varbinary(maxlength). 
    * Some authors also recomend nvarchar(maxlegth).

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
Storing images in DB: http://www.binaryintellect.net/articles/2f55345c-1fcb-4262-89f4-c4319f95c5bd.aspx