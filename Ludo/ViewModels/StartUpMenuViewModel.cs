using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ludo
{
    public class StartUpMenuViewModel : BaseViewModel
    {
        public StartUpMenuViewModel()
        {

        StartGameCommand = new RelayParameterizedCommand((parameter) => StartGame(parameter));
        }


        public ICommand StartGameCommand { get; set; }

        private void StartGame(object sender)
        {
            var button = sender as Button;
            (Application.Current.MainWindow.DataContext as WindowViewModel).CurrentPage = new GameBoard(Convert.ToInt32((sender as Button).Content), 800);
        }
    }
}
