using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using JsonClass;
using MySql.Data.MySqlClient;   

namespace SQLConnector
{
    public class SQLConnector
    {
        private string _ConnectionString;
        public string ConnectionString { get { return _ConnectionString; } }

        MySqlConnection connection;
        public SQLConnector() { }

        public SQLConnector( string server, string port, string userName, string password, string dataBase)
        {
            _ConnectionString = "Server=" + server + ";Port=" + port + ";Database=" +dataBase + ";Uid=" + userName + ";Pwd=" + password + ";";
        }
        public void SQLBootUp()
        {  
            if(ConnectionString == null)
            {
                var json = new JsonClass.Json();
                var server = json.ReturnValue("Server", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
                var port = json.ReturnValue("Port", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
                var dataBase = json.ReturnValue("Database", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
                var userName = json.ReturnValue("User", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
                var password = json.ReturnValue("Password", Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "json1.json"));
                _ConnectionString = "Server=" + server + ";Port=" + port + ";Database=" + dataBase + ";Uid=" + userName + ";Pwd=" + password + ";";
            }

            connection = new MySqlConnection(ConnectionString);
            try
            {
                connection.Open();
            }
            catch (MySqlException ex) { Console.WriteLine("error: " + ex.Message);
                    }
        }

        public void AddDiscordUser ( string Username)
        {
            string sqlQuery = "SHOW TABLES;";
            MySqlCommand cmd = new MySqlCommand(sqlQuery, connection);
            int x = 0;
            using(MySqlDataReader reader = cmd.ExecuteReader())
                while(reader.Read())
                {
                    Console.WriteLine(reader.GetString(x));
                    x++;
                }
        }

        public bool CheckIfUserIsInDataBase(string userName)
        {
            return false;
        }
    }
}

