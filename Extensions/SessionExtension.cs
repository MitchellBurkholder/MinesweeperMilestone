using Microsoft.AspNetCore.Http;
using Newtonsoft.Json; // Changed to Newtonsoft for 2d array support
using System.Text.Json.Serialization;

namespace MinesweeperMilestone.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // Use JsonConvert.SerializeObject (handles [,] arrays)
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            // Use JsonConvert.DeserializeObject
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}