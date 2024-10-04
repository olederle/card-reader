using CardReader.Camera;

namespace CardReader;

public static class TestRunner
{
    public static void Main(string[] args)
    {
        CameraGrabber cameraGrabber = new();
        CardReader cr = new(cameraGrabber);
        cr.TurnOn();
        string? qrCode = cr.GrabQRAsync().GetAwaiter().GetResult();
        System.Console.WriteLine(qrCode);
        cr.TurnOff();
    }
}
