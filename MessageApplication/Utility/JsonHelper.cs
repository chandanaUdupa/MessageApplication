using System.Text.Json;

namespace PublicMessageStorytel.Utility
{
    public static class JsonHelper
    {
        public static string Serialize(object objToSerialize, out string err)
        {
            err = null;
            try
            {
                return JsonSerializer.Serialize(objToSerialize);
            }
            catch (JsonException exc)
            {
                err = exc.Message;
                return null;
            }
        }

        public static T Deserialize<T>(string jsonContent, out string err)
        {
            err = null;
            try
            {
                return JsonSerializer.Deserialize<T>(jsonContent);
            }
            catch (JsonException exc)
            {
                err = exc.Message;
                return default(T);
            }
        }

    }

}
