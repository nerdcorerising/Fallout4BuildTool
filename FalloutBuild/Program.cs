using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace FalloutBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || args[0].Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                PrintUsage();
                return;
            }

            string path = args[0];
            if(!File.Exists(path))
            {
                Console.Error.WriteLine($"Error file {path} not found.");
                PrintUsage();
                return;
            }

            SkillPriorityQueue queue = new SkillPriorityQueue(path);

            Build build = queue.GetBuild();
            if(build == null)
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(new FileStream("Build.txt", FileMode.Create)))
            {
                writer.WriteLine("Initial build:");

                foreach (Special special in EnumHelper.GetEnumValues<Special>())
                {
                    writer.WriteLine($"    {special}: {build.InitialBuild[special]}");
                }

                writer.WriteLine("Perk investment per level:");

                IEnumerable<PerkInstruction> infos = build.BuildOrder;
                for (int i = 0; i < infos.Count(); ++i)
                {
                    PerkInstruction info = infos.ElementAt(i);
                    writer.WriteLine($"    Level {i + 2}: {info.Perk} level {info.PerkLevel}");
                }
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: dotnet FalloutBuild.dll <path to build file.txt>");
            Console.WriteLine("Creates the build in Build.txt. Will overwrite Build.Txt on every run.");
            Console.WriteLine("See Sample.txt for how the build file should be formatted.");
        }
    }
}