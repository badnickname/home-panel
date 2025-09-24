using System.Reflection;

using AudioSwitcher.AudioApi.CoreAudio;
using WebPanel.Server.Models;
using WebPanel.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

using NAudio.Wave;

using Timer = WebPanel.Server.Models.Timer;

var options = new WebApplicationOptions
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
};
var builder = WebApplication.CreateBuilder(options);
builder.Services.AddWindowsService(o => o.ServiceName = "WebPanel.Server");
builder.Services.AddSingleton<CoreAudioController>();
builder.Services.AddHostedService<ListenService>();
builder.Services.AddSingleton<INotifyService, NotifyService>();
builder.Services.AddSingleton<ISettingsService, SettingsService>();
builder.Services.AddSingleton<IActorService, ActorService>();
builder.Services.AddSingleton<IVoiceService, VoiceService>();
builder.Services.AddSingleton<WaveInEvent>(_ => new WaveInEvent
{
    DeviceNumber = 0,
    WaveFormat = new WaveFormat(16000, 1)
});
builder.Services.AddSingleton<IRecognizerKeeper, RecognizerKeeper>();
builder.Host.UseWindowsService(o => o.ServiceName = "WebPanel.Server");
var app = builder.Build();

app.MapWhen(c => !c.Request.Path.StartsWithSegments("/api"), b =>
{
    if (builder.Environment.IsDevelopment())
    {
        b.UseSpa(c =>
        {
            c.UseProxyToSpaDevelopmentServer("http://localhost:4200");
        });
    }
    else
    {
        b.UseStaticFiles();
        b.UseSpaStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "wwwroot")), RequestPath = ""
        });
        b.UseSpa(options => options.Options.SourcePath = Path.Combine(AppContext.BaseDirectory, "wwwroot"));
    }
});

app.MapGet("/api/volume", ([FromServices] ISettingsService service) => service.Volume);
app.MapPut("/api/volume", ([FromBody] Volume model, [FromServices] ISettingsService service) => service.Volume = model);
app.MapGet("/api/timer", ([FromServices] ISettingsService service) => service.Timer);
app.MapPut("/api/timer", ([FromBody] Timer model, [FromServices] ISettingsService service) => service.Timer = model);
app.MapGet("/api/voice", ([FromServices] ISettingsService service) => service.Voice);
app.MapPut("/api/voice", ([FromBody] Voice model, [FromServices] ISettingsService service) => service.Voice = model);

await app.RunAsync();