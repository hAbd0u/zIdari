using System.Collections.Generic;
using zIdari.Model;

// Repository/ICorpBranchGradRepository.cs
namespace zIdari.Repository
{
    public interface ICorpBranchGradRepository
    {
        // Grid/listing by type
        List<CorpBranchGradGridRow> GetByType(string type);

        // CRUD
        CorpBranchGrad GetById(int csgId);
        void Add(CorpBranchGrad cbg);
        void Update(CorpBranchGrad cbg);
        void Delete(int csgId);
    }
}