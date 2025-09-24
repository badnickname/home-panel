using Vosk;

namespace WebPanel.Server.Services;

public interface IRecognizerKeeper
{
    VoskRecognizer Get();

    void Clear();
}

public class RecognizerKeeper : IRecognizerKeeper, IDisposable
{
    private VoskRecognizer? _recognizer;

    public VoskRecognizer Get()
    {
        _recognizer ??= new VoskRecognizer(new Model("vosk-model-small-ru-0.22"), 16000.0f);
        return _recognizer;
    }

    public void Clear()
    {
        if (_recognizer is null) return;
        _recognizer.Dispose();
        _recognizer = null;
    }

    public void Dispose() => _recognizer?.Dispose();
}