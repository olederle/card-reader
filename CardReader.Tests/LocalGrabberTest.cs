using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace CardReader.Tests;

public sealed class LocalGrabberTest
{
    private static bool AreImagesEqual(Image<Rgba32> img1, Image<Rgba32> img2)
    {
        int width1 = img1.Width;
        int width2 = img2.Width;
        int height1 = img1.Height;
        int height2 = img2.Height;
        bool imagesAreEqual = true;

        if (width1 == width2 && height1 == height2)
        {
            for (int x = 0; imagesAreEqual && x < width1; x++)
            {
                for (int y = 0; imagesAreEqual && y < height1; y++)
                {
                    if (img1[x, y] != img2[x, y])
                    {
                        imagesAreEqual = false;
                    }
                }
            }
        }
        else
        {
            imagesAreEqual = false;
        }
        return imagesAreEqual;
    }

    private static Image<Rgba32> ReadImage(string name)
    {
        try
        {
            return Image.Load<Rgba32>(name);
        }
        catch (IOException e)
        {
            Assert.Fail(e.Message);
            // an diese Stelle kommen wir nicht mehr, aber der Compiler weiß das nicht
            return null!;
        }
    }

    [Fact]
    public async Task OneImageAsync()
    {
        LocalGrabber lg = new() { Images = [Path.Combine("data", "invalid1.png")] };

        Image<Rgba32> img1 = await lg.GrabImageAsync(default);
        Image<Rgba32> img2 = await lg.GrabImageAsync(default);

        // es dürfen keine NULL Werte geliefert werden
        Assert.NotNull(img1);
        Assert.NotNull(img2);

        // alle gelieferten Bilder müssen übereinstimmen
        Assert.True(AreImagesEqual(img1, img2), "Die Bilder stimmen nicht wie erwartet überein.");

        // zuletzt prüfen wir noch, ob übheraupt das richtige Bild geöffnet wurde
        Assert.True(
            AreImagesEqual(img1, ReadImage(Path.Combine("data", "invalid1.png"))),
            "Es wurde nicht data/invalid1.png geladen"
        );
    }

    [Fact]
    public void ThreeImages()
    {
        LocalGrabber lg =
            new()
            {
                Images =
                [
                    Path.Combine("data", "invalid1.png"),
                    Path.Combine("data", "invalid2.png"),
                    Path.Combine("data", "invalid3.png"),
                ],
            };

        // TBD
    }

    [Fact]
    public void TwoImages()
    {
        LocalGrabber lg =
            new()
            {
                Images =
                [
                    Path.Combine("data", "invalid1.png"),
                    Path.Combine("data", "invalid2.png"),
                ],
            };
    }
}
