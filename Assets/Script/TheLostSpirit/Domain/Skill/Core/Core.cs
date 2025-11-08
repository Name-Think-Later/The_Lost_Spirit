namespace TheLostSpirit.Domain.Skill.Core
{
    public class Core
    {
        readonly CoreConfig _config;

        public IInputPolicy InputPolicy => _config.inputPolicy;
        public IOutputPolicy OutputPolicy => _config.outputPolicy;

        public Core(CoreConfig config) {
            _config = config;
        }
    }
}