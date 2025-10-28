using System;
using System.Collections.Generic;
using zIdari.Model;
using zIdari.Repository;

namespace zIdari.Service
{
    public class DocumentService
    {
        private readonly IDocumentRepository _repo;

        public DocumentService(IDocumentRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public List<Document> GetAll(string search = null)
        {
            return _repo.GetAll(search);
        }

        public Document GetById(int id)
        {
            return _repo.GetById(id);
        }

        public (bool ok, List<string> errors, int? id) Add(Document doc)
        {
            var errors = Validate(doc, isNew: true);
            if (errors.Count > 0) return (false, errors, null);
            var id = _repo.Insert(doc);
            return (true, new List<string>(), id);
        }

        public (bool ok, List<string> errors) Update(Document doc)
        {
            var errors = Validate(doc, isNew: false);
            if (errors.Count > 0) return (false, errors);
            _repo.Update(doc);
            return (true, new List<string>());
        }

        public void Delete(int id)
        {
            _repo.Delete(id);
        }

        private List<string> Validate(Document doc, bool isNew)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(doc?.Title)) errors.Add("العنوان مطلوب");
            return errors;
        }
    }
}


