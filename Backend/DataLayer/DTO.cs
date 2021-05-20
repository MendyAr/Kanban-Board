using System;
using System.Data.SQLite;
using System.IO;
using log4net;
using log4net.Config;
using System.Reflection;

namespace IntroSE.Kanban.Backend.DataLayer
{
    internal abstract class DTO
    {

        // properties

        private string _id;
        protected const string COL_ID = "ID";
        protected readonly string _connectionString;
        protected readonly string _tableName;
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal string Id
        {
            get => _id; set
            {
                if (Persist)
                {
                  Update(COL_ID, value);
                }
                _id = value;
            }
        }

        internal bool Persist { get; set; }


        // constructor

        internal DTO(string id, string tableName)
        {
            Persist= false;
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
            Id = id;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }


        // methods

        internal bool Insert()
        {
            bool duplicate = false;
            int result = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = InsertCommand(connection);
                try             
                {
                    connection.Open();
                    result = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Fatal($"Insert on table {_tableName} failed, tried command: {command.CommandText},\n" +
                        $" the SQLite exception massage was: {e.Message}");
                    duplicate = true;
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                    if (duplicate)
                        throw new InvalidOperationException();
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
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where {COL_ID}=@{COL_ID}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    command.Parameters.Add(new SQLiteParameter(COL_ID, Id));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Fatal($"Update on table {_tableName} failed, tried command: {command.CommandText},\n" +
                        $" the SQLite exception massage was: {e.Message}");
                    
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
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where {COL_ID} = @{COL_ID}"
                };

                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    command.Parameters.Add(new SQLiteParameter(COL_ID, Id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Insert on table {_tableName} failed, tried command: {command.CommandText},\n" +
                        $" the SQLite exception massage was: {e.Message}");
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
                    CommandText = $"update {_tableName} set [{attributeName}]=@{attributeName} where {COL_ID} =@ {COL_ID}"
                };

                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    command.Parameters.Add(new SQLiteParameter(COL_ID,Id));
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    log.Error($"Insert on table {_tableName} failed, tried command: {command.CommandText},\n" +
                        $" the SQLite exception massage was: {e.Message}");
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
