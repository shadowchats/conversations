using Confluent.Kafka;
using Shadowchats.Conversations.Application.IntegrationEvents;
using Shadowchats.Conversations.Application.Interfaces;

namespace Shadowchats.Conversations.Infrastructure;

public sealed class KafkaIntegrationEventBroker : IIntegrationEventBroker, IDisposable
{
    public KafkaIntegrationEventBroker(KafkaConfig config)
    {
        _config = config;

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = config.BootstrapServers,
            GroupId = config.ConsumerGroupId,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
            MaxPollIntervalMs = 300000
        };

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config.BootstrapServers,
            Acks = Acks.All,
            EnableIdempotence = true,
            MaxInFlight = 1
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        _producer = new ProducerBuilder<string, string>(producerConfig).Build();
        
        _consumer.Subscribe(config.InboxTopics);
    }

    public List<IntegrationEventEnvelope> Consume(int batchSize)
    {
        var events = new List<IntegrationEventEnvelope>();
        var deadline = DateTime.UtcNow + _consumeTimeout; 
        ConsumeResult<string, string>? lastConsumed = null;

        while (events.Count < batchSize && DateTime.UtcNow < deadline)
        {
            var result = _consumer.Consume(TimeSpan.FromMilliseconds(100));
            if (result == null)
                break;

            lastConsumed = result;
            var container = Deserialize(result.Message.Value);
            events.Add(container);
        }

        _lastConsumed = lastConsumed;
        return events;
    }

    public async Task Publish(IReadOnlyCollection<OutboxIntegrationEventContainer> events, CancellationToken cancellationToken)
    {
        foreach (var @event in events)
        {
            var message = new Message<string, string>
            {
                Key = @event.Id.ToString(),
                Value = Serialize(@event),
                Headers = new Headers
                {
                    { "traceparent", System.Text.Encoding.UTF8.GetBytes(@event.Traceparent) },
                    { "event-type", System.Text.Encoding.UTF8.GetBytes(@event.EventType) }
                }
            };

            await _producer.ProduceAsync(_config.OutboxTopic, message, cancellationToken);
        }

        if (events.Count != 0)
            _producer.Flush(TimeSpan.FromSeconds(2));
    }

    public void Commit()
    {
        if (_lastConsumed != null) 
            _consumer.Commit(_lastConsumed);
    }

    public void Dispose()
    {
        try
        {
            _consumer.Close();
        }
        finally
        {
            _consumer.Dispose();
            _producer.Flush(TimeSpan.FromSeconds(2));
            _producer.Dispose();
        }
    }

    private readonly KafkaConfig _config;
    
    private readonly IConsumer<string, string> _consumer;
    
    private readonly IProducer<string, string> _producer;
    
    private readonly TimeSpan _consumeTimeout = TimeSpan.FromSeconds(5);
    
    private ConsumeResult<string, string>? _lastConsumed;
}

public sealed record KafkaConfig
{
    public required string BootstrapServers { get; init; }
    
    public required string ConsumerGroupId { get; init; }
    
    public required IReadOnlyCollection<string> InboxTopics { get; init; }
    
    public required string OutboxTopic { get; init; }
}