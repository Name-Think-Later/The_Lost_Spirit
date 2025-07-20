namespace Script.TheLostSpirit.Circuit.Skill {
    public class SkillBase {
        readonly string _name;

        public SkillBase(string name) {
            _name = name;
        }

        public string Name => _name;
    }
}