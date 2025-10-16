using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Repository/EmployeeGridRow.cs
namespace zIdari.Repository
{
    public sealed class EmployeeGridRow
    {
        // keys (useful for Edit/Delete)
        public int FolderNum { get; set; }
        public int FolderNumYear { get; set; }

        // columns bound to the grid
        public string NumFolderCol { get; set; }
        public string FullNameArCol { get; set; }
        public string FullNameFrCol { get; set; }
        public string PhoneCol { get; set; }
        public string EmailCol { get; set; }
        public string AddressCol { get; set; }
    }
}

