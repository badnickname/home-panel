namespace WebPanel.Server.Services;

public interface INotifyService
{
    void Success();
}

public class NotifyService : INotifyService
{
    private readonly System.Media.SoundPlayer _player = new("success.wav");

    public void Success()
    {
        _player.Play();
    }
}