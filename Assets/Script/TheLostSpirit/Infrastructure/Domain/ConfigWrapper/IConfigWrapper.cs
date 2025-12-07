using Sirenix.OdinInspector;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.ConfigID;

namespace TheLostSpirit.Infrastructure.Domain.ConfigWrapper
{
    public interface IConfigWrapper<out TConfigID, out TConfig> : IConfig<TConfigID>, ISearchFilterable
        where TConfigID : IConfigID
        where TConfig : IConfig<TConfigID>
    {
        public TConfig Inner { get; }
    }
}