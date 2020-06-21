
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql;
using System.Data;
using MySql.Data.MySqlClient;


namespace XenfbotDN
{

    public static class SQL
    {
        static string lastError = "0x0000000000000000 NO_ERROR";
        private static string host;
        private static string username;
        private static string password;
        private static string database;
        private static string cstring;

        public static bool Init(string host1, string user1, string password1, string db1)
        {
            host = host1;
            username = user1;
            password = password1;
            database = db1;
            cstring = string.Format("server={0};user={1};database={2};port=3306;password={3}", host, username, database, password);
            return true;

        }

        public static string escape(string esc)
        {
            return MySqlHelper.EscapeString(esc);
        }

        public static DataRowCollection Query(string query)
        {   try
            {
                using (DataTable retl = new DataTable())
                {
                    using (MySqlConnection nc = new MySqlConnection(cstring))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, nc))
                        {
                            
                            nc.Open();
                            using (var w = new MySqlDataAdapter(cmd))
                            {
                                w.Fill(retl);
                            }
                            return retl.Rows;
                        }
                    }
                }
            } catch (Exception e)
            {
                lastError = e.ToString();
                return null;
            }
        }

        public static async Task<DataRowCollection> QueryAsync(string query)
        {
            try
            {
                using (DataTable retl = new DataTable())
                {
                    using (MySqlConnection nc = new MySqlConnection(cstring))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(query, nc))
                        {
                            nc.Open();
                            using (var w = new MySqlDataAdapter(cmd))
                            {
                                await w.FillAsync(retl);
                            }
                            return retl.Rows;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                lastError = e.ToString();
                return null;
            }
        }

        public static bool NonQuery(string query, out int rowsAffected)
        {
            try
            {
                using (MySqlConnection nc = new MySqlConnection(cstring))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, nc))
                    {
                        nc.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            } catch (Exception E)
            {
                lastError = E.ToString();
                rowsAffected = 0;
                return false;
            }
        }

        public static async Task<bool> NonQueryAsync(string query)
        {
            try
            {
                using (MySqlConnection nc = new MySqlConnection(cstring))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, nc))
                    {
                        nc.Open();
                        await cmd.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception E)
            {
                lastError = E.ToString();
                return false;
            }
        }

        public static string getLastError()
        {
            return lastError;
        }
    }
}