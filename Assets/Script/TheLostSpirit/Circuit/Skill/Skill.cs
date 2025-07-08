namespace Script.TheLostSpirit.Circuit {
    public class Skill {
        readonly string _name;

        public Skill(string name) {
            _name = name;
        }

        public string Name => _name;
    }
}