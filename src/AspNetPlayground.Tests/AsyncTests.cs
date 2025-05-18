using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.VisualBasic;
using Xunit;

namespace AsyncAwaitPractice
{
    // Problem 1: Basic Async/Await with Task<T>
    // This tests your understanding of creating and working with async methods
    public class BasicAsyncAwait
    {
        // Implement an async method that simulates fetching a user by ID
        // It should delay for 100ms and then return a string "User123"
        public async Task<string> FetchUserAsync(int userId)
        {
            await Task.Delay(100);
            return $"User{userId}";
        }

        [Fact]
        public async Task ShouldFetchUserCorrectly()
        {
            // TODO: Use FetchUserAsync and assert that it returns "User123"
            // Be sure to await the result correctly
            var result = await FetchUserAsync(123);
            Assert.Equal("User123", result);
        }
    }

    // Problem 2: Exception Handling in Async Methods
    // This tests your understanding of how exceptions work in async scenarios
    public class AsyncExceptionHandling
    {
        // Implement an async method that fails with an ArgumentException when userId is negative
        // Otherwise, it returns "User" + userId
        public async Task<string> FetchUserWithValidationAsync(int userId)
        {
            if (userId < 0) throw new ArgumentException("no negatives");
            await Task.Delay(50);
            return $"User{userId}";
        }

