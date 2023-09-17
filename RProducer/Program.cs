using RabbitMQ.Client;
using System;
using System.Text;
using RabbitMQ.Client.Events;

namespace RProducer
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            int count = 0;

            //while (true)
            {
                channel.QueueDeclare(queue: "CiceroTesteFila",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

                string message = $"{count++} Mensagem de teste para fila do Nova!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: string.Empty,
                                     routingKey: "CiceroTesteFila",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($" [x] Sent {message}");
                System.Threading.Thread.Sleep(200);
            }
        }
    }
}
