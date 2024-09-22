using MessageQueueApp.Contacts;
using MessageQueueApp.Data;
using MessageQueueApp.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Set up Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the DI container
builder.Services.AddControllers();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<IMessageQueueService, RabbitMQService>();
builder.Services.AddSingleton<IMessageRepository,MessageRepository>();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseRouting();
app.MapControllers();

app.Run();
