using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using zIdari.Model;

namespace zIdari.Repository
{
    public sealed class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connString;

        public EmployeeRepository(string dbPath)
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

            EnsureIndexes();
        }

        private void EnsureIndexes()
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();
            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"CREATE INDEX IF NOT EXISTS idx_employee_folder 
                                    ON employee(FolderNum, FolderNumYear);";
                cmd.ExecuteNonQuery();
            }
        }

        // ---- Helpers ----
        private static object Db(object value) => value ?? DBNull.Value;

        private static DateTime? ReadDate(object obj)
        {
            if (obj == DBNull.Value || obj == null) return null;
            // Expecting ISO-8601 text in DB; adjust if you store unix time
            if (DateTime.TryParse(obj.ToString(), CultureInfo.InvariantCulture,
                                  DateTimeStyles.AssumeLocal, out var dt))
                return dt;
            return null;
        }

        private static string DateOut(DateTime? dt)
            => dt?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        public (int FolderNum, int FolderNumYear) GenerateNextKey(int? yearOverride = null)
        {
            int year = yearOverride ?? DateTime.Now.Year;

            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            // Get MAX(FolderNum) for the chosen year, then +1
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT COALESCE(MAX(FolderNum), 0) FROM employee WHERE FolderNumYear = @fy;";
            cmd.Parameters.AddWithValue("@fy", year);
            var obj = cmd.ExecuteScalar();

            int next = (obj == DBNull.Value ? 0 : Convert.ToInt32(obj)) + 1;

            // Safety loop: if somehow taken, bump until free (rare, but robust)
            using var exists = conn.CreateCommand();
            exists.CommandText = @"SELECT 1 FROM employee WHERE FolderNum = @fn AND FolderNumYear = @fy LIMIT 1;";
            exists.Parameters.AddWithValue("@fn", next);
            exists.Parameters.AddWithValue("@fy", year);

            var taken = exists.ExecuteScalar();
            while (taken != null && taken != DBNull.Value)
            {
                next++;
                exists.Parameters["@fn"].Value = next;
                taken = exists.ExecuteScalar();
            }

            return (next, year);
        }

        // ---- Listing for Grid ----
        public List<EmployeeGridRow> GetEmployeesForGrid(string search = null)
        {
            var rows = new List<EmployeeGridRow>();
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using (var pragma = new SQLiteCommand("PRAGMA foreign_keys = ON;", conn))
                pragma.ExecuteNonQuery();

            using var cmd = conn.CreateCommand();
            if (string.IsNullOrWhiteSpace(search))
            {
                cmd.CommandText = @"
                    SELECT 
                        FolderNum, FolderNumYear,
                        (FolderNumYear || '/' || FolderNum)                  AS NumFolderCol,
                        (COALESCE(Fname,'')   || ' ' || COALESCE(Lname,''))  AS FullNameArCol,
                        (COALESCE(FnameFr,'') || ' ' || COALESCE(LnameFr,''))AS FullNameFrCol,
                        Phone  AS PhoneCol,
                        Email  AS EmailCol,
                        Address AS AddressCol
                    FROM employee;";
            }
            else
            {
                cmd.CommandText = @"
                    SELECT 
                        FolderNum, FolderNumYear,
                        (FolderNumYear || '/' || FolderNum)                 AS NumFolderCol,
                        (COALESCE(Fname,'')   || ' ' || COALESCE(Lname,''))  AS FullNameArCol,
                        (COALESCE(FnameFr,'') || ' ' || COALESCE(LnameFr,''))AS FullNameFrCol,
                        Phone  AS PhoneCol,
                        Email  AS EmailCol,
                        Address AS AddressCol
                    FROM employee
                    WHERE
                        Fname LIKE @q OR Lname LIKE @q OR
                        FnameFr LIKE @q OR LnameFr LIKE @q OR
                        Phone LIKE @q OR Email LIKE @q OR Address LIKE @q;";
                cmd.Parameters.AddWithValue("@q", $"%{search}%");
            }

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                rows.Add(new EmployeeGridRow
                {
                    FolderNum = Convert.ToInt32(r["FolderNum"]),
                    FolderNumYear = Convert.ToInt32(r["FolderNumYear"]),
                    NumFolderCol = r["NumFolderCol"]?.ToString(),
                    FullNameArCol = r["FullNameArCol"]?.ToString(),
                    FullNameFrCol = r["FullNameFrCol"]?.ToString(),
                    PhoneCol = r["PhoneCol"]?.ToString(),
                    EmailCol = r["EmailCol"]?.ToString(),
                    AddressCol = r["AddressCol"]?.ToString()
                });
            }
            return rows;
        }

        // ---- CRUD ----
        public Employee GetByKey(int folderNum, int folderNumYear)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM employee 
                                WHERE FolderNum=@fn AND FolderNumYear=@fy;";
            cmd.Parameters.AddWithValue("@fn", folderNum);
            cmd.Parameters.AddWithValue("@fy", folderNumYear);

            using var r = cmd.ExecuteReader();
            if (!r.Read()) return null;

            return new Employee
            {
                FolderNum = Convert.ToInt32(r["FolderNum"]),
                FolderNumYear = Convert.ToInt32(r["FolderNumYear"]),
                Fname = r["Fname"]?.ToString(),
                Lname = r["Lname"]?.ToString(),
                FnameFr = r["FnameFr"]?.ToString(),
                LnameFr = r["LnameFr"]?.ToString(),
                FatherName = r["FatherName"]?.ToString(),
                MotherName = r["MotherName"]?.ToString(),
                Birth = (DateTime)ReadDate(r["Birth"]),
                Wilaya = r["Wilaya"]?.ToString(),
                Sex = Convert.ToInt32(r["Sex"]) != 0,
                Address = r["Address"]?.ToString(),
                Phone = r["Phone"]?.ToString(),
                Email = r["Email"]?.ToString(),
                Relation = r["Relation"]?.ToString(),
                HusbandName = r["HusbandName"]?.ToString(),
                ActDate = (DateTime)ReadDate(r["ActDate"]),
                ActNum = (int)(r["ActNum"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["ActNum"]))
            };
        }

        public void Add(Employee e)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO employee (
                    FolderNum, FolderNumYear, Fname, Lname, FnameFr, LnameFr,
                    FatherName, MotherName, Birth, Wilaya, Sex, Address,
                    Phone, Email, Relation, HusbandName, ActDate, ActNum
                ) VALUES (
                    @FolderNum, @FolderNumYear, @Fname, @Lname, @FnameFr, @LnameFr,
                    @FatherName, @MotherName, @Birth, @Wilaya, @Sex, @Address,
                    @Phone, @Email, @Relation, @HusbandName, @ActDate, @ActNum
                );";

            cmd.Parameters.AddWithValue("@FolderNum", e.FolderNum);
            cmd.Parameters.AddWithValue("@FolderNumYear", e.FolderNumYear);
            cmd.Parameters.AddWithValue("@Fname", Db(e.Fname));
            cmd.Parameters.AddWithValue("@Lname", Db(e.Lname));
            cmd.Parameters.AddWithValue("@FnameFr", Db(e.FnameFr));
            cmd.Parameters.AddWithValue("@LnameFr", Db(e.LnameFr));
            cmd.Parameters.AddWithValue("@FatherName", Db(e.FatherName));
            cmd.Parameters.AddWithValue("@MotherName", Db(e.MotherName));
            cmd.Parameters.AddWithValue("@Birth", Db(DateOut(e.Birth)));
            cmd.Parameters.AddWithValue("@Wilaya", Db(e.Wilaya));
            cmd.Parameters.AddWithValue("@Sex", e.Sex ? 1 : 0);
            cmd.Parameters.AddWithValue("@Address", Db(e.Address));
            cmd.Parameters.AddWithValue("@Phone", Db(e.Phone));
            cmd.Parameters.AddWithValue("@Email", Db(e.Email));
            cmd.Parameters.AddWithValue("@Relation", Db(e.Relation));
            cmd.Parameters.AddWithValue("@HusbandName", Db(e.HusbandName));
            cmd.Parameters.AddWithValue("@ActDate", Db(DateOut(e.ActDate)));
            cmd.Parameters.AddWithValue("@ActNum", Db(e.ActNum));

            cmd.ExecuteNonQuery();
        }

        public void Update(Employee e)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE employee SET
                    Fname=@Fname, Lname=@Lname, FnameFr=@FnameFr, LnameFr=@LnameFr,
                    FatherName=@FatherName, MotherName=@MotherName, Birth=@Birth,
                    Wilaya=@Wilaya, Sex=@Sex, Address=@Address, Phone=@Phone, Email=@Email,
                    Relation=@Relation, HusbandName=@HusbandName, ActDate=@ActDate, ActNum=@ActNum
                WHERE FolderNum=@FolderNum AND FolderNumYear=@FolderNumYear;";

            cmd.Parameters.AddWithValue("@FolderNum", e.FolderNum);
            cmd.Parameters.AddWithValue("@FolderNumYear", e.FolderNumYear);
            cmd.Parameters.AddWithValue("@Fname", Db(e.Fname));
            cmd.Parameters.AddWithValue("@Lname", Db(e.Lname));
            cmd.Parameters.AddWithValue("@FnameFr", Db(e.FnameFr));
            cmd.Parameters.AddWithValue("@LnameFr", Db(e.LnameFr));
            cmd.Parameters.AddWithValue("@FatherName", Db(e.FatherName));
            cmd.Parameters.AddWithValue("@MotherName", Db(e.MotherName));
            cmd.Parameters.AddWithValue("@Birth", Db(DateOut(e.Birth)));
            cmd.Parameters.AddWithValue("@Wilaya", Db(e.Wilaya));
            cmd.Parameters.AddWithValue("@Sex", e.Sex ? 1 : 0);
            cmd.Parameters.AddWithValue("@Address", Db(e.Address));
            cmd.Parameters.AddWithValue("@Phone", Db(e.Phone));
            cmd.Parameters.AddWithValue("@Email", Db(e.Email));
            cmd.Parameters.AddWithValue("@Relation", Db(e.Relation));
            cmd.Parameters.AddWithValue("@HusbandName", Db(e.HusbandName));
            cmd.Parameters.AddWithValue("@ActDate", Db(DateOut(e.ActDate)));
            cmd.Parameters.AddWithValue("@ActNum", Db(e.ActNum));

            cmd.ExecuteNonQuery();
        }

        public void Delete(int folderNum, int folderNumYear)
        {
            using var conn = new SQLiteConnection(_connString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM employee 
                                WHERE FolderNum=@fn AND FolderNumYear=@fy;";
            cmd.Parameters.AddWithValue("@fn", folderNum);
            cmd.Parameters.AddWithValue("@fy", folderNumYear);
            cmd.ExecuteNonQuery();
        }
    }
}