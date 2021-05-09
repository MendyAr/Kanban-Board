using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class BoardMemberController 

    {
        private readonly string _connectionString;
        private static readonly string _tableName = "BoardMember";

        public BoardMemberController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "database.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            
        }
            
        public Dictionary<string, HashSet<string>> Select()  // email, set of boards with syntax creatorEmail:boardName
        {
            Dictionary<string, HashSet<string>> results = new Dictionary<string, HashSet<string>>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        string assign = dataReader.GetString(2);
                        string board = dataReader.GetString(0) + ":" + dataReader.GetString(1);
                        if (results.ContainsKey(assign))
                        {
                            results[assign].Add(board);
                        }
                        else
                        {
                            results[assign] = new HashSet<string>();
                            results[assign].Add(board);
                        }

                    }
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        public void insert()
        {

        }

        public void delete()
        {

        }
    }
}
