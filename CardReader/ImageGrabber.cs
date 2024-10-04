using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace CardReader;

public abstract class ImageGrabber
{
    public virtual void ActivateWebcam() { }

    public virtual void DeactivateWebcam() { }

    public abstract Task<Image<Rgba32>> GrabImageAsync(CancellationToken cancellationToken);
}
