using HamroFashion.Api.V1.Utils;
using System.Collections.Concurrent;
using System.Net.Mail;

namespace HamroFashion.Api.V1.Services
{
    /// <summary>
    /// Runs like a scheduled task and sends out our site emails
    /// </summary>
    public class MailerBackgroundService : BackgroundService
    {
        /// <summary>
        /// The queue of MailMessages to process (might be empty)
        /// </summary>
        private static ConcurrentQueue<MailMessage> Queue = new ConcurrentQueue<MailMessage>();

        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(0.17);

        /// <summary>
        /// Anything can call this to push a new message into the Queue
        /// </summary>
        /// <param name="to">Email address to send to</param>
        /// <param name="subject">Subject of the message</param>
        /// <param name="body">Body of the message</param>
        public static void QueueMessage(string to, string subject, string body)
        {
            Queue.Enqueue(Mailer.BuildMessage(to, subject, body));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessEmailQueueAsync(stoppingToken);
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task ProcessEmailQueueAsync(CancellationToken stoppingToken)
        {
            var cancellationToken = stoppingToken;
            var mailer = new Mailer();
            var tasks = new List<Task>();
            var limiter = 1;

            while (limiter < 10 && Queue.TryDequeue(out var msg))
            {
                tasks.Add(mailer.SendAsync(msg, cancellationToken));
                limiter++;
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
