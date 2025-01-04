
using Microsoft.AspNetCore.SignalR;
using OpenCVAspNetTest.Hubs;
using OpenCvSharp;

namespace OpenCVAspNetTest;

public class ImageCreationHostedService : IHostedService
{
    private Timer _timer;
    private VideoCapture _capture;
    private readonly IHubContext<ImageHub> _imageHubContext;

    public ImageCreationHostedService(IHubContext<ImageHub> imageHubContext)
    {
        _capture = new(1);
        _imageHubContext = imageHubContext;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1000));
    }

    public async void DoWork(object state)
    {
        if (!_capture.IsOpened())
        {
            return;
        }

        Mat image = new();

        _capture.Read(image);

        if (image.Empty())
        {
            return;
        }

        await _imageHubContext.Clients.All.SendAsync("ReceiveNewImage", image.ToBytes());
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
    }
}
