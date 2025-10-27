using System.Collections.Generic;
using System.Linq;
using zIdari.Model;
using zIdari.Repository;

// Service/ExperienceService.cs
namespace zIdari.Service
{
    public sealed class ExperienceService
    {
        private readonly IExperienceRepository _repo;

        public ExperienceService(IExperienceRepository repo) => _repo = repo;

        // Basic validations
        public List<string> Validate(Experience exp)
        {
            var errors = new List<string>();

            if (exp.FolderNum <= 0) errors.Add("FolderNum must be > 0.");
            if (exp.FolderNumYear <= 0) errors.Add("FolderNumYear must be > 0.");
            if (string.IsNullOrWhiteSpace(exp.Company))
                errors.Add("Company/Institution name is required.");

            // Optional: validate date range
            if (exp.DateFrom.HasValue && exp.DateTo.HasValue && exp.DateFrom > exp.DateTo)
                errors.Add("Date From cannot be after Date To.");

            return errors;
        }

        public (bool ok, List<string> errors) AddOrUpdate(Experience exp)
        {
            var errors = Validate(exp);
            if (errors.Any()) return (false, errors);

            if (exp.ExperienceId.HasValue && exp.ExperienceId > 0)
                _repo.Update(exp);
            else
                _repo.Add(exp);

            return (true, new List<string>());
        }

        public void Delete(int experienceId) => _repo.Delete(experienceId);

        public List<ExperienceGridRow> GetGrid(int folderNum, int folderNumYear)
            => _repo.GetExperiencesForGrid(folderNum, folderNumYear);

        public Experience GetById(int experienceId) => _repo.GetById(experienceId);
    }
}