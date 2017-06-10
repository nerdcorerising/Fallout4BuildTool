using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace FalloutBuild
{
    public class SkillPriorityQueue
    {
        private static readonly string s_dataPath = "BuildInfo.json";
        private PerksData _data;

        public SkillPriorityQueue(string path)
        {
            string data = null;
            using (StreamReader reader = new StreamReader(new FileStream(s_dataPath, FileMode.Open)))
            {
                data = reader.ReadToEnd();
            }
            
            _data = JsonConvert.DeserializeObject<PerksData>(data);

            BuildQueueFromFile(path);
        }

        public Build GetBuild()
        {
            throw new NotImplementedException();
        }

        private void BuildQueueFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                string line;
                int lineNum = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    lineNum++;
                    if (String.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    line = line.Trim();
                    if (line.StartsWith("#"))
                    {
                        continue;
                    }

                    string[] parts = line.Split(':');
                    if (parts.Length != 2)
                    {
                        Console.Error.WriteLine($"Invalid format for line #{lineNum}, skipping.");
                        continue;
                    }

                    if (!Int32.TryParse(parts[0], out int pri))
                    {
                        Console.Error.WriteLine($"Invalid priority ranking {parts[0]} for line #{lineNum}, must be a valid number. Skipping.");
                        continue;
                    }

                    IEnumerable<string> perks = parts[1].Split(',').Select(x => x.Trim());
                    foreach (string perk in perks)
                    {
                        AddPerkToQueue(perk, pri);
                    }
                }
            }
        }

        private void AddPerkToQueue(string perk, int pri)
        {
            // TODO: figure out priority queue implementation
            // Parse each perk and add it to the queue
            // Do validation for format of perk in file
            // Do validation for perk having max rank <= what's available
            // Do validation that perk exists 
            // Do spell check suggestions if perk doesn't exist?
            throw new NotImplementedException();
        }
    }
}
