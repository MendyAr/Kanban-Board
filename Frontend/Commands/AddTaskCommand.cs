using IntroSE.Kanban.Frontend.ViewModel;
using System;
using System.Windows.Input;

namespace IntroSE.Kanban.Frontend.Commands
{
    public class AddTaskCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var addingTaskViewModel = parameter as AddingTaskViewModel;
            try
            {
                addingTaskViewModel.Board.AddTask(addingTaskViewModel.Title, addingTaskViewModel.Description, addingTaskViewModel.DueDate);
                addingTaskViewModel.Message = $"Task '{addingTaskViewModel.Title}' has added successfully!";
            }
            catch (Exception e)
            {
                addingTaskViewModel.Message = e.Message;
            }
        }
    }
}