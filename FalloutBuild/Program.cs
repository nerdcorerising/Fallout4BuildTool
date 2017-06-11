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
            string path = "Sample.txt";
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
    }
}