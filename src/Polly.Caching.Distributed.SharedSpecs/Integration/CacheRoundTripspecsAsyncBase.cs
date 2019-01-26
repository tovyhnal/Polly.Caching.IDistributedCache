﻿#if !NETCOREAPP1_1

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;

namespace Polly.Caching.Distributed.Specs.Integration
{
    public abstract class CacheRoundTripSpecsAsyncBase<TCache> : CacheRoundTripSpecsBase
    {
        public override async Task Should_roundtrip_this_variant_of<TResult>(TResult testValue)
        {
            // Arrange
            var (cacheProvider, cache) = CachePolicyFactory.CreateAsyncCachePolicy<TCache, TResult>();

            // Assert - should not be in cache
            (bool cacheHit1, TResult fromCache1) = await cacheProvider.TryGetAsync(OperationKey, CancellationToken.None, false);
            cacheHit1.Should().BeFalse();
            fromCache1.Should().Be(default(TResult));

            // Act - should execute underlying delegate and place in cache
            int underlyingDelegateExecuteCount = 0;
            (await cache.ExecuteAsync(ctx =>
                {
                    underlyingDelegateExecuteCount++;
                    return Task.FromResult(testValue);
                }, new Context(OperationKey)))
                .ShouldBeEquivalentTo(testValue);

            // Assert - should have executed underlying delegate
            underlyingDelegateExecuteCount.Should().Be(1);

            // Assert - should be in cache
            (bool cacheHit2, TResult fromCache2) = await cacheProvider.TryGetAsync(OperationKey, CancellationToken.None, false);
            cacheHit2.Should().BeTrue();
            fromCache2.ShouldBeEquivalentTo(testValue);

            // Act - should execute underlying delegate and place in cache
            (await cache.ExecuteAsync(ctx =>
                {
                    underlyingDelegateExecuteCount++;
                    throw new Exception("Cache should be used so this should not get invoked.");
                }, new Context(OperationKey)))
                .ShouldBeEquivalentTo(testValue);
            underlyingDelegateExecuteCount.Should().Be(1);
        }
    }
}

#endif