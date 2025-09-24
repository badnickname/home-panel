using System.Diagnostics;
using System.Timers;
using AudioSwitcher.AudioApi.CoreAudio;
using WebPanel.Server.Models;

using NAudio.Wave;

using Timer = WebPanel.Server.Models.Timer;

namespace WebPanel.Server.Services;

public interface ISettingsService
{
    public Volume Volume { get; set; }
    public Timer Timer { get; set; }
    public Voice Voice { get; set; }
}

public sealed class SettingsService(CoreAudioController controller, IRecognizerKeeper keeper, WaveInEvent wave) : ISettingsService
{
    private int _timeout;
    private readonly System.Timers.Timer _timer = new();
    private bool _voice;
    private readonly SemaphoreSlim _semaphore = new(1);

    public Voice Voice
    {
        get => new(_voice);
        set
        {
            _semaphore.Wait();
            try
            {
                if (!value.Listen)
                {
                    if (_voice) wave.StopRecording();
                    Task.Delay(2000).Wait();
                    keeper.Clear();
                }
                else if (!_voice)
                {
                    wave.StartRecording();
                }

                _voice = value.Listen;
                Task.Delay(2000).Wait();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public Volume Volume
    {
        get => new((int) controller.DefaultPlaybackDevice.Volume);
        set => controller.DefaultPlaybackDevice.Volume = value.Level;
    }

    public Timer Timer
    {
        get => new(_timeout);
        set
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                _timer.Elapsed -= TimerHandler;
            }
            Process.Start("shutdown", "/a");
            if (value.Seconds > 100)
            {
                _timer.Elapsed += TimerHandler;
                Process.Start("shutdown", $"/s /t {value.Seconds}");
                _timer.Interval = 1000;
                _timer.Start();
            }
            _timeout = value.Seconds;
        }
    }

    private void TimerHandler(object? sender, ElapsedEventArgs e)
    {
        _timeout -= 1;
    }
}