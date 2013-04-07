using System;
using System.Data.SqlServerCe;
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
            if (File.Exists(DbFileName))
                File.Delete(DbFileName);

            string connStr = new SqlCeConnectionStringBuilder { DataSource = DbFileName, FileMode = "Shared Read" }.ConnectionString;
            using (var engine = new SqlCeEngine(connStr))
                engine.CreateDatabase();

            using (var conn = new SqlCeConnection(connStr)) {
                conn.Open();
                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = "CREATE TABLE Users (Id INT IDENTITY PRIMARY KEY, Name NVARCHAR(255), Avatar NVARCHAR(255))";
                    cmd.ExecuteNonQuery();
                }
            }

            var db = Database.OpenConnection(connStr);
            db.Users.Insert(Name: "Athari", Avatar: "athari.jpg");
            Console.WriteLine(db.Users.FindByName("Athari").Avatar);
        }
    }
}