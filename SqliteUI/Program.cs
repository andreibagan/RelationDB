using DataAccessLibrary;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace SqliteUI
{
    class Program
    {
        static void Main(string[] args)
        {
            //SqlCrud sql = new SqlCrud(GetConnectionString());

            //ReadAllContacts(sql);

            //ReadContact(sql);

            //CreateNewContact(sql);

            //UpdateContact(sql);

            //RemovePhoneNumberFromContact(sql, 1, 2);

            Console.ReadLine();
        }

        private static string GetConnectionString(string connectionStringName = "Default")
        {
            string output = string.Empty;

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            output = config.GetConnectionString(connectionStringName);

            return output;
        }
    }
}
