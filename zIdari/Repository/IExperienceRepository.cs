using System.Collections.Generic;
using zIdari.Model;

// Repository/IExperienceRepository.cs
namespace zIdari.Repository
{
    public interface IExperienceRepository
    {
        // Grid/listing for specific employee
        List<ExperienceGridRow> GetExperiencesForGrid(int folderNum, int folderNumYear);

        // CRUD
        Experience GetById(int experienceId);
        void Add(Experience exp);
        void Update(Experience exp);
        void Delete(int experienceId);
    }
}