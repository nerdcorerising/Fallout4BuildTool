using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FalloutBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "output.txt";
            using (StreamWriter writer = new StreamWriter(new FileStream(path, FileMode.Create)))
            {
                IEnumerable<string> specials = new string[] { "strength", "perception", "endurance", "charisma", "intelligence", "agility", "luck" };
                IEnumerable<int> levels = Enumerable.Range(1, 10);

                writer.WriteLine("{");
                writer.WriteLine("  \"special\": [ \"strength\", \"perception\", \"endurance\", \"charisma\", \"intelligence\", \"agility\", \"luck\" ],");
                writer.WriteLine("  \"perks\": [");

                foreach (string special in specials)
                {
                    foreach (int level in levels)
                    {
                        writer.WriteLine("    {");
                        writer.WriteLine("      \"NIY\": {");
                        writer.WriteLine($"        \"specialPreReq\": {{ \"{special}\": {level} }},");
                        writer.WriteLine("         \"ranks\": [");
                        writer.WriteLine("        {");
                        writer.WriteLine("          \"rank\": 1,");
                        writer.WriteLine("          \"levelPreReq\":  ");
                        writer.WriteLine("        },");
                        writer.WriteLine("        {");
                        writer.WriteLine("          \"rank\": 2,");
                        writer.WriteLine("          \"levelPreReq\":  ");
                        writer.WriteLine("        },");
                        writer.WriteLine("        {");
                        writer.WriteLine("          \"rank\": 3,");
                        writer.WriteLine("          \"levelPreReq\":  ");
                        writer.WriteLine("        },");
                        writer.WriteLine("        {");
                        writer.WriteLine("          \"rank\": 4,");
                        writer.WriteLine("          \"levelPreReq\":  ");
                        writer.WriteLine("        },");
                        writer.WriteLine("        {");
                        writer.WriteLine("          \"rank\": 5,");
                        writer.WriteLine("          \"levelPreReq\":  ");
                        writer.WriteLine("        },");
                        writer.WriteLine("        ]");
                        writer.WriteLine("      }");
                        writer.WriteLine("    },");
                    }
                }

                writer.WriteLine("  ]");
                writer.WriteLine("}");
            }
        }
    }
}