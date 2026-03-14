using _7tvEmoteToTGSticker.Components;
using _7tvEmoteToTGSticker.Services;
using FFMpegCore;
using FFMpegCore.Extensions.Downloader;

var path = "";

#if DEBUG
path = Path.GetDirectoryName(Environment.ProcessPath);
#else
path = Environment.CurrentDirectory;
#endif

if (!Directory.Exists($"{path}/bin"))
    Directory.CreateDirectory($"{path}/bin");

if (!File.Exists($"{path}/tmp"))
    Directory.CreateDirectory($"{path}/tmp");

GlobalFFOptions.Configure(new FFOptions
{
    BinaryFolder = $"{path}/bin",
    TemporaryFilesFolder = $"{path}/tmp"
});

if (!File.Exists($"{path}/bin/ffmpeg.exe"))
{
    Console.WriteLine("FFmpeg binaries not found. Downloading...");
    await FFMpegDownloader.DownloadBinaries();
}


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddTransient<ConvertEmoteToStickerService>();
builder.Services.AddTransient<TwitchInfoService>();
builder.Services.AddTransient<SevenTVInfoService>();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();