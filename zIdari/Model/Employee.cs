using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zIdari.Model
{
    public class Employee
    {
        public int FolderNum { get; set; }
        public int FolderNumYear { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string FnameFr { get; set; }
        public string LnameFr { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime Birth { get; set; }
        public string Wilaya { get; set; }
        public bool Sex { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Relation { get; set; }
        public string HusbandName { get; set; }
        public DateTime ActDate { get; set; }
        public int ActNum { get; set; }
    }

}
