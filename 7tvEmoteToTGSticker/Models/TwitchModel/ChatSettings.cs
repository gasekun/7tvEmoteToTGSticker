namespace _7tvEmoteToTGSticker.Models.TwitchModel;

public class ChatSettings
{
    public int chatDelayMs { get; set; }
    public object followersOnlyDurationMinutes { get; set; }
    public object slowModeDurationSeconds { get; set; }
    public bool blockLinks { get; set; }
    public bool isSubscribersOnlyModeEnabled { get; set; }
    public bool isEmoteOnlyModeEnabled { get; set; }
    public bool isFastSubsModeEnabled { get; set; }
    public bool isUniqueChatModeEnabled { get; set; }
    public bool requireVerifiedAccount { get; set; }
    public List<string> rules { get; set; }
}