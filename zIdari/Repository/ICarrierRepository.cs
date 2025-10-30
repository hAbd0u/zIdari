using System.Collections.Generic;
using zIdari.Model;

namespace zIdari.Repository
{
    public interface ICarrierRepository
    {
        List<Carrier> GetByEmployee(int folderNum, int folderNumYear);
        Carrier GetById(int carrierId);
        int Insert(Carrier carrier);
        void Update(Carrier carrier);
        void Delete(int carrierId);
    }
}

