namespace _7tvEmoteToTGSticker.Models.TwitchModel;

public class Stream
{
    public string title { get; set; }
    public string id { get; set; }
    public DateTime createdAt { get; set; }
    public string type { get; set; }
    public int viewersCount { get; set; }
    public Game game { get; set; }
}