
using Confluent.Kafka;
using Kafka.Producer.Api.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Kafka.Consumer.Console;

public class ConsumerService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<ConsumerService> _logger;
    private readonly ConsumerConfig _consumerConfig;

    public ConsumerService(ILogger<ConsumerService> logger)
    {
        _logger = logger;
        _consumerConfig = new ConsumerConfig()
        {
            BootstrapServers = "localhost:9092",
            GroupId = "Group 1",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build();  
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("topic1");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Run(() =>
            {
                var result = _consumer.Consume(stoppingToken);
                var json = result.Message.Value;
                Message message = JsonSerializer.Deserialize<Message>(json)!;
                string messageLogger = $"GroupId: Group 1, Message: {message.Name}";
                _logger.LogInformation(messageLogger);
            });
        }
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _consumer.Close();
        _logger.LogInformation("Aplicação parou, conexão fechada");
        return Task.CompletedTask;
    }
}