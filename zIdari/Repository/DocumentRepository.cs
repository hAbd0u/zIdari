using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using zIdari.Model;

namespace zIdari.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly string _connString;

        public DocumentRepository(string dbPath)
        {
            _connString = $"Data Source={dbPath};Version=3;";
        }

        public List<Document> GetAll(string search = null)
        {
            var list = new List<Document>();
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = string.IsNullOrWhiteSpace(search)
                    ? "SELECT doc_id, type, title FROM document ORDER BY doc_id DESC"
                    : "SELECT doc_id, type, title FROM document WHERE title LIKE @q OR type LIKE @q ORDER BY doc_id DESC";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    cmd.Parameters.AddWithValue("@q", "%" + search + "%");
                }
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(new Document
                        {
                            DocumentId = rd.GetInt32(0),
                            Type = rd.IsDBNull(1) ? null : rd.GetString(1),
                            Title = rd.IsDBNull(2) ? null : rd.GetString(2)
                        });
                    }
                }
            }
            return list;
        }

        public Document GetById(int id)
        {
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "SELECT doc_id, type, title FROM document WHERE doc_id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return new Document
                        {
                            DocumentId = rd.GetInt32(0),
                            Type = rd.IsDBNull(1) ? null : rd.GetString(1),
                            Title = rd.IsDBNull(2) ? null : rd.GetString(2)
                        };
                    }
                }
            }
            return null;
        }

        public int Insert(Document doc)
        {
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "INSERT INTO document (type, title) VALUES (@tp,@t); SELECT last_insert_rowid();";
                cmd.Parameters.AddWithValue("@tp", (object)doc.Type ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@t", (object)doc.Title ?? DBNull.Value);
                var id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }

        public void Update(Document doc)
        {
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "UPDATE document SET type=@tp, title=@t WHERE doc_id=@id";
                cmd.Parameters.AddWithValue("@tp", (object)doc.Type ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@t", (object)doc.Title ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@id", doc.DocumentId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "DELETE FROM document WHERE doc_id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}


