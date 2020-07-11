using FileUpload.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        // Private members .............................
        private List<EmployeeProfile> _employeeProfiles;

        // Constructors ...........................
        public EmployeeRepository()
        {
            _employeeProfiles = new List<EmployeeProfile>();

            // Sample data 
            var emplProfile = new EmployeeProfile
            {
                Id = 1,
                EmployeeName = "John Doe",
                ProfileImageFileName = "XmasTree1.jpg"
            };

            _employeeProfiles.Add(emplProfile);

        }

        // Methods ..............................
        public EmployeeProfile GetById(int empId)
        {
            var empProf = _employeeProfiles.SingleOrDefault(e => e.Id == empId);

            return empProf;
        }

        public EmployeeProfile Update(EmployeeProfile updatedEmployeeProfile)
        {
            var empProf = _employeeProfiles.SingleOrDefault(e => e.Id == updatedEmployeeProfile.Id);

            if (empProf != null)
            {
                empProf.EmployeeName = updatedEmployeeProfile.EmployeeName;
                empProf.ProfileImageFileName = updatedEmployeeProfile.ProfileImageFileName;
            }

            return empProf;
        }
    }
}
