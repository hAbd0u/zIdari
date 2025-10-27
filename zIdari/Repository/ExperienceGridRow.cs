// Repository/ExperienceGridRow.cs
namespace zIdari.Repository
{
    public sealed class ExperienceGridRow
    {
        // Hidden key for Edit/Delete
        public int ExperienceId { get; set; }

        // Columns bound to the grid
        public string CertNumCol { get; set; }
        public string CompanyCol { get; set; }
        public string DateFromCol { get; set; }
        public string DateToCol { get; set; }
        public string PositionCol { get; set; }
    }
}