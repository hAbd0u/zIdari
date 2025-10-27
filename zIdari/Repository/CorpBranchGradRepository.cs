using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using zIdari.Model;

// Repository/CorpBranchGradRepository.cs
namespace zIdari.Repository
{
    public sealed class CorpBranchGradRepository : ICorpBranchGradRepository
    {
        private readonly string _connString;

        public CorpBranchGradRepository(string dbPath)
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

        private static object Db(object value) => value ?? DBNull.Value;

        // Get records by type for grid display
        public List<CorpBranchGradGridRow> GetByType(string type)
        {
            var rows = new List<CorpBranchGradGridRow>();
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    csg_id AS CsgId,
                    law_num AS LawNumCol,
                    title AS TitleCol
                FROM corp_bran_grad
                WHERE type = @type
                ORDER BY title;";

            cmd.Parameters.AddWithValue("@type", type);

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                rows.Add(new CorpBranchGradGridRow
                {
                    CsgId = Convert.ToInt32(r["CsgId"]),
                    LawNumCol = r["LawNumCol"]?.ToString(),
                    TitleCol = r["TitleCol"]?.ToString()
                });
            }
            return rows;
        }

        // Get single record by ID
        public CorpBranchGrad GetById(int csgId)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT * FROM corp_bran_grad 
                WHERE csg_id = @id;";
            cmd.Parameters.AddWithValue("@id", csgId);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new CorpBranchGrad
            {
                CsgId = Convert.ToInt32(r["csg_id"]),
                Type = r["type"]?.ToString(),
                LawNum = r["law_num"]?.ToString(),
                Title = r["title"]?.ToString()
            };
        }

        // Add new record
        public void Add(CorpBranchGrad cbg)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO corp_bran_grad (type, law_num, title) 
                VALUES (@Type, @LawNum, @Title);";

            cmd.Parameters.AddWithValue("@Type", Db(cbg.Type));
            cmd.Parameters.AddWithValue("@LawNum", Db(cbg.LawNum));
            cmd.Parameters.AddWithValue("@Title", Db(cbg.Title));

            cmd.ExecuteNonQuery();
        }

        // Update existing record
        public void Update(CorpBranchGrad cbg)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE corp_bran_grad SET
                    type = @Type,
                    law_num = @LawNum,
                    title = @Title
                WHERE csg_id = @Id;";

            cmd.Parameters.AddWithValue("@Id", cbg.CsgId);
            cmd.Parameters.AddWithValue("@Type", Db(cbg.Type));
            cmd.Parameters.AddWithValue("@LawNum", Db(cbg.LawNum));
            cmd.Parameters.AddWithValue("@Title", Db(cbg.Title));

            cmd.ExecuteNonQuery();
        }

        // Delete record
        public void Delete(int csgId)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM corp_bran_grad WHERE csg_id = @id;";
            cmd.Parameters.AddWithValue("@id", csgId);
            cmd.ExecuteNonQuery();
        }
    }
}