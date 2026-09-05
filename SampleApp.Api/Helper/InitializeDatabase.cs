using Microsoft.Data.Sqlite;
 
using System.Reflection;

namespace SampleApp.Api.Helper
{
    public class InitializeDatabase
    {
        public static void Initialize()
        {
            /*var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string dbpath = Path.Combine(currentPath, "employee.db");
            var dbFileInfo = new FileInfo(dbpath);
            if (!dbFileInfo.Exists)
            {               
                using (var connection = new SqliteConnection("Data Source=employee.db"))
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = @"CREATE TABLE Employee (
                                            EmployeeID [int] IDENTITY(100,1) NOT NULL,
                                            FirstName varchar(255),
                                            LastName varchar(255),
                                            Email varchar(255),
                                            AddressLine varchar(255),
                                            City varchar(255)
                                            ); ";
                    command.ExecuteNonQuery();
                }
            }*/
        }
    }
}