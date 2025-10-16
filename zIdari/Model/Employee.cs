using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Model/Employee.cs
namespace zIdari.Model
{
    public sealed class Employee
    {
        public int FolderNum { get; set; }
        public int FolderNumYear { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string FnameFr { get; set; }
        public string LnameFr { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }

        public System.DateTime? Birth { get; set; }   // <-- nullable
        public string Wilaya { get; set; }
        public bool Sex { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Relation { get; set; }
        public string HusbandName { get; set; }

        public System.DateTime? ActDate { get; set; } // <-- nullable
        public int? ActNum { get; set; }              // <-- nullable
    }
}
