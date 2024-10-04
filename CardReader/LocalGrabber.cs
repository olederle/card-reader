using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace CardReader;

public sealed class LocalGrabber : ImageGrabber
{
    private string[] images = [];
    private int index;

    public string[] Images
    {
        get { return images; }
        set
        {
            images = value;
            index = -1;
        }
    }

    public override async Task<Image<Rgba32>> GrabImageAsync(CancellationToken cancellationToken)
    {
        if (index + 1 <= images.Length)
        {
            index++;
        }
        try
        {
            await Task.Delay(200, cancellationToken);
        }
        catch (TaskCanceledException) { }
        try
        {
            return await Image.LoadAsync<Rgba32>(images[index], cancellationToken);
        }
        catch (IOException)
        {
            return new Image<Rgba32>(1, 1);
        }
    }
}
