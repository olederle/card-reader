using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace CardReader;

public interface ICameraImageListener
{
    public Image<Rgba32> Image { set; }
}
