using System.Collections.Concurrent;

namespace BuildingBlocks.EventStoreDB.Subscriptions;

public class InMemorySubscriptionCheckpointRepository : ISubscriptionCheckpointRepository
{
    private readonly ConcurrentDictionary<string, ulong> checkpoints = new();

    public ValueTask<ulong?> Load(string subscriptionId, CancellationToken ct)
    {
        return new(checkpoints.TryGetValue(subscriptionId, out var checkpoint) ? checkpoint : null);
    }

    public ValueTask Store(string subscriptionId, ulong position, CancellationToken ct)
    {
        checkpoints.AddOrUpdate(subscriptionId, position, (_, _) => position);

        return ValueTask.CompletedTask;
    }
}