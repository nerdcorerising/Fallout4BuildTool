using System.Collections.Generic;

namespace FalloutBuild
{
    public class Build
    {
        public Dictionary<Special, int> InitialBuild;
        public IEnumerable<PerkInfo> BuildOrder;
    }
}