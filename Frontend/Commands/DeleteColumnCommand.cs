using IntroSE.Kanban.Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IntroSE.Kanban.Frontend.Commands
{
    public class DeleteColumnCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var boardViewModel = parameter as BoardViewModel;
            try
            {
                boardViewModel.Board.DeleteColumn(boardViewModel.SelectedColumn.Ordinal);
                boardViewModel.Message = "Column deleted successfully!";
            }
            catch (Exception e)
            {
                boardViewModel.Message = e.Message;
            }
        }
    }
}
