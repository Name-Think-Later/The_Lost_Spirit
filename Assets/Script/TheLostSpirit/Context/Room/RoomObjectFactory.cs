using Extension.Linq;
using Script.TheLostSpirit.Application.ObjectContextContract;
using Script.TheLostSpirit.Application.ObjectContextContract.ObjectFactory;
using UnityEngine;

namespace TheLostSpirit.Context.Room
{
    public class RoomObjectFactory : IRoomObjectFactory
    {
        readonly RoomObjectFactoryConfig _config;

        Vector2 _spawnPosition;

        public RoomObjectFactory(RoomObjectFactoryConfig config) {
            _config = config;
        }

        public IRoomObjectContext Create() {
            var prefab   = _config.roomObjectContexts.GetRandom();
            var parent   = _config.roomDrawingGrid.transform;
            var instance = Object.Instantiate(prefab, _spawnPosition, Quaternion.identity, parent);
            instance.Instantiate();

            _spawnPosition += Vector2.right * _config.horizontalOffset;

            return instance;
        }

        public void Reset() => _spawnPosition = Vector2.zero;
    }
}