using System;
using System.Collections.Generic;
using zIdari.Model;
using zIdari.Repository;

namespace zIdari.Service
{
    public class CarrierService
    {
        private readonly ICarrierRepository _repo;

        public CarrierService(ICarrierRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public List<Carrier> GetByEmployee(int folderNum, int folderNumYear)
        {
            return _repo.GetByEmployee(folderNum, folderNumYear);
        }

        public Carrier GetById(int carrierId)
        {
            return _repo.GetById(carrierId);
        }

        public (bool ok, List<string> errors, int? id) Add(Carrier carrier)
        {
            var errors = Validate(carrier, isNew: true);
            if (errors.Count > 0) return (false, errors, null);

            var id = _repo.Insert(carrier);
            return (true, new List<string>(), id);
        }

        public (bool ok, List<string> errors) Update(Carrier carrier)
        {
            var errors = Validate(carrier, isNew: false);
            if (errors.Count > 0) return (false, errors);

            _repo.Update(carrier);
            return (true, new List<string>());
        }

        public void Delete(int carrierId)
        {
            _repo.Delete(carrierId);
        }

        private List<string> Validate(Carrier carrier, bool isNew)
        {
            var errors = new List<string>();

            if (carrier.FolderNum <= 0)
                errors.Add("رقم الملف مطلوب");

            if (carrier.FolderNumYear <= 0)
                errors.Add("سنة الملف مطلوبة");

            if (string.IsNullOrWhiteSpace(carrier.CarrierType))
                errors.Add("نوع المسار مطلوب");

            return errors;
        }
    }
}

