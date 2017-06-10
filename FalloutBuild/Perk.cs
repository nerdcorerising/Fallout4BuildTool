using System;
using System.Collections.Generic;
using System.Text;

namespace FalloutBuild
{
    public class Perk
    {
        public string name;
        public PreReq specialPreReq;
        public List<Rank> ranks;
    }
}
