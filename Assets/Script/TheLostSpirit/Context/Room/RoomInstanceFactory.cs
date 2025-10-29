using Extension.Linq;
using Script.TheLostSpirit.Application.ObjectContextContract;
using Script.TheLostSpirit.Application.ObjectContextContract.ObjectFactory;
using UnityEngine;

namespace TheLostSpirit.Context.Room
{
    public class RoomInstanceFactory : IRoomInstanceFactory
    {
        readonly RoomInstanceFactoryConfig _config;

        Vector2 _spawnPosition;

        public RoomInstanceFactory(RoomInstanceFactoryConfig config) {
            _config = config;
        }

        public IRoomInstanceContext Create() {
            var prefab   = _config.roomObjectContexts.GetRandom();
            var parent   = _config.roomDrawingGrid.transform;
            var instance = Object.Instantiate(prefab, _spawnPosition, Quaternion.identity, parent);
            instance.Construct();

            _spawnPosition += Vector2.right * _config.horizontalOffset;

            return instance;
        }

        public void Reset() => _spawnPosition = Vector2.zero;
    }
}