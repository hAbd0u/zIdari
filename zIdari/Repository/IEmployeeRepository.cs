using System.Collections.Generic;
using zIdari.Model;

namespace zIdari.Repository
{
    public interface IEmployeeRepository
    {
        // Grid/listing
        List<EmployeeGridRow> GetEmployeesForGrid(string search = null);

        // CRUD
        Employee GetByKey(int folderNum, int folderNumYear);
        void Add(Employee e);
        void Update(Employee e);
        void Delete(int folderNum, int folderNumYear);
        (int FolderNum, int FolderNumYear) GenerateNextKey(int? yearOverride = null);
    }
}
