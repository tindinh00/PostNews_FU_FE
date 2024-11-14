using System.Text.Json;

namespace Client.Helpers
{
    public static class SessionHelper
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }
        public static T GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json != null ? JsonSerializer.Deserialize<T>(json) : default(T);
        }
        public static void Remove(this ISession session, string key)
        {
            session.Remove(key);
        }
    }

}
