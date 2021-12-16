using Azure.Messaging.ServiceBus;
using System;
using System.Threading.Tasks;

namespace Send
{
    public class Program
    {
        private const string ConnectionString = "";
        private const string QueueName = "";
        private const int NumberOfMessages = 100;

        public static async Task Main()
        {
            ServiceBusClient serviceBusClient = new ServiceBusClient(ConnectionString);
            ServiceBusSender sender = serviceBusClient.CreateSender(QueueName);

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
            for (int i = 1; i <= NumberOfMessages; i++)
            {
                ServiceBusMessage createdMessage = new ServiceBusMessage("body");
                createdMessage.ApplicationProperties.Add("Property1", "");
                createdMessage.ApplicationProperties.Add("Property2", "");

                if (!messageBatch.TryAddMessage(createdMessage))
                {
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }

            try
            {
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {NumberOfMessages} messages has been published to the queue {QueueName}.");
            }
            finally
            {
                await sender.DisposeAsync();
                await serviceBusClient.DisposeAsync();
            }

            Console.WriteLine("Press any key to end the application");
            Console.ReadKey();
        }
    }
}
