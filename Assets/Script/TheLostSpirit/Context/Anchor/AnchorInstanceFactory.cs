using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using UnityEngine;

namespace TheLostSpirit.Context.Anchor
{
    public class AnchorInstanceFactory : IAnchorInstanceFactory
    {
        readonly AnchorInstanceContext _original;

        public AnchorInstanceFactory(AnchorInstanceContext original) {
            _original = original;
        }

        public IAnchorInstanceContext Create() {
            var instance = Object.Instantiate(_original);
            instance.Construct();

            return instance;
        }
    }
}