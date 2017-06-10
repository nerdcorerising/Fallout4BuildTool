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
            string dataPath = "BuildInfo.json";
            SkillPriorityQueue queue = new SkillPriorityQueue(dataPath);
        }
    }
}