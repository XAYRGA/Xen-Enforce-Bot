using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XENFCoreSharp.DoubleDecker;
using MySql.Data;
using MySql;
using MySql.Data.MySqlClient;
using System.Threading;


namespace XENFCoreSharp.Bot
{
    public static partial class XenforceRoot
    {
        public static XenforceGroupConfiguration CurrentGroupConfig;
        public static string lastError;
        public static Thread backgroundTasks; 
        private static long lazyBackgroundTaskTime = 0;

        public static bool AddCleanupMessage(long GroupID, long messageid, long lifetime)
        {
            var query = string.Format("INSERT INTO xenf_spoken (`group`,`messageid`,`lifetime`,`whencreated`) VALUES ({0},{1},{2},{3})",
                GroupID,
                messageid,
                lifetime,
                Helpers.getUnixTime()
                );
            var ra = 0;
            SQL.NonQuery(query, out ra);
            if (ra < 1) { return false; }
            return true;
        }

        public static void DoMessageCleanup()
        {
            var query = "SELECT * FROM xenf_spoken";
            SQLQueryInstance QueryInst;
            MySqlDataReader data;

            Stack<long> MessageCleanupIndicies = new Stack<long>(32);

            var ok1 = SQL.Query(query, out QueryInst);
           // Console.WriteLine(ok1);
            if (!ok1 || QueryInst==null)
            {
                Console.WriteLine("Failed to clean up messages {0}", SQL.getLastError());
                return;
            }
            data = QueryInst.reader;
            var MessageIndex = 0;
            while (data.Read())
            {
               

                var idx = (int)data["index"];
                var grp = (long)data["group"];
                var mid = (long)data["messageid"];
                var lifetime = (int)data["lifetime"];
                var whencreated = (long)data["whencreated"];

                if ((lifetime + whencreated) < Helpers.getUnixTime())
                {
                    var chat = new TGChat();
                    chat.id = grp;
                    Telegram.deleteMessage(chat, mid);
                    MessageCleanupIndicies.Push(idx);
                }

             
                MessageIndex++; 
                if (MessageIndex > 30)
                {
                    break;
                }
            }
            QueryInst.Finish();

            while (MessageCleanupIndicies.Count > 0) // Had to do this,  no concurrent queries. 
            {
                Console.WriteLine("CLEANUP?");
                var mid = MessageCleanupIndicies.Pop();
                int ra = 0;
                var ok = SQL.NonQuery("DELETE FROM xenf_spoken WHERE `index`=" + mid, out ra);
                if (!ok)
                {
                    Console.WriteLine("Message cannot be cleaned up {0}", SQL.getLastError());
                }
            }
        }

        public static void Start()
        {
            long last_update = 0;
            XenforceCommandInterpreter.LoadCommands();
            backgroundTasks = new Thread(new ThreadStart(backgroundTasksThread));
            backgroundTasks.Start();
            lazyBackgroundTaskTime = Helpers.getUnixTime();
            while (true)
            {
                if (lazyBackgroundTaskTime < Helpers.getUnixTime() - 3)
                {
                   // backgroundTasksThread();
                    lazyBackgroundTaskTime = Helpers.getUnixTime();
                }

                var b = Telegram.getUpdates(last_update + 1, 30);
                if (b==null)
                {
                    Console.WriteLine("NULL Update Array:", Telegram.lastError);
                    Thread.Sleep(250);
                    continue;
                }

                Console.WriteLine("Grabbed {0} updates.", b.Length);
                for (int i=0; i < b.Length; i++)
                {

                    var CurrentUpdate = b[i];
                    if (CurrentUpdate.update_id > last_update)
                    {
                        last_update = CurrentUpdate.update_id;
                    }
                    if (CurrentUpdate.edited_message!=null)
                    {
                        CurrentUpdate.message = CurrentUpdate.edited_message;
                    }

                    if (CurrentUpdate.message != null)
                    {
                        var message = CurrentUpdate.message;

                        if (message.from.is_bot)
                        {
                            continue; // don't need to process these
                        }

                        CurrentGroupConfig = getGroupConfiguration(message.chat);


                        if (message.photo!=null)
                        {
                            try
                            {
                                XENFCoreSharp.Bot.Filters.XESFilter.doURLMediaFilter(message, message.from);
                            }
                            catch (Exception E)
                            {
                                var file = Helpers.writeStack(E.ToString());
                                //message.replySendMessage("Hello -- Something terrible went wrong with XenfBot, please report this, and reference XES_STK_" + file);
                                Console.WriteLine(E.ToString());
                            }

                        }

                        if (message.text!=null)
                        {

                            try
                            {
                                XENFCoreSharp.Bot.Filters.XESFilter.doURLFilter(message, message.from);
                            } catch (Exception E)
                            {
                                var file = Helpers.writeStack(E.ToString());
                                // message.replySendMessage("Hello -- Something terrible went wrong with XenfBot, please report this, and reference XES_STK_" + file);
                                Console.WriteLine(E.ToString());
                            }


                            var varg_1 = message.text.Split((char)0x20);
                            if (varg_1.Length > 0)
                            {
                                if (varg_1[0]=="/xen")
                                {
                                    if (varg_1.Length > 1)
                                    {
                                        var VARGUMENTS = new string[varg_1.Length - 2];
                                        for (int ix = 2; ix < varg_1.Length; ix++)
                                        {
                                            VARGUMENTS[ix - 2] = varg_1[ix]; // Move only string argument into argument table. 
                                        }
                                        var success = XenforceCommandInterpreter.ExecuteCommand(varg_1[1], message, VARGUMENTS);
                                        if (!success)
                                        {
                                            message.replySendMessage(XenforceCommandInterpreter.TRESULT);
                                        }
                                        continue;
                                    }
                                    var successx = XenforceCommandInterpreter.ExecuteCommand("help", message, new string[0]);
                                    if (!successx)
                                    {
                                        //message.replySendMessage(XenforceCommandInterpreter.TRESULT);
                                    }
                                    continue;
                                }
                            }
                        }

                        if (message.new_chat_members!=null && message.new_chat_members.Length > 0)
                        {
                            for (int bq=0; bq < message.new_chat_members.Length;bq++)
                            {
                               XenforceUserFilter.doFilterUser(message,message.new_chat_members[bq]);
                            }
                        }

                    }            
                }            
            }            
        }


        public static void backgroundTasksThread()
        {
            while (true)
            {
                try
                {
                    DoMessageCleanup();
                    Filters.XESFilter.captcha_CheckExpired();
                } catch (Exception E)
                {
                    Console.WriteLine(E);
                }
                Thread.Sleep(2000);
            }
        }


    }
}
