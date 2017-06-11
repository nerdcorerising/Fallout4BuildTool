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
        private List<PriorityPerkRequest> _queue = new List<PriorityPerkRequest>();
        private int _currentLevel = 2;
        private int _initalSpecialPoints = 21;
        private List<PerkInstruction> _buildOrder = new List<PerkInstruction>();
        private Dictionary<Special, int> _initialBuild = new Dictionary<Special, int>();
        private Dictionary<Special, int> _investedPoints = new Dictionary<Special, int>();
        private bool _good;

        public SkillPriorityQueue(string path)
        {
            string data = null;
            using (StreamReader reader = new StreamReader(new FileStream(s_dataPath, FileMode.Open)))
            {
                data = reader.ReadToEnd();
            }
            
            _data = JsonConvert.DeserializeObject<PerksData>(data);
            
            _good = BuildQueueFromFile(path);

            foreach (Special special in EnumHelper.GetEnumValues<Special>())
            {
                _initialBuild[special] = 1;
                _investedPoints[special] = 0;
            }
        }

        public Build GetBuild()
        {
            if (!_good)
            {
                return null;
            }
            
            while(_queue.Count > 0)
            {
                GetNextPerk(out PriorityPerkRequest next, out int pos);
                EnsureSpecialForPerk(next.Perk, next.PerkLevel);

                PerkInstruction pi = new PerkInstruction() { Perk = next.Perk, PerkLevel = next.PerkLevel };
                _buildOrder.Add(pi);
                _queue.RemoveAt(pos);

                _currentLevel++;
            }
            
            return new Build() { InitialBuild = _initialBuild, BuildOrder = _buildOrder};
        }

        private void GetNextPerk(out PriorityPerkRequest next, out int pos)
        {
            PriorityPerkRequest candidate = null;
            int candidatePriority = Int32.MaxValue;
            int candidatePos = -1;

            for (int i = 0; i < _queue.Count; ++i)
            {
                PriorityPerkRequest ppr = _queue[i];
                if (ppr.Priority < candidatePriority && _currentLevel >= ppr.RequiredCharacterLevel)
                {
                    candidate = ppr;
                    candidatePriority = ppr.Priority;
                    candidatePos = i;
                }
            }

            next = candidate;
            pos = candidatePos;
        }

        private void EnsureSpecialForPerk(string perkName, int perkLevel)
        {
            Perk perk = GetPerk(perkName);
            Special requiredSpecial = perk.specialPreReq.type;
            int requiredLevel = perk.specialPreReq.level;

            int currentLevel = _initialBuild[requiredSpecial] + _investedPoints[requiredSpecial];
            if (currentLevel >= requiredLevel)
            {
                return;
            }

            int diff = requiredLevel - currentLevel;
            while (diff > 0)
            {
                if(_initalSpecialPoints > 0)
                {
                    _initalSpecialPoints--;
                    _initialBuild[requiredSpecial]++;
                }
                else
                {
                    _investedPoints[requiredSpecial]++;
                    PerkInstruction pi = new PerkInstruction() { Perk = requiredSpecial.ToString(), PerkLevel = _currentLevel + 1 };
                }

                diff--;
            }
        }

        private bool BuildQueueFromFile(string path)
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
                        Console.Error.WriteLine($"Invalid format for line #{lineNum}.");
                        return false;
                    }

                    if (!Int32.TryParse(parts[0], out int pri))
                    {
                        Console.Error.WriteLine($"Invalid priority ranking {parts[0]} for line #{lineNum}, must be a valid number.");
                        return false;
                    }

                    IEnumerable<string> perks = parts[1].Split(',').Select(x => x.Trim());
                    foreach (string perk in perks)
                    {
                        if(!AddPerkToQueue(perk, pri, lineNum))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private bool AddPerkToQueue(string perk, int pri, int lineNum)
        {
            string[] parts = perk.Split('-');
            if(parts.Length < 1 || parts.Length > 2)
            {
                Console.Error.WriteLine($"Invalid specification {perk} on line #{lineNum}");
                return false;
            }

            string perkName = parts[0].Trim();
            IEnumerable<Perk> matches = _data.perks.Where(x => x.name == perkName);
            if(matches.Count() <= 0)
            {
                string closestName = GetClosestPerkName(perkName);
                Console.Error.WriteLine($"Perk {perkName} does not exist on line #{lineNum}. Did you mean {closestName}?");
                return false;
            }

            Perk match = matches.First();

            int maxLevel = GetMaxLevelForPerk(perkName);
            int specifiedLevel;
            if (parts.Length > 1)
            {
                if (!Int32.TryParse(parts[1], out specifiedLevel))
                {
                    Console.Error.WriteLine($"Invalid max rank {parts[1]} on line #{lineNum}, is not a number.");
                    return false;
                }

                if (specifiedLevel > maxLevel)
                {
                    Console.Error.WriteLine($"Level {specifiedLevel} is greater than max level {maxLevel} for perk {perkName}");
                    return false;
                }
            }
            else
            {
                specifiedLevel = maxLevel;
            }

            for (int i = 1; i <= specifiedLevel; ++i)
            {
                Rank currentRank = match.ranks.Where(x => x.rank == i).First();
                int requiredLevel = currentRank.levelPreReq;
                PriorityPerkRequest pi = new PriorityPerkRequest()
                {
                    Perk = perkName,
                    PerkLevel = i,
                    Priority = pri,
                    RequiredCharacterLevel = requiredLevel
                };
                _queue.Add(pi);
            }

            return true;
        }

        private string GetClosestPerkName(string perkName)
        {
            string candidate = null;
            int closest = Int32.MinValue;

            foreach(Perk perk in _data.perks)
            {
                string currentName = perk.name.ToLower();
                int numInCommon = GetNumberOfCharactersInCommon(currentName, perkName.ToLower());

                if (numInCommon > closest)
                {
                    closest = numInCommon;
                    candidate = currentName;
                }
            }

            return candidate;
        }

        private int GetNumberOfCharactersInCommon(string lhs, string rhs)
        {
            // Very naive algorithm, just look at how close each letter is alphabetically
            int sum = 0;
            int maxLength = Math.Max(lhs.Length, rhs.Length);

            int[] lhsChars = new int[26];
            foreach(char ch in lhs)
            {
                int pos = (int)ch - 'a';
                if (pos <= 0 || pos > 25)
                {
                    continue;
                }

                lhsChars[pos] += 1;
            }

            int[] rhsChars = new int[26];
            foreach (char ch in rhs)
            {
                int pos = (int)ch - 'a';
                if (pos <= 0 || pos > 25)
                {
                    continue;
                }

                rhsChars[pos] += 1;
            }

            for (int i = 0; i < 26; ++i)
            {
                sum += Math.Min(lhsChars[i], rhsChars[i]);
            }

            return sum;
        }

        private int GetMaxLevelForPerk(string perkName)
        {
            IEnumerable<Rank> allRanks = GetPerk(perkName).ranks;
            return allRanks.Max(rank => rank.rank);
        }

        private Perk GetPerk(string perkName)
        {
            return _data.perks.Where(perk => perk.name.ToLower() == perkName.ToLower()).First();
        }
    }
}
