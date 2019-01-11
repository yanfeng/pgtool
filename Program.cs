using System;
using System.Threading;
using Marten;

namespace pgtool
{
    class Program
    {
        private const string ConnectionString = "host=localhost;port=5432;database=minifarm;password=123456;username=postgres;pooling=false;";

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: dotnet run arg1 arg2");
                Console.WriteLine("  arg1: total number of connections being opened, e.g. 100");
                Console.WriteLine("  arg2: database connection string, such as \"host=localhost;port=5432;database=minifarm;password=123456;username=postgres;pooling=false;\"");
                return;
            }

            // arg 1: total number of connections being opened
            var total = 1;
            if (args.Length >= 1)
            {
                int.TryParse(args[0], out total);
            }

            // arg 2: database connection string
            var connectionString = ConnectionString;
            if (args.Length == 2)
            {
                connectionString = args[1];
            }

            var store = DocumentStore.For(connectionString);

            for (int i = 0; i < total; i++)
            {
                using (var session = store.QuerySession())
                {
                    var result = session.Query<string>($"select '{i + 1}'");
                    Thread.Sleep(100);
                    Console.WriteLine(result[0]);
                }
            }
        }
    }
}
