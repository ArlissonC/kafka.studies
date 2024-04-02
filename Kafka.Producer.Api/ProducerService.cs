using Confluent.Kafka;
using Kafka.Producer.Api.Entities;
using System.Text.Json;

namespace Kafka.Producer.Api
{
    public class ProducerService
    {
        private readonly IConfiguration _configuration;
        private readonly ProducerConfig _producerConfig;
        private readonly ILogger<ProducerService> _logger;

        public ProducerService(IConfiguration configuration, ILogger<ProducerService> logger)
        {
            _logger = logger;
            _configuration = configuration;

            var bootstrap = _configuration.GetSection("KafkaConfig").GetSection("BoostrapServer").Value;
            _producerConfig = new ProducerConfig()
            {
                BootstrapServers = bootstrap
            };
        }
        public async Task<string> SendMessage(Message message)
        {
            var topic = _configuration.GetSection("KafkaConfig").GetSection("TopicName").Value;
            try
            {
                using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
                {
                    string json = JsonSerializer.Serialize(message);

                    var result = await producer.ProduceAsync(topic: topic, new() { Value = json });
                    _logger.LogInformation(result.Status.ToString() + " - " + message);

                    return result.Status.ToString() + " - " + message;
                }
            }
            catch
            {
                _logger.LogError("Erro ao enviar mensagem");

                return "Erro ao enviar mensagem";
            }
        }
    }
}