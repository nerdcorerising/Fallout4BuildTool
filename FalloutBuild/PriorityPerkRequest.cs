namespace FalloutBuild
{
    public class PriorityPerkRequest
    {
        public string Perk;
        public int PerkLevel;
        public int Priority;
        public int RequiredCharacterLevel;

        public override string ToString()
        {
            return $"{Perk}: {PerkLevel} Pri:{Priority} ReqCharLevel:{RequiredCharacterLevel}";
        }
    }
}