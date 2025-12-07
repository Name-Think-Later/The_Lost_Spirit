using TheLostSpirit.Identity.ConfigID;

namespace TheLostSpirit.Domain
{
    public interface IConfig<out TConfigID> where TConfigID : IConfigID
    {
        TConfigID ID { get; }
    }
}