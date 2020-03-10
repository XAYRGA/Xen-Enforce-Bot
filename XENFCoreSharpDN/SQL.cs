using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql;
using MySql.Data.MySqlClient;
using XENFCoreSharp.SQLSys;

namespace XENFCoreSharp
{
    public class SQLQueryInstance
    {
        public MySqlConnection connection;
        public MySqlDataReader reader;
        public bool Finish()
        {
            if (reader!=null && !reader.IsClosed)
            {
                reader.Close();
            }
            if (connection != null)
            {
                SQL.returnConnection(connection);
            }
            return true;
        }
    }

    public static class SQL
    {
        static string lastError;
        static SQLConnectionManager connectionManager;

        public static bool Init(string host, string user, string password, string db)
        {
            var connectInfo = new SQLConnectionInformation
            {
                host = host,
                username = user,
                password = password,
                database = db,
            };

            connectionManager = new SQLConnectionManager(connectInfo, 10);
            return true;        
        }

        public static void returnConnection(MySqlConnection conn)
        {
            connectionManager.returnConnection(conn);
        }

        public static string escape(string esc)
        {
            return MySqlHelper.EscapeString(esc);
        }

        public static bool Query(string query, out SQLQueryInstance qLQueryInstance)
        {
            var qi = new SQLQueryInstance();
            var conn = connectionManager.getConnection();
            qi.connection = conn;

            var wtf = new MySqlCommand(query, conn);
            try
            {
                var wtf2 = wtf.ExecuteReader();
                qi.reader = wtf2;
                qLQueryInstance = qi;
              

                return true; 
            } catch (MySqlException E) {
                lock (lastError)
                {
                    lastError = E.Message;
                    connectionManager.returnConnection(conn);
                    qLQueryInstance = null; 
                    return false;
                }
            }
        }

        public static bool NonQuery(string data, out int rowsAffected)
        {
            rowsAffected = 0;
            var conn = connectionManager.getConnection(); 
            MySqlCommand comm = new MySqlCommand(data, conn);
            try
            {
                rowsAffected = comm.ExecuteNonQuery();
                connectionManager.returnConnection(conn);
                return true;
            } catch (MySqlException E)
            {
                lock (lastError)
                {
                    lastError = E.Message;
                }
                connectionManager.returnConnection(conn);
                return false;
            }
        }

        public static string getLastError()
        {
            return lastError;
        }
    }
}
