using System;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using Simple.Data;

namespace ExpSimpleData
{
    internal class Program
    {
        private const string DbFileName = "ExpSimpleData.sdf";

        private static void Main (string[] args)
        {
            try {
                MainInternal();
                Console.WriteLine("Done");
            }
            catch (Exception e) {
                Console.WriteLine();
                Console.WriteLine(e);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }

        private static void MainInternal ()
        {
            string connStr = new SqlCeConnectionStringBuilder { DataSource = DbFileName, FileMode = "Shared Read" }.ConnectionString;
            Connect(connStr);

            //Trace.Listeners.Add(new ConsoleTraceListener());
            SimpleDataTraceSources.TraceSource.Listeners.Add(new ConsoleTraceListener());

            var db = Database.OpenConnection(connStr);
            db.Users.Insert(Name: "Athari", Avatar: "athari.jpg");
            Console.WriteLine(db.Users.FindByName("Athari").Avatar);

            db.IDoNotExist.Insert(Id: 0);
        }

        private static void Connect (string connStr)
        {
            if (File.Exists(DbFileName))
                File.Delete(DbFileName);

            using (var engine = new SqlCeEngine(connStr))
                engine.CreateDatabase();

            using (var conn = new SqlCeConnection(connStr)) {
                conn.Open();
                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = "CREATE TABLE Users (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(255), Avatar NVARCHAR(255))";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}