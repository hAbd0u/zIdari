// Model/CorpBranchGrad.cs
namespace zIdari.Model
{
    public sealed class CorpBranchGrad
    {
        public int? CsgId { get; set; }  // nullable for new records
        public string Type { get; set; }  // "corp", "branche", or "fonction"
        public string LawNum { get; set; }
        public string Title { get; set; }
    }
}