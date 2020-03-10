using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 


public class CXayrgaConfRom
{
    Dictionary<string, string> configuration;
    
    public CXayrgaConfRom(string filename)
    {
        configuration = new Dictionary<string, string>();
        string[] splitarc = new string[] { "=" };
        try
        {
            string[] rom_contents = File.ReadAllLines(filename);
          
            //Console.WriteLine("Reading {0}", filename);

            for (int i=0;i<rom_contents.Length;i++)
            {
                var gx = rom_contents[i].Split(splitarc,2,StringSplitOptions.None);
              
                if (gx.Length == 2)
                {
                    configuration[gx[0]] = gx[1];
                   // Console.WriteLine(gx[0] + " " + gx[1]);
                }
            }
        } catch(Exception E)
        {
            Console.WriteLine("Error reading configuration {0}",E.ToString());
        }
    }

    public string getValue(string val)
    {
        string retl;
        configuration.TryGetValue(val, out retl);

        return retl;
    }

}

