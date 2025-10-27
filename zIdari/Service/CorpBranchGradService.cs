using System.Collections.Generic;
using System.Linq;
using zIdari.Model;
using zIdari.Repository;

// Service/CorpBranchGradService.cs
namespace zIdari.Service
{
    public sealed class CorpBranchGradService
    {
        private readonly ICorpBranchGradRepository _repo;

        public CorpBranchGradService(ICorpBranchGradRepository repo) => _repo = repo;

        // Validations
        public List<string> Validate(CorpBranchGrad cbg)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(cbg.Type))
                errors.Add("النوع مطلوب.");

            if (!new[] { "corp", "branche", "fonction" }.Contains(cbg.Type))
                errors.Add("النوع يجب أن يكون: corp أو branche أو fonction.");

            if (string.IsNullOrWhiteSpace(cbg.Title))
                errors.Add("العنوان مطلوب.");

            return errors;
        }

        public (bool ok, List<string> errors) AddOrUpdate(CorpBranchGrad cbg)
        {
            var errors = Validate(cbg);
            if (errors.Any()) return (false, errors);

            if (cbg.CsgId.HasValue && cbg.CsgId > 0)
                _repo.Update(cbg);
            else
                _repo.Add(cbg);

            return (true, new List<string>());
        }

        public void Delete(int csgId) => _repo.Delete(csgId);

        public List<CorpBranchGradGridRow> GetByType(string type) => _repo.GetByType(type);

        public CorpBranchGrad GetById(int csgId) => _repo.GetById(csgId);
    }
}