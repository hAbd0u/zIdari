using System.Collections.Generic;
using zIdari.Model;

namespace zIdari.Repository
{
    public interface IDocumentRepository
    {
        List<Document> GetAll(string search = null);
        Document GetById(int id);
        int Insert(Document doc);
        void Update(Document doc);
        void Delete(int id);
        List<string> GetDistinctTypes();
        List<string> GetTitlesByType(string type);
    }
}


