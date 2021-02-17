using System;
using Microsoft.Data.SqlClient;

namespace AdoNet
{
    public class Program
    {
        const string SqlConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=MinionsDB;Integrated Security=true ";
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(SqlConnectionString))
            {
                connection.Open();
                //string createDatabase = "CREATE DATABASE MinionsDB";
                //var createTableStatements = GetCreateTableStatements();
                //foreach (var query in createTableStatements)
                //{
                //    NewMethod(connection, query);
                //}
                //var insertTablesStatements = InsertRecords();
                //foreach (var statemnt in insertTablesStatements)
                //{
                //    NewMethod(connection, statemnt);
                //}
                int idParam = int.Parse(Console.ReadLine());

                MinionNames(connection, idParam);
            }
        }

        private static void MinionNames(SqlConnection connection, int idParam)
        {
            var nameQuery = @"SELECT Name FROM Villains WHERE Id = @Id";
            var command = new SqlCommand(nameQuery, connection);
            command.Parameters.AddWithValue("@Id", idParam);

            var nameViliar = command.ExecuteScalar();
            Console.WriteLine($"Villain: {nameViliar}");

            if (nameViliar == null)
            {
                Console.WriteLine($"No villain with ID {idParam} exists in the database.");
            }
            else
            {
                var query2 = @"SELECT ROW_NUMBER() OVER(ORDER BY m.Name) as RowNum,
                                    m.Name, 
                                    m.Age
                                    FROM MinionsVillains AS mv
                                    JOIN Minions As m ON mv.MinionId = m.Id
                                   WHERE mv.VillainId = @Id
                                ORDER BY m.Name";
                var command2 = new SqlCommand(query2, connection);
                command2.Parameters.AddWithValue("@Id", idParam);
                SqlDataReader reader = command2.ExecuteReader();
                using (reader)
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            //  string row =(string)reader["RowNum"];
                            string name = (string)reader["Name"];
                            int age = (int)reader["Age"];
                            Console.WriteLine($"{reader[0]}. {name} {age}");
                        }
                    }
                    else
                        Console.WriteLine($"(no minions)");
                }

            }
        }

        private static void VillainNames(SqlConnection connection)
        {
            var query = @"SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount 
                            FROM Villains AS v
                            JOIN MinionsVillains AS mv ON v.Id = mv.VillainId
                            GROUP BY v.Id, v.Name
                            HAVING COUNT(mv.VillainId) > 3
                            ORDER BY COUNT(mv.VillainId)";
            var command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    var name = reader[0];
                    var count = reader[1];
                    Console.WriteLine($"{name} - {count}");
                }
            }
        }

        private static string[] InsertRecords()
        {
            var result = new string[]
            {
                "INSERT INTO Countries ([Name]) VALUES('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')",

                "INSERT INTO Towns([Name], CountryCode) VALUES('Plovdiv', 1),('Varna', 1)," +
                "('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2)," +
                "('Berlin', 3),('Frankfurt', 3),('Oslo', 4)",

                "INSERT INTO Minions(Name, Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6)," +
                "('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1)," +
                "('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)",

                "INSERT INTO EvilnessFactors(Name) VALUES('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')",

                "INSERT INTO Villains(Name, EvilnessFactorId) VALUES('Gru', 2),('Victor', 1),('Jilly', 3),('Miro', 4)," +
                    "('Rosen', 5),('Dimityr', 1),('Dobromir', 2)",

                "INSERT INTO MinionsVillains(MinionId, VillainId) " +
                "VALUES(4, 2),(1, 1),(5, 7),(3, 5),(2, 6),(11, 5),(8, 4),(9, 7),(7, 1),(1, 3),(7, 3),(5, 3)," +
                "(4, 3),(1, 2),(2, 1),(2, 7)"
            };
            return result;
    }
        private static void NewMethod(SqlConnection connection, string query)
        {
            using (var command = new SqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        private static string[] GetCreateTableStatements()
        {
            var result = new string[]
            {
                "CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))",

                "CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), " +
                "CountryCode INT FOREIGN KEY REFERENCES Countries(Id))",

                "CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, " +
                "TownId INT FOREIGN KEY REFERENCES Towns(Id))",

                "CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))",

                "CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), " +
                "EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",

                "CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id)," +
                "VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))",
            };
            return result;
        }
    }
}
