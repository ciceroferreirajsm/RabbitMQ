using RabbitMQ.Client;
using System;
using System.Text;
using RabbitMQ.Client.Events;

namespace RConsumer
{
    class Program
    {
        static void Main(string[] args)
        {

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "CiceroTesteFila",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            Console.WriteLine(" [*] Waiting for messages.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");
                    //Caso a regra de negócio funcione executar um Ack para confirmar sucesso da mensagem
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    //Caso de erro na regra de negócio da um Nack para voltar a mensagem para fila e nao perder nos threads
                    channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };
            channel.BasicConsume(queue: "CiceroTesteFila",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
