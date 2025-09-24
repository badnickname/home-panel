using NAudio.Wave;

namespace WebPanel.Server.Services;

public sealed class ListenService(IVoiceService voice, WaveInEvent waveIn) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        waveIn.DataAvailable += DataAvailable;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        waveIn.StopRecording();
        return Task.CompletedTask;
    }

    private void DataAvailable(object? sender, WaveInEventArgs e) => voice.Put(e.Buffer, e.BytesRecorded);
}