using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DUserController : DalController
    {

        internal DUserController() : base ("User")
        {}

        override protected DTO ConvertReaderToObject(SQLiteDataReader reader)
        {

        }


        override public void Insert(DTO dTO)
        {

        }
    }
}
