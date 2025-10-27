// Repository/CorpBranchGradGridRow.cs
namespace zIdari.Repository
{
    public sealed class CorpBranchGradGridRow
    {
        // Hidden key for Edit/Delete
        public int CsgId { get; set; }

        // Columns bound to the grid
        public string LawNumCol { get; set; }
        public string TitleCol { get; set; }
    }
}