namespace industriation_crm.Server.Queues
{
    public class PriceService : BackgroundService
    {
        private readonly BackgroundPriceQueue queue;
        public PriceService(BackgroundPriceQueue queue)
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
