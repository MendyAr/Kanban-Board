using IntroSE.Kanban.Frontend.Model;
using IntroSE.Kanban.Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntroSE.Kanban.Frontend.Commands
{

    public class AddColumnCommand : ICommand
    {

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var boardViewModel = parameter as BoardViewModel;

            if (boardViewModel.NewColumnName == "")
            {
                boardViewModel.Message = "Enter a name please";
            }
            else
            {
                try
                {
                    var newColumnOrdinal = int.Parse(boardViewModel.NewColumnOrdinal);
                    boardViewModel.Board.AddColumn(newColumnOrdinal, boardViewModel.NewColumnName);
                }
                catch (Exception e)
                {
                    boardViewModel.Message = e.Message;
                }
            }
        }

    }
}
