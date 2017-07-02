using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddedDb
{
    class Program
    {
        static System.Data.SQLite.SQLiteConnection m_dbConnection = null;
        static void Main(string[] args)
        {
            CreateSQLiteDb();
            OpenSQLiteDb();
            CreateTable();
            InsertRecordsIntoTable();
            SelectFromTable();
        }

        private static void CreateSQLiteDb()
        {
            if (System.IO.File.Exists("MyDatabase.sqlite"))
                return;

            System.Data.SQLite.SQLiteConnection.CreateFile("MyDatabase.sqlite");
            m_dbConnection = new System.Data.SQLite.SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.SetPassword("P@55w0rD");
        }

        private static void OpenSQLiteDb()
        {
            if (m_dbConnection != null && m_dbConnection.State == System.Data.ConnectionState.Open)
                return;

            m_dbConnection = new System.Data.SQLite.SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;Password=password;");
            m_dbConnection.Open();
        }

        private static void CreateTable()
        {
            if (m_dbConnection != null && m_dbConnection.State != System.Data.ConnectionState.Open)
                throw new Exception("SQLite Connection Not Open");

            if (IsExistingTable())
                return;

            string sql = "create table highscores (name varchar(20), score int)";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        private static void InsertRecordsIntoTable()
        {
            if (m_dbConnection != null && m_dbConnection.State != System.Data.ConnectionState.Open)
                throw new Exception("SQLite Connection Not Open");

            string sql = "insert into highscores (name, score) values ('Me', 3000)";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into highscores (name, score) values ('Myself', 6000)";
            command = new System.Data.SQLite.SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into highscores (name, score) values ('And I', 9001)";
            command = new System.Data.SQLite.SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        private static void SelectFromTable()
        {
            if (m_dbConnection != null && m_dbConnection.State != System.Data.ConnectionState.Open)
                throw new Exception("SQLite Connection Not Open");

            string sql = "select * from highscores order by score desc";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql, m_dbConnection);
            System.Data.SQLite.SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
        }



        private static void ChangePassword()
        {
            if (m_dbConnection != null && m_dbConnection.State != System.Data.ConnectionState.Open)
                throw new Exception("SQLite Connection Not Open");

            m_dbConnection.ChangePassword("NewP@55w0rD");
        }

        private static bool IsExistingTable()
        {
            string sql = "SELECT name FROM sqlite_master WHERE type = 'table' AND name = 'highscores'";
            System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sql, m_dbConnection);
            object obj = command.ExecuteScalar();

            return obj != null;
        }
    }
}
