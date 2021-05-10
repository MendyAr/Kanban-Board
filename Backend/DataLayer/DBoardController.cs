using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DBoardController : DalController
    {

        public DBoardController() :base ("Board")
        {

        }
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            throw new NotImplementedException();
        }

        protected override bool insert(DTO dTO)
        {
            throw new NotImplementedException();
        }
    }
}
