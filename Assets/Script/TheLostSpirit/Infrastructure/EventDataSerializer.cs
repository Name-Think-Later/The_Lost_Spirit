using System;
using Sirenix.Serialization;
using UnityEngine;

namespace TheLostSpirit.Infrastructure
{
    public static class EventDataSerializer
    {
        public static EventData Deserialize(string dataStr)
        {
            if (string.IsNullOrEmpty(dataStr))
            {
                return new EventData();
            }

            try
            {
                var bytes = Convert.FromBase64String(dataStr);
                return SerializationUtility.DeserializeValue<EventData>(bytes, DataFormat.Binary);
            }
            catch
            {
                try
                {
                    return JsonUtility.FromJson<EventData>(dataStr);
                }
                catch
                {
                    return new EventData();
                }
            }
        }

        public static string Serialize(EventData data)
        {
            if (data == null)
            {
                return "";
            }

            var bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
            return Convert.ToBase64String(bytes);
        }
    }
}
