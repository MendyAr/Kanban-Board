using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STask = IntroSE.Kanban.Backend.ServiceLayer.Task;

namespace IntroSE.Kanban.Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private UserModel _user;
        private BoardModel _board;
        private ColumnModel _column;
        private int _id;
        private DateTime _creationTime;
        private string _title;
        private string _description;
        private DateTime _dueDate;
        private string _emailAssignee;

        public UserModel User { get => _user; }
        public BoardModel Board { get => _board; }
        public ColumnModel Column { get => _column; }
        public int ID { get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged("TaskID");
            }
        }
        public DateTime CreationTime { get => _creationTime;
            set
            {
                _creationTime = value;
                RaisePropertyChanged("TaskCreationTime");
            }
        }
        public string Title { get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged("TaskTitle");
            }
        }
        public string Description { get => _description;
            set
            {
                _description = value;
                RaisePropertyChanged("TaskDescription");
            }
        }
        public DateTime DueDate { get => _dueDate;
            set
            {
                _dueDate = value;
                RaisePropertyChanged("TaskDueDate");
            }
        }
        public string Assignee { get => _emailAssignee;
            set
            {
                _emailAssignee = value;
                RaisePropertyChanged("TaskAssignee");
            }
        }

        public TaskModel(ColumnModel column, STask sTask) : base(column.Controller)
        {
            this._user = column.User;
            this._board = column.Board;
            this._column = column;
            this._id = sTask.Id;
            this._creationTime = sTask.CreationTime;
            this._title = sTask.Title;
            this._description = sTask.Description;
            this._dueDate = sTask.DueDate;
            this._emailAssignee = sTask.emailAssignee;
        }
    }
}
