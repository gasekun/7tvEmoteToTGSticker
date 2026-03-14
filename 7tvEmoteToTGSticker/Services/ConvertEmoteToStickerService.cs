using FFMpegCore;
using ImageMagick;
using System.Net;

namespace _7tvEmoteToTGSticker.Services;

public class ConvertEmoteToStickerService
{
    public async Task<StickResult> GetStickerFromEmojiAsync(string emojiId)
    {
        var result = new StickResult();
        var tmp = Path.GetTempFileName() + ".webp";
        var tmpOutput = Path.GetTempFileName() + ".webm";

        try
        {
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("User-Agent", "Mozilla/5.0");
                webClient.DownloadFile($"https://cdn.7tv.app/emote/{emojiId}/4x.webp", tmp);
            }

            if (!File.Exists(tmp) || new FileInfo(tmp).Length == 0)
                return result with { ErrorMessage = "Error create tmp file" };

            var folder = $"{Environment.CurrentDirectory}/tmp/{emojiId}/";
            Directory.CreateDirectory(folder);

            var sumDelay = 0;
            var fps = 10;

            using (var collection = new MagickImageCollection(tmp))
            {
                collection.Coalesce();
                
                Parallel.ForEach(collection,
                    new ParallelOptions {MaxDegreeOfParallelism = System.Environment.ProcessorCount},
                    (image, state, index) =>
                    {
                        sumDelay += (int)image.AnimationTicksPerSecond + (int)image.AnimationDelay;

                        image.Format = MagickFormat.Png;
                        var size = new MagickGeometry
                        {
                            IgnoreAspectRatio = true
                        };

                        if (image.Width == image.Height)
                        {
                            size.Width = size.Height = 512;
                        }
                        else
                        {
                            var cofH = image.Height / (float)image.Width;
                            var cofW = image.Width / (float)image.Height;

                            if (image.Width > image.Height)
                            {
                                size.Width = 512;
                                size.Height = (uint)(512 * cofH);
                            }
                            else
                            {
                                size.Width = (uint)(512 * cofW);
                                size.Height = 512;
                            }

                        }


                        image.Resize(size);
                        image.Strip();
                        image.Write(Path.Combine(folder, $"image_{index:D}.png"));
                    });


                fps = (int)(1000 / (sumDelay / (float)collection.Count));
            }
            
            //Render webm for telegram
            if (await FFMpegArguments.FromFileInput(Path.Combine(folder, "image_%d.png"), false)
                    .OutputToFile(tmpOutput, false, option =>
                    {
                        option.WithVideoCodec("libvpx-vp9")
                            .WithVideoBitrate(256)
                            .ForcePixelFormat("yuva420p")
                            .WithFramerate(fps)
                            .WithDuration(TimeSpan.FromSeconds(3));
                    }).ProcessAsynchronously())
            {
                var data = await File.ReadAllBytesAsync(tmpOutput);

                Directory.Delete(folder, true);
                return result with { IsCreated = true, Data = data };
            }



        }
        catch (Exception e)
        {
            return result with { ErrorMessage = e.Message };
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
            if (File.Exists(tmpOutput)) File.Delete(tmpOutput);
        }

        return result with { ErrorMessage = "Unknown error" };
    }
}

public struct StickResult()
{
    public byte[] Data;
    public bool IsCreated;
    public string ErrorMessage;
}