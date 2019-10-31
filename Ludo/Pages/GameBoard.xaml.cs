using System.Windows.Controls;

namespace Ludo
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Page
    {
        public GameBoard(int playerNumber, double height)
        {
            InitializeComponent();
            DataContext = new GameBoardViewModel(playerNumber , height, PathGrid);
        }
    }
}
