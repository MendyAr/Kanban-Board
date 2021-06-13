using System.Collections.Generic;
using BBoard = IntroSE.Kanban.Backend.BusinessLayer.Board;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public struct Board
    {
        public readonly string CreatorEmail;
        public readonly string Name;
        public readonly int ColumnCount;
        public readonly int TaskCount;

        internal Board(string creatorEmail, string boardName, int columnCount, int taskCount)
        {
            CreatorEmail = creatorEmail;
            Name = boardName;
            ColumnCount = columnCount;
            TaskCount = taskCount;
        }

        internal Board(string creatorEmail, string boardName, BBoard bBoard)
        {
            CreatorEmail = creatorEmail;
            Name = boardName;
            ColumnCount = bBoard.ColumnCounter;
            TaskCount = bBoard.TaskIdCounter;
        }
    }
}
