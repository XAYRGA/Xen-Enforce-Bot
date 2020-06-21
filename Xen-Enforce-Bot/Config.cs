using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace XenfbotDN
{
    public static class Config
    {
        private const string tag = "xenfbot@config";
        private static Dictionary<string, string> configuration;

        public static void init(string filename)
        {
                configuration = new Dictionary<string, string>(); // configuration is effectively a dictionary
                string[] splitarc = new string[] { "=" }; // these are the characters we're going to split at.
                if (!File.Exists(filename)) // Check to make sure the configuration file exists first
                {
                    Helpers.writeOut(tag, "Configuration file loading skipped. File doesn't exist. Assuming environment variables instead."); // if it doesn't , skip
                    return; // return
                }
                try // make sure we're safe
                {
                    string[] rom_contents = File.ReadAllLines(filename); // read the file, split at \r\n
                    for (int i = 0; i < rom_contents.Length; i++) // for each line
                    {
                        var gx = rom_contents[i].Split(splitarc, 2, StringSplitOptions.None); // split the line at the first = , max 1 split (2 results)
                        if (gx.Length == 2) // check to see if the length is == 2, a bit redundant but whatever
                        {
                            configuration[gx[0]] = gx[1]; // split name=value
                        }
                    }
                }
                catch (Exception E) // catch any errors
                {
                    Helpers.writeOut(tag, "Error reading configuration {0}", E.ToString()); // Throw error if something happens
                }
            }

        public static string getValue(string val) //  Read a configuration value
        {
            string retl;
            var ok = configuration.TryGetValue(val, out retl); // check to see if it is in the dictionary
            if (!ok) // if it's not
            {
                // stuff the environment variable into it, if there is one
                retl = Environment.GetEnvironmentVariable(val); // for Darl -- environment / docker support 
            }
            return retl;
        }

    }

}
