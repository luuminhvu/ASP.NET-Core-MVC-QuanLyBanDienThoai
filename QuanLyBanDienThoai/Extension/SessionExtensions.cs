namespace QuanLyBanDienThoai.Extension
{
    public static class SessionExtensions
    {
        public static void Set<T>(this Microsoft.AspNetCore.Http.ISession session, string key, T value)
        {
            session.SetString(key, Newtonsoft.Json.JsonConvert.SerializeObject(value));
        }
        public static T Get<T>(this Microsoft.AspNetCore.Http.ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }
    }
}
