using Microsoft.OpenApi;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenTelemetry().ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
    .WithTracing(tracing =>
    {
        tracing
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation();
    })
    .WithMetrics(metrics =>
    {
        metrics
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation();
    })
    .UseOtlpExporter();

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.IncludeFormattedMessage = true;
});


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
