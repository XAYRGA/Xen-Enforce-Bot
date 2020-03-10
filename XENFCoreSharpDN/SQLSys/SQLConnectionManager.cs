using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace XENFCoreSharp.SQLSys
{
    class SQLConnectionManager
    {
        private Stack<MySqlConnection> muConnections;
        private SQLConnectionInformation sConnectConfiguration;

        public SQLConnectionManager(SQLConnectionInformation connectInfo, int initalConnections)
        {
            sConnectConfiguration = connectInfo;
            muConnections = new Stack<MySqlConnection>();            
            for (int i=0; i <initalConnections; i++)
            {
                addNewConnection();
            }
        }

        private void addNewConnection()
        {
            Console.WriteLine("SQLSys: Adding new connection.");

            string conStr = string.Format("server={0};user={1};database={2};port=3306;password={3}", sConnectConfiguration.host, sConnectConfiguration.username, sConnectConfiguration.database, sConnectConfiguration.password);
            var sqlConnection = new MySqlConnection(conStr);
            sqlConnection.Open(); // Open connection
            lock (muConnections)
            {
                muConnections.Push(sqlConnection);
            }
        }

        public MySqlConnection getConnection()
        {
            lock (muConnections)
            {
                while (muConnections.Count > 0) // go until we're empty or get a valid connection.
                {
                    var checkConnection = muConnections.Pop();
                    if (checkConnection==null) { continue; } // this will eat dead connections. this is already eaten by GC
                    if ((checkConnection.State & (ConnectionState.Broken | ConnectionState.Closed)) > 0 )
                    {
                        checkConnection.Dispose(); //  These are broken or disconnected but not eaten by GC, feed them to GC
                        continue; // Skip to next iteration
                    }
                    return checkConnection; // Its out of the stack and it's ready, we can just drop out of the function here.
                } // If we fall through the while loop, there were no connections available
                addNewConnection(); // Add the new connection. Automatically adds it to the end of the stack.
                return muConnections.Pop(); // Pull it from the stack, should be a valid connection.
            }
        }
        public bool returnConnection(MySqlConnection connection)
        {
            if (connection==null) { return false; } // It's null, we don't want anything to do with it.
            if ((connection.State & (ConnectionState.Broken | ConnectionState.Closed)) > 0)
            {
                // Connection is broken, we don't want anything to do with it.
                return false; 
            }
            // it meets the chriteria for re-adding
            lock (muConnections)
            {
                muConnections.Push(connection);
            }
            return true;
        }
    }
}
