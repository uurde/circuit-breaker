using System.Net;
using Polly;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Policy

        HttpStatusCode[] codesWorthRetrying =
        {
            HttpStatusCode.RequestTimeout, //408
            HttpStatusCode.InternalServerError, //500
            HttpStatusCode.BadGateway, //502
            HttpStatusCode.ServiceUnavailable, //503
            HttpStatusCode.GatewayTimeout //504
        };

        var policyBuilder = new Main.PolicyBuilder();
        var circutBreakerPolicy = policyBuilder.GetCircutBreakerPolicy();
        var retryPolicy = policyBuilder.GetRetryPolicy(codesWorthRetrying);
        var wrappedPolicy = retryPolicy.WrapAsync(circutBreakerPolicy);

        #endregion

        builder.Services.AddControllers();

        #region HttpClient

        builder.Services.AddHttpClient("providerClient", c =>
        {
            c.BaseAddress = new Uri("https://localhost:7062");
        }).AddTransientHttpErrorPolicy(p => wrappedPolicy);

        #endregion

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}