using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace CardReader.Tests;

public sealed class CardReaderTest
{
    public CardReaderTest()
    {
        CardReader.MaxDuration = 1000;
    }

    [Fact]
    public void TurnOnTest()
    {
        LocalGrabber lg = new();

        using CardReader cr = new(lg);

        Assert.Equal(CardReader.OffState, cr.CheckState());

        cr.TurnOn();

        Assert.Equal(CardReader.OnState, cr.CheckState());

        cr.TurnOn();

        Assert.Equal(CardReader.OnState, cr.CheckState());
    }

    [Fact]
    public void TurnOffTest()
    {
        // TBD
    }

    [Fact]
    public void TestValidQrCode()
    {
        // TBD
    }

    [Fact]
    public void TestValidQrCodeInSecondTry()
    {
        // TBD
    }
}
