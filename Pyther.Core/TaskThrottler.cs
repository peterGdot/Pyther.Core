namespace Pyther.Core;

public class TaskThrottler
{
    private readonly SemaphoreSlim semaphore;

    public TaskThrottler(int maxParallelTasks)
    {
        semaphore = new SemaphoreSlim(maxParallelTasks);
    }

    public async Task StartAsync(Func<int, int, Task> method, int count)
    {
        var tasks = Enumerable.Range(1, count).Select(i => Run(i, method, count));
        await Task.WhenAll(tasks);
    }

    private async Task Run(int taskId, Func<int, int, Task> method, int count)
    {
        // wait if the semaphore is "full"
        await semaphore.WaitAsync();
        try
        {
            // execute callback
            await method.Invoke(taskId, count);
        }
        finally
        {
            semaphore.Release();
        }
    }
}
