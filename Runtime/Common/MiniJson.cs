using QonversionUnity.MiniJSON;

namespace QonversionUnity
{
	public class MiniJson
	{
		public static string JsonEncode(object json)
		{
			return Json.Serialize(json);
		}

		public static object JsonDecode(string json)
		{
			return Json.Deserialize(json);
		}
	}
}
