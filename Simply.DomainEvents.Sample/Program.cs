using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simply.DomainEvents.Sample
{
    internal class Program
    {
        private class MessageHandler
            : IEventHandler<int>
        {
            public Task HandleAsync(int @event, CancellationToken cancellationToken)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Custom message handler was canceled");
                    return Task.FromCanceled(cancellationToken);
                }
                else
                {
                    Console.WriteLine($"Custom message handler was invoked: {@event}");
                    return Task.CompletedTask;
                }
            }
        }

        private static void Main(string[] args)
        {
            IEventBus bus = new EventBus();
            
            bus.OnEventHandlerError += OnEventHandlerError;
            bus.OnEventInterceptorError += OnEventInterceptorError;

            bus.HandleEvent<int>(e =>
            {
                Console.WriteLine($"Action handler was invoked: {e}");
            });

            bus.HandleEvent<int>((e, ct) =>
            {
                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine($"Async handler was canceled");
                    return Task.FromCanceled(ct);
                }
                else
                {
                    Console.WriteLine($"Async handler was invoked: {e}");
                    return Task.CompletedTask;
                }
            });

            bus.AddEventHandler(new MessageHandler());

            bus.Raise(42);

            Console.ReadKey();
        }

        private static void OnEventHandlerError(object sender, ErrorEventArgs args)
        {
            Console.WriteLine($"Event handler error occurred: {args.Error}");
        }

        private static void OnEventInterceptorError(object sender, ErrorEventArgs args)
        {
            Console.WriteLine($"Event interceptor error occurred: {args.Error}");
        }
    }
}