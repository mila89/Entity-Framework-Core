using System;
using Microsoft.Data.SqlClient;

namespace Add_Minion
{
    public class Program
    {
        const string SqlConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MinionsDB;Integrated Security=true ";
        public static void Main()
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                string[] input = Console.ReadLine().Split(" ");
                string minionName = input[1];
                int age = int.Parse(input[2]);
                string townName = input[3];
                var queryTown = @"SELECT Id FROM Towns WHERE Name = @Name";
                var commandTown =new SqlCommand(queryTown, connection);
                commandTown.Parameters.AddWithValue("@Name", townName);
                var idTown = commandTown.ExecuteScalar();
                if (idTown is null)
                {
                    var insertTownQuery = @"INSERT INTO Towns (Name) VALUES (@townName)";
                    commandTown = new SqlCommand(insertTownQuery, connection);
                    commandTown.Parameters.AddWithValue("@townName", townName);
                    int result=commandTown.ExecuteNonQuery();
                    Console.WriteLine(result);
                }
            }
        }
    }
}
