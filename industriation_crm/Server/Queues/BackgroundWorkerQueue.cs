using System.Collections.Concurrent;

namespace industriation_crm.Server.Queues
{
    public class BackgroundWorkerQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }

        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem, int? is_clone)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }


            if (is_clone == 1)
            {
                var items = _workItems.ToArray();
                _workItems.Clear();
                _workItems.Enqueue(workItem);
                foreach (var item in items)
                    _workItems.Enqueue(item);
            }
            else
                _workItems.Enqueue(workItem);
            _signal.Release();
        }
    }
}
