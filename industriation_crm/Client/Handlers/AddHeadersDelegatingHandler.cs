namespace industriation_crm.Client.Handlers
{
    public class AddHeadersDelegatingHandler : DelegatingHandler
    {
        public AddHeadersDelegatingHandler() : base(new HttpClientHandler())
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-API-KEY", "pgH7QzFHJx4w46fI~5Uzi4RvtTwlEXp");  

            return base.SendAsync(request, cancellationToken);
        }
    }
}
