using System;

namespace zIdari.Model
{
    public class Carrier
    {
        public int CarrierId { get; set; }
        public int FolderNum { get; set; }
        public int FolderNumYear { get; set; }
        public string CarrierType { get; set; }
        public string CarrierName { get; set; }
        public string Corp { get; set; }
        public string Branche { get; set; }
        public string Position { get; set; }
        public string Class { get; set; }
        public string Degree { get; set; }
        public string Status { get; set; }
        public string DocType { get; set; }
        public string DocName { get; set; }
        public string DocNum { get; set; }
        public DateTime? DocDateSign { get; set; }
        public DateTime? DocDateEffective { get; set; }
        public string PubFuncNum { get; set; }
        public DateTime? PubFuncDate { get; set; }
        public string FinCtrlNum { get; set; }
        public DateTime? FinCtrlDate { get; set; }
    }
}

