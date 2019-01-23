using System;
using Google.Cloud.PubSub.V1;

namespace adapter
{


    class Program
    {
        private String projectId = "praxistag-229219";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var pubSubEsImporter = new PubSubElasticImporter();
            pubSubEsImporter.Run();
        }
    }
}