        [Fact]
        public async Task ShouldThrowExceptionForNegativeUserId()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await FetchUserWithValidationAsync(-1));
        }

        [Fact]
        public async Task ShouldReturnUserForPositiveUserId()
        {
            var result = await FetchUserWithValidationAsync(123);
            Assert.Equal("User123", result);
        }
    }

    // Problem 3: Task Cancellation
    // This tests your understanding of CancellationToken usage
    public class TaskCancellation
    {

        // Implement an async method that accepts a CancellationToken
        // It should check for cancellation before and during its operation
        // The method should simulate a long-running task (using Task.Delay)
        public async Task<string> LongRunningOperationAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(50);
            cancellationToken.ThrowIfCancellationRequested();
            return "foo";
        }

        [Fact]
        public async Task ShouldCompleteLongRunningOperation()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            Assert.Equal("foo", await LongRunningOperationAsync(token));
        }

        [Fact]
        public async Task ShouldRespectCancellationToken()
        {
            // TODO: Test that LongRunningOperationAsync responds to cancellation by throwing OperationCanceledException
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;
            source.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await LongRunningOperationAsync(token));
        }
    }

    // Problem 4: ValueTask Usage
    // This tests your understanding of ValueTask vs Task performance implications
    public class ValueTaskUsage
    {
        private Dictionary<string, string> _cache = new Dictionary<string, string>();

        // Implement a method that uses ValueTask<T> for potential optimization
        // It should check a cache first, and only perform an async operation if needed
        public async ValueTask<string> GetCachedDataAsync(string key)
        {
            // TODO: Implement this method
            // If the key exists in _cache, return the value immediately using ValueTask<string>.FromResult
            // Otherwise, simulate fetching it asynchronously and add it to the cache
            if (_cache.ContainsKey(key))
            {
                return _cache[key];
            }
            await Task.Delay(50);
            var val = $"_{key}_";
            _cache[key] = val;
            return val;
        }

        [Fact]
        public async Task ShouldReturnCachedValueSynchronously()
        {
            // TODO: Test that GetCachedDataAsync returns cached values synchronously
            // Hint: Call it twice with the same key and verify the second call returns immediately
            var t1 = GetCachedDataAsync("foo");
            Assert.False(t1.IsCompletedSuccessfully);
            Assert.Equal("_foo_", await t1);
            var t2 = GetCachedDataAsync("foo");
            Assert.True(t1.IsCompletedSuccessfully);
            Assert.Equal("_foo_", await t2);
        }

        [Fact]
        public async Task ShouldFetchMissingValues()
        {
            // TODO: Test that GetCachedDataAsync fetches missing values correctly
            var t1 = GetCachedDataAsync("bar");
            Assert.False(t1.IsCompletedSuccessfully);
            Assert.Equal("_bar_", await t1);
        }
    }

    // Problem 5: Parallel Task Execution
    // This tests your understanding of Task.WhenAll and parallel execution
    public class ParallelTaskExecution
    {
        // Implement a method that performs multiple async operations in parallel using Task.WhenAll
        // It should take a list of IDs and fetch data for each one concurrently
        public async Task<IEnumerable<string>> FetchMultipleItemsAsync(IEnumerable<int> ids)
        {
            var tasks = new List<Task<string>>();
            foreach (var id in ids)
            {
                tasks.Add(FetchSingleItemAsync(id));
            }
            var results = await Task.WhenAll(tasks);
            return results;
        }

        // Helper method that simulates fetching data for a single ID with a delay
        private async Task<string> FetchSingleItemAsync(int id)
        {
            await Task.Delay(100);
            return $"Item-{id}";
        }

        [Fact]
        public async Task ShouldFetchAllItemsConcurrently()
        {
            // TODO: Test that FetchMultipleItemsAsync fetches all items concurrently
            // Hint: The total time should be ~100ms, not 100ms * number of items
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var results = await FetchMultipleItemsAsync(new List<int> { 3, 6, 1, 7, 4, 5, 1 });
            watch.Stop();

            Assert.Collection(results,
               e => Assert.Equal("Item-3", e),
               e => Assert.Equal("Item-6", e),
               e => Assert.Equal("Item-1", e),
               e => Assert.Equal("Item-7", e),
               e => Assert.Equal("Item-4", e),
               e => Assert.Equal("Item-5", e),
               e => Assert.Equal("Item-1", e)
            );
            var elapsedMs = watch.ElapsedMilliseconds;
            Assert.True(elapsedMs >= 100);
            Assert.True(elapsedMs < 200);
        }
    }

    // Problem 6: Task Continuation
    // This tests your understanding of ContinueWith and task chaining
    public class TaskContinuation
    {
        // Implement a method that uses task continuation to transform results
        // Without using await, chain tasks using ContinueWith to convert a string to uppercase
        public async Task<string> TransformWithContinuationAsync(string input)
        {
            var task = Task.FromResult(input);
            for (int idx = 0; idx < input.Length; idx++)
            {

                task = task.ContinueWith(async (antecedent, iObj) =>
                    {
                        if (iObj is int i)
                        {
                            string next = antecedent.Result;
                            if (next.Length != input.Length)
                                throw new ArgumentException("what's going on");
                            try
                            {
                                if (char.IsLetter(next[i]))
                                {
                                    await Task.Delay(50);
                                    var before = next.Substring(0, i);
                                    var ch = char.ToUpper(next[i]);
                                    var after = next.Substring(i + 1, next.Length - i - 1);
                                    return before + ch + after;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                throw;
                            }
                            return next;
                        }
                        else
                        {
                            throw new ArgumentException("bad index arg");
                        }
                    },
                    idx
                ).Unwrap();
            }
            return await task;
        }

        [Fact]
        public async Task ShouldTransformUsingContinuation()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = await TransformWithContinuationAsync("foo bar");
            watch.Stop();

            Assert.Equal("FOO BAR", result);
            var elapsedMs = watch.ElapsedMilliseconds;
            Assert.True(elapsedMs >= 50 * 6);
            Assert.True(elapsedMs < 50 * 7);
        }
    }

    // Problem 7: Async Streams (IAsyncEnumerable)
    // This tests your understanding of async streams introduced in C# 8
    public class AsyncStreams
    {
        // Implement a method that returns an async stream of integers
        // It should yield return numbers from 1 to count with a delay between each
        public async IAsyncEnumerable<int> GenerateNumbersAsync(int count)
        {
            for (int i = 0; i < count; i++)
            {
                await Task.Delay(50);
                yield return i;
            }
        }

        [Fact]
        public async Task ShouldConsumeAsyncStream()
        {
            // TODO: Test that GenerateNumbersAsync produces the expected sequence
            // Hint: Use await foreach to consume the async stream
            var l = new List<int> { };
            var watch = System.Diagnostics.Stopwatch.StartNew();
            await foreach (var i in GenerateNumbersAsync(6))
            {
                l.Add(i);
            }

            Assert.Equal(new List<int> { 0, 1, 2, 3, 4, 5 }, l);
            var elapsedMs = watch.ElapsedMilliseconds;
            Assert.True(elapsedMs >= 50 * 6);
            Assert.True(elapsedMs < 50 * 7);
        }
    }

    // Problem 8: ConfigureAwait Usage
    // This tests your understanding of context capturing and ConfigureAwait
    public class ConfigureAwaitUsage
    {
        // Implement a method that demonstrates proper ConfigureAwait usage
        // This simulates a library method that shouldn't capture the context
        public async Task<string> LibraryMethodAsync()
        {
            // TODO: Implement this method using ConfigureAwait(false) appropriately
            await Task.Delay(50).ConfigureAwait(false);
            return "foo";
        }

        [Fact]
        public async Task ShouldUseConfigureAwaitCorrectly()
        {
            // TODO: Test that LibraryMethodAsync works correctly
            // Note: It's difficult to test ConfigureAwait directly in a unit test,
            // so this is more about demonstrating that you know when and how to use it
            Assert.Equal("foo", await LibraryMethodAsync());
        }
    }

    // Problem 9: Deadlock Scenarios
    // This tests your understanding of common deadlock pitfalls with async/await
    public class DeadlockScenarios
    {
        // Implement a method that demonstrates how deadlocks can occur when 
        // mixing synchronous and asynchronous code incorrectly
        public string PotentialDeadlockMethod()
        {
            // TODO: Implement this method to demonstrate a potential deadlock scenario
            // Note: In a real app, this would cause a deadlock, but in a test it might not
            // because of how unit test runners handle synchronization contexts
            throw new NotImplementedException();
        }

        // Implement a correct version that avoids the deadlock
        public async Task<string> CorrectAsyncMethod()
        {
            // TODO: Implement the correct async version that avoids deadlocks
            throw new NotImplementedException();
        }

        [Fact]
        public void ShouldDemonstrateDeadlockIssue()
        {
            // TODO: Write a test that conceptually demonstrates the deadlock issue
            // Note: This may not actually deadlock in the test runner
        }

        [Fact]
        public async Task ShouldAvoidDeadlockWithCorrectPattern()
        {
            // TODO: Test that CorrectAsyncMethod works without deadlock
        }
    }

    // Problem 10: HttpClient and Async I/O
    // This tests your understanding of async I/O with HttpClient
    public class HttpClientAsync
    {
        // Implement a method that uses HttpClient to make an async web request
        // It should handle the response and potential exceptions properly
        public async Task<string> FetchWebContentAsync(string url)
        {
            try {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode(); // Throw exception on error
                    string data = await response.Content.ReadAsStringAsync();
                    return data;
                }
            } catch (HttpRequestException ex) {
                return ex.Message;
            }
        }

        [Fact]
        public async Task ShouldFetchWebContentSuccessfully()
        {
            var result = await FetchWebContentAsync("https://www.example.com");
            Assert.Contains("Example Domain", result);
        }

        [Fact]
        public async Task ShouldHandleHttpErrorsGracefully()
        {
            var result = await FetchWebContentAsync("https://www.example.com/does-not-exist-404");
            Assert.Contains("404 (Not Found", result);
        }
    }
}