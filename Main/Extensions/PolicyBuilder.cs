using System.Net;
using Polly;
using Polly.Extensions.Http;

namespace Main
{
    public class PolicyBuilder
    {
        public void OnHalfOpen()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Circut in test mode, one request will be allowed");
            Console.WriteLine("\n--------------------");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void OnReset()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Circut closed, requests flow normally");
            Console.WriteLine("\n--------------------");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void OnBreak(DelegateResult<HttpResponseMessage> result, TimeSpan durationOfBreak)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Provider is Unavalible, please try later on");
            Console.WriteLine($"Duration of break is: {durationOfBreak}");
            Console.WriteLine("Circut cut, requests will not flow");
            Console.WriteLine("\n--------------------");
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public IAsyncPolicy<HttpResponseMessage> GetCircutBreakerPolicy()
        {
            var policy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(4, TimeSpan.FromSeconds(10), onReset: OnReset, onBreak: OnBreak, onHalfOpen: OnHalfOpen);
            return policy;
        }

        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(HttpStatusCode[] httpStatusCodes)
        {
            var policy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => httpStatusCodes.Contains(r.StatusCode))
                .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(10), (exception, timespan, retryAttempt, context) =>
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Retry #{retryAttempt} due to {exception.Exception.Message}. Waiting for {timespan.TotalSeconds} seconds.");
            Console.BackgroundColor = ConsoleColor.Black;
                });
            return policy;
        }
    }
}

