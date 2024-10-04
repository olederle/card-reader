using System;
using System.Threading;
using System.Threading.Tasks;
using BarcodeReader.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CardReader;

public sealed class CardReader(ImageGrabber imageGrabber) : IDisposable
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Usage",
        "CA2211:Non-constant fields should not be visible",
        Justification = "Support Test"
    )]
    public static int MaxDuration = 30000;

    public static readonly int OffState = 0;
    public static readonly int OnState = 1;
    public static readonly int WorkState = 2;

    private readonly ImageGrabber imageGrabber = imageGrabber;
    private CancellationTokenSource? cancellationTokenSource;
    private int currentState;

    public ICameraImageListener? CameraImageListener { get; set; }

    public async Task<string?> GrabQRAsync()
    {
        cancellationTokenSource?.Dispose();
        cancellationTokenSource = new CancellationTokenSource();

        if (currentState == OnState)
        {
            currentState = WorkState;
            long currentTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
            bool repeat = true;
            try
            {
                do
                {
                    using Image<Rgba32> image = await imageGrabber.GrabImageAsync(
                        cancellationTokenSource.Token
                    );
                    if (CameraImageListener != null)
                    {
                        CameraImageListener.Image = image.Clone<Rgba32>();
                    }
                    BarcodeReader<Rgba32> reader = new(types: ZXing.BarcodeFormat.QR_CODE);

                    // then we can get a result by decoding
                    BarcodeResult<Rgba32> result = await reader.DecodeAsync(image);
                    if (result.Status == Status.Found)
                    {
                        return result.Value;
                    }

                    long newTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
                    long duration = newTime - currentTime;
                    if (duration > MaxDuration)
                    {
                        repeat = false;
                    }
                } while (repeat && !cancellationTokenSource.IsCancellationRequested);
                if (repeat == false && cancellationTokenSource.IsCancellationRequested == false)
                {
                    throw new CardReaderTimeoutException();
                }
                else if (repeat == true && cancellationTokenSource.IsCancellationRequested == true)
                {
                    throw new CardReaderCancelledException();
                }
            }
            finally
            {
                currentState = OnState;
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }
        else if (currentState == OffState)
        {
            throw new CardReaderOfflineException();
        }
        else if (currentState == WorkState)
        {
            throw new CardReaderBusyException();
        }
        return null;
    }

    public void Cancel()
    {
        cancellationTokenSource?.Cancel();
    }

    public int CheckState()
    {
        return currentState;
    }

    public void Dispose()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }

    public void TurnOff()
    {
        if (currentState == OffState)
        {
            imageGrabber.DeactivateWebcam();
            currentState = 0;
        }
        else if (currentState == WorkState)
        {
            throw new CardReaderBusyException();
        }
    }

    public void TurnOn()
    {
        if (currentState == OffState)
        {
            imageGrabber.DeactivateWebcam();
            currentState = OffState;
        }
        else if (currentState == OnState) { }
    }
}
