using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using zIdari.Model;

// Repository/ExperienceRepository.cs
namespace zIdari.Repository
{
    public sealed class ExperienceRepository : IExperienceRepository
    {
        private readonly string _connString;

        public ExperienceRepository(string dbPath)
        {
            if (!File.Exists(dbPath))
                throw new FileNotFoundException("Database file not found.", dbPath);

            var csb = new SQLiteConnectionStringBuilder
            {
                DataSource = dbPath,
                Version = 3,
                FailIfMissing = true
            };
            _connString = csb.ToString();
        }

        // Helper methods
        private static object Db(object value) => value ?? DBNull.Value;

        private static DateTime? ReadDate(object obj)
        {
            if (obj == DBNull.Value || obj == null) return null;
            if (DateTime.TryParse(obj.ToString(), CultureInfo.InvariantCulture,
                                  DateTimeStyles.AssumeLocal, out var dt))
                return dt;
            return null;
        }

        private static string DateOut(DateTime? dt)
            => dt?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        // Get experiences for grid display
        public List<ExperienceGridRow> GetExperiencesForGrid(int folderNum, int folderNumYear)
        {
            var rows = new List<ExperienceGridRow>();
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    experience_id AS ExperienceId,
                    cert_ref AS CertNumCol,
                    company AS CompanyCol,
                    date_from AS DateFromCol,
                    date_to AS DateToCol,
                    position AS PositionCol
                FROM experience
                WHERE FolderNum = @fn AND FolderNumYear = @fy
                ORDER BY date_from DESC;";

            cmd.Parameters.AddWithValue("@fn", folderNum);
            cmd.Parameters.AddWithValue("@fy", folderNumYear);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                rows.Add(new ExperienceGridRow
                {
                    ExperienceId = Convert.ToInt32(r["ExperienceId"]),
                    CertNumCol = r["CertNumCol"]?.ToString(),
                    CompanyCol = r["CompanyCol"]?.ToString(),
                    DateFromCol = r["DateFromCol"]?.ToString(),
                    DateToCol = r["DateToCol"]?.ToString(),
                    PositionCol = r["PositionCol"]?.ToString()
                });
            }
            return rows;
        }

        // Get single experience by ID
        public Experience GetById(int experienceId)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT * FROM experience 
                WHERE experience_id = @id;";
            cmd.Parameters.AddWithValue("@id", experienceId);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Experience
            {
                ExperienceId = Convert.ToInt32(r["experience_id"]),
                FolderNum = Convert.ToInt32(r["FolderNum"]),
                FolderNumYear = Convert.ToInt32(r["FolderNumYear"]),
                CertRef = r["cert_ref"]?.ToString(),
                Company = r["company"]?.ToString(),
                Position = r["position"]?.ToString(),
                DateFrom = ReadDate(r["date_from"]),
                DateTo = ReadDate(r["date_to"])
            };
        }

        // Add new experience
        public void Add(Experience exp)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO experience (
                    FolderNum, FolderNumYear, cert_ref, company, position, date_from, date_to
                ) VALUES (
                    @FolderNum, @FolderNumYear, @CertRef, @Company, @Position, @DateFrom, @DateTo
                );";

            cmd.Parameters.AddWithValue("@FolderNum", exp.FolderNum);
            cmd.Parameters.AddWithValue("@FolderNumYear", exp.FolderNumYear);
            cmd.Parameters.AddWithValue("@CertRef", Db(exp.CertRef));
            cmd.Parameters.AddWithValue("@Company", Db(exp.Company));
            cmd.Parameters.AddWithValue("@Position", Db(exp.Position));
            cmd.Parameters.AddWithValue("@DateFrom", Db(DateOut(exp.DateFrom)));
            cmd.Parameters.AddWithValue("@DateTo", Db(DateOut(exp.DateTo)));

            cmd.ExecuteNonQuery();
        }

        // Update existing experience
        public void Update(Experience exp)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE experience SET
                    cert_ref = @CertRef,
                    company = @Company,
                    position = @Position,
                    date_from = @DateFrom,
                    date_to = @DateTo
                WHERE experience_id = @Id;";

            cmd.Parameters.AddWithValue("@Id", exp.ExperienceId);
            cmd.Parameters.AddWithValue("@CertRef", Db(exp.CertRef));
            cmd.Parameters.AddWithValue("@Company", Db(exp.Company));
            cmd.Parameters.AddWithValue("@Position", Db(exp.Position));
            cmd.Parameters.AddWithValue("@DateFrom", Db(DateOut(exp.DateFrom)));
            cmd.Parameters.AddWithValue("@DateTo", Db(DateOut(exp.DateTo)));

            cmd.ExecuteNonQuery();
        }

        // Delete experience
        public void Delete(int experienceId)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM experience WHERE experience_id = @id;";
            cmd.Parameters.AddWithValue("@id", experienceId);
            cmd.ExecuteNonQuery();
        }
    }
}