using _7tvEmoteToTGSticker.Models.GlobalSearchModel;

namespace _7tvEmoteToTGSticker.Models.SevenTVModel;

// UserSearch myDeserializedClass = JsonConvert.DeserializeObject<UserSearch>(myJsonResponse);

public class User
{
    public string id { get; set; }
    public MainConnection mainConnection { get; set; }
    public Style style { get; set; }
}