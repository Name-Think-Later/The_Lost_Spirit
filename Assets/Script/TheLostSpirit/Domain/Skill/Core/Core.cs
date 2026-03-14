namespace TheLostSpirit.Domain.Skill.Core
{
    public class Core
    {
        readonly CoreConfig _config;

        public Core(CoreConfig config) {
            _config = config;
        }

        public IInputPolicy InputPolicy => _config.InputPolicy;
        public IOutputPolicy OutputPolicy => _config.OutputPolicy;
    }
}