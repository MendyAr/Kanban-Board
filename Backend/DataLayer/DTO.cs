using System;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal abstract class DTO
    {
        protected string _id;
        protected readonly string _connectionString;
        protected readonly string _tableName;

        public string ID { get => _id; set {
                if (Persist)
                {
                  Update("ID", value);
                }
                _id = value;
            } }
        public bool Persist { get; set; }        
       
        protected DTO(string id, string tableName)
        {
            Persist= false;
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanbas.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
            ID = id;
        }

        public bool Insert()
        {
            int result = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = InsertCommand(connection);
                try             
                {
                    connection.Open();
                    result = command.ExecuteNonQuery();
                }
                catch
                {
                    //log
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return result > 0;
        }

        protected bool Update(string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection, 
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id=@{_id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    command.Parameters.Add(new SQLiteParameter(_id, _id));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    //log
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        protected bool Update(string attributeName, long attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={_id}"
                };

                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    // log
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }
            }
            return res > 0;
        }

        protected bool Update(string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where id={_id}"
                };

                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    // log
                }
                finally
                {
                    command.Dispose();
                    connection.Close();

                }
            }
            return res > 0;
        }



        protected abstract SQLiteCommand InsertCommand(SQLiteConnection connection);

    }
}
