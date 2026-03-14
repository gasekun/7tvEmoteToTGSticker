# 7TV Emote to Telegram Sticker Converter

A web application that converts 7TV emotes to Telegram-compatible video stickers (.webm format). Built with .NET 10 and Blazor.

## Features

- 🔍 **Search 7TV Users** - Find any 7TV user and browse their emotes
- 🎨 **Animated Emote Support** - Handles animated WebP emotes seamlessly
- 📱 **Telegram Sticker Export** - Convert emotes to VP9-encoded WebM videos
- 👤 **Twitch Integration** - Display Twitch user information alongside 7TV profiles
- ⚡ **Real-time Search** - Instant search results with loading indicators
- 📊 **Responsive UI** - Works across devices with a modern interface

## Technology Stack

- **Frontend**: Blazor Server, .NET 10
- **Backend**: ASP.NET Core, C# 14.0
- **APIs**: 7TV GraphQL API, Twitch (via ivr.fi)
- **Image Processing**: ImageMagick.NET, FFMpeg
- **HTTP**: GraphQL Client with System.Text.Json serialization

## Prerequisites

- .NET 10 SDK
- FFmpeg (for video encoding)
- Windows, Linux, or macOS

## Installation

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/7tvEmoteToTGSticker.git
cd 7tvEmoteToTGSticker
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build the Project
```bash
dotnet build
```

### 4. Run the Application
```bash
dotnet run
```

The application will start at `https://localhost:5001` (or `http://localhost:5000`)

## Project Structure

```
7tvEmoteToTGSticker/
├── Components/
│   └── Pages/
│       ├── Home.razor              # Search page
│       └── UserPage.razor          # User profile & emote display
├── Models/
│   ├── UserDbo.cs                  # User data object
│   ├── User.cs                     # 7TV user model
│   ├── ItemEmote.cs                # Emote model
│   ├── StickResult.cs              # Conversion result
│   └── TwitchModel/                # Twitch-related models
├── Services/
│   ├── SevenTVInfoService.cs       # 7TV GraphQL queries
│   ├── TwitchInfoService.cs        # Twitch API integration
│   └── ConvertEmoteToStickerService.cs  # Emote conversion
├── wwwroot/                        # Static files & styles
└── Program.cs                      # Application configuration
```

## Usage

### 1. Search for a User
- Navigate to the home page
- Enter a 7TV username in the search box
- Click on a user to view their profile

### 2. Browse Emotes
- View all emotes from the user's active emote set
- Filter emotes using the search box
- Click on an emote to see details

### 3. Download as Sticker
- Select an emote from the modal
- Click "Download sticker"
- The WebM file will download automatically
- Add to Telegram: Open Telegram → Stickers → Create → Upload the file

## API Integration

### 7TV GraphQL
- **Endpoint**: `https://7tv.io/v4/gql`
- **Queries**:
  - `GlobalSearch` - Search for users
  - `UserQuery` - Get user details and emotes

### Twitch (via ivr.fi)
- **Endpoint**: `https://api.ivr.fi/v2/`
- **Query**: `twitch/user?id={id}` - Get user info

## How It Works

1. **Download**: Fetches the emote WebP file from 7TV CDN
2. **Extract**: ImageMagick extracts all frames from animated WebP
3. **Resize**: Scales frames to 512px while maintaining aspect ratio
4. **Encode**: FFmpeg encodes frames as VP9 WebM video for Telegram
5. **Download**: Sends the final WebM file to the user's browser

## Performance Optimizations

- **Parallel Processing**: Frame extraction uses all CPU cores
- **Dynamic FPS**: Calculates optimal frame rate from animation delays
- **Lazy Loading**: Images load on-demand in the UI
- **Optimized GraphQL**: Minimal query payloads (removed 170+ unused fields)
- **Resource Cleanup**: Temporary files deleted after processing

## Configuration

### Customize Output
Edit `ConvertEmoteToStickerService.cs`:
```csharp
.WithVideoBitrate(256)           // Adjust video quality (kbps)
.WithFramerate(fps)              // Frame rate (auto-calculated)
.WithDuration(TimeSpan.FromSeconds(3))  // Max duration
```

### Change API Endpoints
- 7TV: `SevenTVInfoService.cs` - `GraphQLEndpoint` constant
- Twitch: `TwitchInfoService.cs` - `apiEndpoint` field

## Error Handling

The application includes comprehensive error handling:
- ✅ File validation after download
- ✅ Dimension validation (prevents 0x0 images)
- ✅ GraphQL error responses
- ✅ Graceful fallbacks for missing data
- ✅ User-friendly error messages

## Troubleshooting

### FFmpeg Not Found
- **Windows**: Install [FFmpeg](https://ffmpeg.org/download.html) and add to PATH
- **Linux**: `sudo apt-get install ffmpeg`
- **macOS**: `brew install ffmpeg`

### WebP Conversion Fails
- Ensure FFmpeg is properly installed
- Check file permissions in temp directory
- Verify the emote URL is accessible

### Empty Results
- Try a different search query
- Verify 7TV API is accessible: https://7tv.io/v4/gql
- Check browser console for errors

## Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| FFMpegCore | 5.4.0 | Video encoding |
| FFMpegCore.Extensions.Downloader | 5.0.0 | FFmpeg auto-download |
| Magick.NET-Q16-x64 | 14.10.4 | Image processing |
| GraphQL.Client | 6.1.0 | GraphQL requests |
| GraphQL.Client.Serializer.SystemTextJson | 6.1.0 | JSON serialization |

## License

This project is licensed under the MIT License - see LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit pull requests.

### Development Guidelines
- Follow C# naming conventions (PascalCase for classes, camelCase for locals)
- Use async/await for I/O operations
- Implement null safety with nullable reference types
- Add error handling for external API calls
- Test with various emote types (static, animated, different sizes)

## Acknowledgments

- [7TV](https://7tv.io) - Emote platform
- [FFmpeg](https://ffmpeg.org) - Video processing
- [ImageMagick](https://imagemagick.org) - Image manipulation
- [Telegram](https://telegram.org) - Sticker format specifications

## Support

Found a bug or have a feature request? Open an [issue](https://github.com/yourusername/7tvEmoteToTGSticker/issues)

## Disclaimer

This tool is for personal use. Ensure you have the rights to convert and use emotes according to 7TV's terms of service.

---

**Status**: Active Development  
**.NET Version**: 10.0  
**C# Version**: 14.0  
**Last Updated**: 2024
