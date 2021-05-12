using System.Collections.Generic;
using BBoard = IntroSE.Kanban.Backend.BuisnessLayer.Board;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly string CreatorEmail;
        public readonly string Name;
        public readonly IList<string> Columns;
        public readonly int TaskCounter;

        internal Board(string creatorEmail, string boardName, IList<string> columnNames, int taskCounter)
        {
            CreatorEmail = creatorEmail;
            Name = boardName;
            Columns = new List<string>(columnNames);
            TaskCounter = taskCounter;
        }

        internal Board(string creatorEmail, string boardName, BBoard bBoard)
        {
            CreatorEmail = creatorEmail;
            Name = boardName;
            Columns = bBoard.Columns;
            TaskCounter = bBoard.TaskIdCounter;
        }
    }
}
