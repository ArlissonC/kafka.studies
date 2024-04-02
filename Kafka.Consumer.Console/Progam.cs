using Kafka.Consumer.Console;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(s =>
{
    s.AddHostedService<ConsumerService>();
}).Build();

await host.RunAsync();