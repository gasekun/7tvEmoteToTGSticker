namespace _7tvEmoteToTGSticker.Models.TwitchModel;

// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);

public class TwitchUser
{
    public bool banned { get; set; }
    public string? displayName { get; set; }
    public string? login { get; set; }
    public string id { get; set; }
    public string? bio { get; set; }
    public object? follows { get; set; }
    public int? followers { get; set; }
    public object? profileViewCount { get; set; }
    public string? chatColor { get; set; }
    public string? logo { get; set; }
    public string? banner { get; set; }
    public object? verifiedBot { get; set; }
    public DateTime? createdAt { get; set; }
    public DateTime? updatedAt { get; set; }
    public string? emotePrefix { get; set; }
    public Roles? roles { get; set; }
    public int? chatterCount { get; set; }
    public ChatSettings? chatSettings { get; set; }
    public Stream? stream { get; set; }
    public LastBroadcast? lastBroadcast { get; set; }
    public List<Panel>? panels { get; set; }
}