using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using zIdari.Model;
using zIdari.Repository;

namespace zIdari.Service
{
    public sealed class EmployeeService
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeService(IEmployeeRepository repo) => _repo = repo;

        // Basic validations (extend as needed)
        public List<string> Validate(Employee e)
        {
            var errors = new List<string>();

            if (e.FolderNum <= 0) errors.Add("FolderNum must be > 0.");
            if (e.FolderNumYear <= 0) errors.Add("FolderNumYear must be > 0.");
            if (string.IsNullOrWhiteSpace(e.Fname) && string.IsNullOrWhiteSpace(e.Lname))
                errors.Add("Arabic first/last name is required.");
            if (!string.IsNullOrWhiteSpace(e.Email) &&
                !Regex.IsMatch(e.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Email format is invalid.");

            return errors;
        }

        public (bool ok, List<string> errors) AddOrUpdate(Employee e)
        {
            var errors = Validate(e);
            if (errors.Any()) return (false, errors);

            var existing = _repo.GetByKey(e.FolderNum, e.FolderNumYear);
            if (existing == null) _repo.Add(e);
            else _repo.Update(e);

            return (true, new List<string>());
        }

        public void Delete(int folderNum, int folderNumYear) => _repo.Delete(folderNum, folderNumYear);

        public List<EmployeeGridRow> GetGrid(string search = null)
            => _repo.GetEmployeesForGrid(search);

        public (int FolderNum, int FolderNumYear) GenerateNewFolderKey(int? year = null)
            => _repo.GenerateNextKey(year);

    }
}
