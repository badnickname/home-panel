using System.Text.Json.Nodes;
using System.Threading.Tasks.Dataflow;

namespace WebPanel.Server.Services;

public interface IVoiceService
{
    void Put(byte[] data, int length);
}

public sealed class VoiceService : IVoiceService, IDisposable
{
    private readonly TransformBlock<(byte[], int), string> _receiver;
    private readonly ActionBlock<string> _actor;

    public VoiceService(IRecognizerKeeper keeper, IActorService service)
    {
        _receiver = new TransformBlock<(byte[], int), string>(data =>
        {
            try
            {
                var recognizer = keeper.Get();
                if (!recognizer.AcceptWaveform(data.Item1, data.Item2)) return string.Empty;
                var txt = recognizer.FinalResult() ?? "{}";
                var result = JsonNode.Parse(txt)?["text"]?.GetValue<string>() ?? string.Empty;
                return result;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1, BoundedCapacity = 1 });
        _actor = new ActionBlock<string>(service.Act, new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1, BoundedCapacity = 1 });
        _receiver.LinkTo(_actor);
    }

    public void Put(byte[] data, int length) => _receiver.Post((data, length));

    public void Dispose()
    {
        _receiver.Complete();
        _actor.Complete();
    }
}