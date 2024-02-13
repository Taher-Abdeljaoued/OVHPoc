using OVHPoc.Shared.Services.Abstractions;
using OVHPoc.Shared.Services;
using OVHPoc.Consumer.Api;
using OVHPoc.Shared;
using MassTransit;
using OVHPoc.Consumer.Api.Services.Abstractions;
using OVHPoc.Consumer.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAlmostMailService, AlmostMailService>();
builder.Services.AddScoped<IMerelyMailService, MerelyMailService>();
builder.Services.AddScoped<IMailMigrationService, MailMigrationService>();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();
    config.SetInMemorySagaRepositoryProvider();

    var assembly = typeof(Program).Assembly;

    config.AddConsumers(assembly);
    config.AddSagaStateMachines(assembly);
    config.AddSagas(assembly);
    config.AddActivities(assembly);

    config.UsingRabbitMq((context, conf) =>
    {
        conf.Host("localhost", "/", host =>
        {
            host.Username("ovhuser");
            host.Password("ovhpass");
        });

        conf.ConfigureEndpoints(context);
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
