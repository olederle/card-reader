using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace CardReader.Camera;

public sealed class CameraGrabber : ImageGrabber
{
    public override void ActivateWebcam() { }

    public override void DeactivateWebcam() { }

    public override Task<Image<Rgba32>> GrabImageAsync(CancellationToken cancellationToken)
    {
        // TODO
        return Task.FromResult(new Image<Rgba32>(1, 1));
    }
}
