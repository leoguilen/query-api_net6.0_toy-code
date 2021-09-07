var builder = WebApplication.CreateBuilder();
builder.Services
    .AddApi()
    .AddIoC(builder.Configuration);

var app = builder.Build();
app.UseApi(builder.Configuration);

await app.RunAsync();
