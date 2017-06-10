using System;
using System.Collections.Generic;
using System.Linq;

namespace FalloutBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IEnumerable<string> specials = new string[]{"strength", "perception", "endurance", "charisma", "intelligence", "agility", "luck"};
            IEnumerable<int> levels = Enumerable.Range(1, 10);

            Console.WriteLine("{");
            Console.WriteLine("  \"special\": [ \"strength\", \"perception\", \"endurance\", \"charisma\", \"intelligence\", \"agility\", \"luck\" ],");
            Console.WriteLine("  \"perks\": [");

            foreach (string special in specials)
            {
                foreach (int level in levels)
                {
                    Console.WriteLine("    {");
                    Console.WriteLine("      \"NIY\": [");
                    Console.WriteLine("        {");
                    Console.WriteLine($"          \"specialPreReq\": {{ \"{special}\": {level} }}");
                    Console.WriteLine("        },");
                    Console.WriteLine("        {");
                    Console.WriteLine("          \"rank\": 1,");
                    Console.WriteLine("          \"preReq\": { \"level\":  }");
                    Console.WriteLine("        },");
                    Console.WriteLine("        {");
                    Console.WriteLine("          \"rank\": 2,");
                    Console.WriteLine("          \"preReq\": { \"level\":  }");
                    Console.WriteLine("        },");
                    Console.WriteLine("        {");
                    Console.WriteLine("          \"rank\": 3,");
                    Console.WriteLine("          \"preReq\": { \"level\":  }");
                    Console.WriteLine("        },");
                    Console.WriteLine("        {");
                    Console.WriteLine("          \"rank\": 4,");
                    Console.WriteLine("          \"preReq\": { \"level\":  }");
                    Console.WriteLine("        },");
                    Console.WriteLine("        {");
                    Console.WriteLine("          \"rank\": 5,");
                    Console.WriteLine("          \"preReq\": { \"level\":  }");
                    Console.WriteLine("        },");
                    Console.WriteLine("      ]");
                    Console.WriteLine("    },");
                }
            }

            Console.WriteLine("  ]");
            Console.WriteLine("}");
        }
    }
}