using Apache.NMS;
using System;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text)) return;
                SendNewMessage(text);
            }
        }

        private static void SendNewMessage(string text)
        {
            string topic = "TextQueue";

            Console.WriteLine($"Adding message to queue topic: {topic}");

            string brokerUri = $"activemq:tcp://localhost:61616";  // Default port
            NMSConnectionFactory factory = new NMSConnectionFactory(brokerUri);

            using (IConnection connection = factory.CreateConnection())
            {
                connection.Start();

                using (ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge))
                using (IDestination dest = session.GetTopic(topic))
                using (IMessageProducer producer = session.CreateProducer(dest))
                {
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;

                    producer.Send(session.CreateTextMessage(text));
                    Console.WriteLine($"Sent {text} messages");
                }
            }
        }
    }
}
