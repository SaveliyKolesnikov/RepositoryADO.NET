using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;

namespace RepositoryADO.NET
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Repository!");
            Console.ForegroundColor = ConsoleColor.White;

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();

            var conStr = config["connectionString"];

            List<Record> records;
            using (var repo = new Repository(conStr))
            {
                records = UpdateData(repo);
                PrintRecords();
                Console.WriteLine("Create a record");
                repo.CreateRecord(new Record
                (
                    id: 0,
                    author: "Ronald",
                    text: "Bye!",
                    recoredDate: DateTime.Now
                )).GetAwaiter().GetResult();

                records = UpdateData(repo);
                PrintRecords();

                Console.WriteLine("Delete a record");
                repo.DeleteRecord(records.FirstOrDefault()).GetAwaiter().GetResult();
                records = UpdateData(repo);

                PrintRecords();

                var lastRecord = records.LastOrDefault();
                if (!(lastRecord is null))
                    lastRecord.Text = "I'm a SQL!!!";

                Console.WriteLine("Update a record");
                repo.UpdateRecord(lastRecord).GetAwaiter().GetResult();
                records = UpdateData(repo);
                PrintRecords();
            }

            void PrintRecords()
            {
                if (records is null) return;

                foreach (var record in records)
                    Console.WriteLine(record);

                Console.WriteLine(new String('-', 50));
            }
        }

        static List<Record> UpdateData(Repository repository) =>
            repository.GetRecords().GetAwaiter().GetResult();
    }


}
