using Proxy;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services
    .AddSingleton<IProxyConfigProvider>(new CustomProvider())
    .AddReverseProxy();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapReverseProxy();
});

app.Run();
