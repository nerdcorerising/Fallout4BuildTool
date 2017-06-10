using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FalloutBuild
{
    public class SkillPriorityQueue
    {
        private PerksData _data;

        public SkillPriorityQueue(string path)
        {
            string data = null;
            using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                data = reader.ReadToEnd();
            }

            _data = JsonConvert.DeserializeObject<PerksData>(data);
        }
    }
}
