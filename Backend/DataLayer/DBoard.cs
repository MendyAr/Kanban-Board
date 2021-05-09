using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataLayer
{
    class DBoard : DTO
    {
        private static string CreatorColumnName = "Creator";
        private static string BoardNameColumnName = "Creator";
        private static string LimitColumn1Name = "LimitColumn1";
        private static string LimitColumn2Name = "LimitColumn2";
        private static string LimitColumn3Name = "LimitColumn3";
        private readonly string _creator;
        public string Creator { get => _creator;}


        private string _boardName;
        public string BoardName { get => _boardName; set 
            {
                if (persist)
                {
                    update(BoardNameColumnName, value);
                }
                _boardName = value;
            } }


        private int _limitColumn1;
        public int LimitColumn1
        {
            get => _limitColumn1; set
            {
                if (persist)
                {
                    update(_limitColumn1, value);
                }
                _limitColumn1 = value;
            }
        }


        private int _limitColumn2;
        public int LimitColumn2
        {
            get => _limitColumn2; set
            {
                if (persist)
                {
                    update(_limitColumn2, value);
                }
                _limitColumn2 = value;
            }
        }


        private int _limitColumn3;
        public int LimitColumn3
        {
            get => _limitColumn3; set
            {
                if (persist)
                {
                    update(_limitColumn3, value);
                }
                _limitColumn3 = value;
            }
        }


        public DBoard (string creator, string boardName,int limitColumn1, int limitColumn2, int limitColumn3) : base(new DBoardController())
        {
            this._creator = creator;
            this._boardName = boardName;
            _limitColumn1 = limitColumn1;
            _limitColumn2 = limitColumn2;
            _limitColumn3 = limitColumn3;

        }
        public override void insert()
        {
            _controller.insert();
        }
    }
}
