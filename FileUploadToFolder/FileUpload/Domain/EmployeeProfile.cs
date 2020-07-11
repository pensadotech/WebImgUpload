using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Domain
{
    public class EmployeeProfile
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string ProfileImageFileName { get; set; }

        // For a SQL database, this field need to be defined in
        // the DB as varbinary(maxlength) or nvarchar(maxlegth)
        public byte[] ProfileImageData { get; set; }
    }
}
