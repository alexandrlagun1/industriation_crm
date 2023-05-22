namespace industriation_crm.Server.Queues
{
    public class RemoveProductService : BackgroundService
    {
        private readonly BackgroundRemoveProductQueue queue;
        public RemoveProductService(BackgroundRemoveProductQueue queue)
        {
            this.queue = queue;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await queue.DequeueAsync(stoppingToken);

                await workItem(stoppingToken);
            }
        }
    }
}
