using WebPanel.Server.Models;

using Timer = WebPanel.Server.Models.Timer;

namespace WebPanel.Server.Services;

public interface IActorService
{
    void Act(string command);
}

public sealed class ActorService(ISettingsService service, INotifyService notify) : IActorService
{
    public void Act(string command)
    {
        if (string.IsNullOrWhiteSpace(command)) return;
        var raw = command.ToLower().Trim().Split(' ');
        if (!Array.Exists(raw, x => x is "ок" or "окей" or "оке")) return;
        if (Array.Exists(raw, x => x is "громче" or "громкий" or "громко" or "увеличь" or "увеличить" or "увеличься"))
        {
            var volume = service.Volume;
            service.Volume = new Volume(Math.Min(100, volume.Level + 10));
            notify.Success();
        }
        else if (Array.Exists(raw, x => x is "тише" or "тихий" or "тихо" or "уменьши" or "уменьшить" or "уменьшись"))
        {
            var volume = service.Volume;
            service.Volume = new Volume(Math.Max(0, volume.Level - 10));
            notify.Success();
        }
        else if (Array.Exists(raw, x => x is "выключи" or "выключить" or "таймер" or "поставь"))
        {
            service.Timer = new Timer(9999);
            notify.Success();
        }
        else if (Array.Exists(raw, x => x is "отмени" or "отменить" or "отмена" or "стоп"))
        {
            service.Timer = new Timer(0);
            notify.Success();
        }
    }
}