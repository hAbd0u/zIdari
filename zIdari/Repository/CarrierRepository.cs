using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using zIdari.Model;

namespace zIdari.Repository
{
    public class CarrierRepository : ICarrierRepository
    {
        private readonly string _connString;

        public CarrierRepository(string dbPath)
        {
            if (!File.Exists(dbPath))
                throw new FileNotFoundException("Database file not found.", dbPath);

            _connString = $"Data Source={dbPath};Version=3;";
        }

        private static object Db(object value) => value ?? DBNull.Value;

        private static string SafeGetString(SQLiteDataReader rd, int index)
        {
            if (rd.IsDBNull(index)) return null;
            
            try
            {
                // Try to get as string first
                return rd.GetString(index);
            }
            catch
            {
                // If that fails, get the raw value and convert to string
                try
                {
                    var val = rd.GetValue(index);
                    return val?.ToString();
                }
                catch
                {
                    return null; // Last resort
                }
            }
        }

        public List<Carrier> GetByEmployee(int folderNum, int folderNumYear)
        {
            var list = new List<Carrier>();
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT carrier_id, folderNum, folderNumYear, carrier_type, carrier_name,
                           corp, branche, position, class, degree, status,
                           doc_type, doc_name, doc_num, doc_date_sign, doc_date_effective,
                           pub_func_num, pub_func_date, fin_ctrl_num, fin_ctrl_date
                    FROM carrier
                    WHERE folderNum = @fn AND folderNumYear = @fny
                    ORDER BY carrier_id DESC";
                cmd.Parameters.AddWithValue("@fn", folderNum);
                cmd.Parameters.AddWithValue("@fny", folderNumYear);

                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        list.Add(ReadCarrier(rd));
                    }
                }
            }
            return list;
        }

        public Carrier GetById(int carrierId)
        {
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    SELECT carrier_id, folderNum, folderNumYear, carrier_type, carrier_name,
                           corp, branche, position, class, degree, status,
                           doc_type, doc_name, doc_num, doc_date_sign, doc_date_effective,
                           pub_func_num, pub_func_date, fin_ctrl_num, fin_ctrl_date
                    FROM carrier
                    WHERE carrier_id = @id";
                cmd.Parameters.AddWithValue("@id", carrierId);

                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        return ReadCarrier(rd);
                    }
                }
            }
            return null;
        }

        public int Insert(Carrier carrier)
        {
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    INSERT INTO carrier (
                        folderNum, folderNumYear, carrier_type, carrier_name,
                        corp, branche, position, class, degree, status,
                        doc_type, doc_name, doc_num, doc_date_sign, doc_date_effective,
                        pub_func_num, pub_func_date, fin_ctrl_num, fin_ctrl_date
                    ) VALUES (
                        @fn, @fny, @ct, @cn, @corp, @br, @pos, @cls, @deg, @st,
                        @dt, @dn, @dnum, @dds, @dde, @pfn, @pfd, @fcn, @fcd
                    );
                    SELECT last_insert_rowid();";

                AddParameters(cmd, carrier);
                var id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }

        public void Update(Carrier carrier)
        {
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    UPDATE carrier SET
                        folderNum = @fn,
                        folderNumYear = @fny,
                        carrier_type = @ct,
                        carrier_name = @cn,
                        corp = @corp,
                        branche = @br,
                        position = @pos,
                        class = @cls,
                        degree = @deg,
                        status = @st,
                        doc_type = @dt,
                        doc_name = @dn,
                        doc_num = @dnum,
                        doc_date_sign = @dds,
                        doc_date_effective = @dde,
                        pub_func_num = @pfn,
                        pub_func_date = @pfd,
                        fin_ctrl_num = @fcn,
                        fin_ctrl_date = @fcd
                    WHERE carrier_id = @id";

                AddParameters(cmd, carrier);
                cmd.Parameters.AddWithValue("@id", carrier.CarrierId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int carrierId)
        {
            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = "DELETE FROM carrier WHERE carrier_id = @id";
                cmd.Parameters.AddWithValue("@id", carrierId);
                cmd.ExecuteNonQuery();
            }
        }

        private void AddParameters(SQLiteCommand cmd, Carrier c)
        {
            cmd.Parameters.AddWithValue("@fn", c.FolderNum);
            cmd.Parameters.AddWithValue("@fny", c.FolderNumYear);
            cmd.Parameters.AddWithValue("@ct", Db(c.CarrierType));
            cmd.Parameters.AddWithValue("@cn", Db(c.CarrierName));
            cmd.Parameters.AddWithValue("@corp", Db(c.Corp));
            cmd.Parameters.AddWithValue("@br", Db(c.Branche));
            cmd.Parameters.AddWithValue("@pos", Db(c.Position));
            cmd.Parameters.AddWithValue("@cls", Db(c.Class));
            cmd.Parameters.AddWithValue("@deg", Db(c.Degree));
            cmd.Parameters.AddWithValue("@st", Db(c.Status));
            cmd.Parameters.AddWithValue("@dt", Db(c.DocType));
            cmd.Parameters.AddWithValue("@dn", Db(c.DocName));
            cmd.Parameters.AddWithValue("@dnum", Db(c.DocNum));
            cmd.Parameters.AddWithValue("@dds", Db(c.DocDateSign));
            cmd.Parameters.AddWithValue("@dde", Db(c.DocDateEffective));
            cmd.Parameters.AddWithValue("@pfn", Db(c.PubFuncNum));
            cmd.Parameters.AddWithValue("@pfd", Db(c.PubFuncDate));
            cmd.Parameters.AddWithValue("@fcn", Db(c.FinCtrlNum));
            cmd.Parameters.AddWithValue("@fcd", Db(c.FinCtrlDate));
        }

        private Carrier ReadCarrier(SQLiteDataReader rd)
        {
            return new Carrier
            {
                CarrierId = rd.GetInt32(0),
                FolderNum = rd.GetInt32(1),
                FolderNumYear = rd.GetInt32(2),
                CarrierType = SafeGetString(rd, 3),
                CarrierName = SafeGetString(rd, 4),
                Corp = SafeGetString(rd, 5),
                Branche = SafeGetString(rd, 6),
                Position = SafeGetString(rd, 7),
                Class = SafeGetString(rd, 8),
                Degree = SafeGetString(rd, 9),
                Status = SafeGetString(rd, 10),
                DocType = SafeGetString(rd, 11),
                DocName = SafeGetString(rd, 12),
                DocNum = SafeGetString(rd, 13),
                DocDateSign = rd.IsDBNull(14) ? (DateTime?)null : rd.GetDateTime(14),
                DocDateEffective = rd.IsDBNull(15) ? (DateTime?)null : rd.GetDateTime(15),
                PubFuncNum = SafeGetString(rd, 16),
                PubFuncDate = rd.IsDBNull(17) ? (DateTime?)null : rd.GetDateTime(17),
                FinCtrlNum = SafeGetString(rd, 18),
                FinCtrlDate = rd.IsDBNull(19) ? (DateTime?)null : rd.GetDateTime(19)
            };
        }
    }
}

