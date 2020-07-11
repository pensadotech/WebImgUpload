using FileUpload.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileUpload.Repositories
{
    public interface IEmployeeRepository
    {
        EmployeeProfile GetById(int empId);
        EmployeeProfile Update(EmployeeProfile updatedEmployeeProfile);

    }
}
