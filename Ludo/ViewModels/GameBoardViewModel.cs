using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ludo
{
    [AddINotifyPropertyChangedInterface]

    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public class GameBoardViewModel : BaseViewModel
    {
        #region Problems To Correct

        //game wining mechanism incomplete
        //red home column enter problem
        //double piece zindex problem
        //piece win message not working
        //piece position after reopening the beaten piece not correct(cause start index & current index are referencing same address)
        //if the player has beaten a piece only then he should be allowed to enter the home column

        #endregion

        #region Default Constructor

        public GameBoardViewModel(int playerNumber, double height, Grid grid)
        {
            //set the grids width and height
            grid.Height = height;
            grid.Width = height;

            GameBoardHeight = height;

            this.grid = grid;

            //set the number of players
            switch (playerNumber)
            {
                case 2:
                    NumberOfPlayer = PlayerType.Blue;
                    break;
                case 3:
                    NumberOfPlayer = PlayerType.Green;
                    break;
                case 4:
                    NumberOfPlayer = PlayerType.Yellow;
                    break;
                default:
                    break;
            }

            
            //initialize the position , HomePosition and safe position lists
            SetPositionList();
            SetHomePosition();
            SetSafePosition();

            //initialize the Players
            RedPlayer = new Player(1, PlayerType.Red);
            BluePlayer = new Player(20, PlayerType.Blue);
            GreenPlayer = new Player(39, PlayerType.Green);
            YellowPlayer = new Player(58, PlayerType.Yellow);

            //initialize the player list
            Players = new List<Player>();

            //add the players
            Players.Add(RedPlayer);
            Players.Add(BluePlayer);
            Players.Add(GreenPlayer);
            Players.Add(YellowPlayer);

            //initialize the commands
            PieceCommand = new RelayParameterizedCommand((parameter) => Piece_Click(parameter));
            RollDiceCommand = new RelayCommand(RollDice);
            MoveNextCommand = new RelayCommand(MoveNextCommandMethod);
            RestartCommand = new RelayCommand(Restart);

            int index = 0;
            //add all pieces in game board
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    var Piece = Players[j].Pieces[i];
                    grid.Children.Add(Piece);
                    Piece.HomeIndex = index;
                    var NewPosition = _HomePosition[index++];
                    Grid.SetColumn(Piece, NewPosition.Column);
                    Grid.SetRow(Piece, NewPosition.Row);
                }
            }


            //initialize the random variable
            rollDice = new Random();

            Dice = new ObservableCollection<int>();

            //set the first player turn
            PlayerTurn = ActivePlayerType.ToString();
        }

        #endregion

        #region Private Members

        /// <summary>
        /// List for the game path
        /// </summary>
        private List<Position> _Positions;

        /// <summary>
        /// The initial index of the pieces when they are closed
        /// </summary>
        private List<Position> _HomePosition;

        /// <summary>
        /// All safe positions where a piece can't be beaten
        /// </summary>
        private List<Position> _SafePositions;

        /// <summary>
        /// RedPlayer
        /// </summary>
        private Player RedPlayer { get; set; }

        /// <summary>
        /// Blue Player
        /// </summary>
        private Player BluePlayer { get; set; }

        /// <summary>
        /// Yellow Player
        /// </summary>
        private Player YellowPlayer { get; set; }

        /// <summary>
        /// Green Player
        /// </summary>
        private Player GreenPlayer { get; set; }

        /// <summary>
        /// List of the players
        /// </summary>
        private List<Player> Players { get; set; }

        /// <summary>
        /// Current active player of the game
        /// </summary>
        private PlayerType ActivePlayerType { get; set; } = PlayerType.Red;

        /// <summary>
        /// random variable to generate the diced values
        /// </summary>
        private Random rollDice;

        /// <summary>
        /// counter to observe rolls
        /// </summary>
        private int rollCount = 0;

        /// <summary>
        /// to store the current rolled value of dice
        /// </summary>
        private int value = 0;

        /// <summary>
        /// Numbers of players playing
        /// </summary>
        private PlayerType NumberOfPlayer { get; set; }

        /// <summary>
        /// tells if the player can roll the dice
        /// </summary>
        private bool CanRollDice { get; set; }

        /// <summary>
        /// Initial Panel index piece
        /// </summary>
        private int panelIndex { get; set; } = 1;

        /// <summary>
        /// Winner number
        /// </summary>
        private int WinnerNumber { get; set; } = 1;
        #endregion

        #region Private Methods

        private void SetPositionList()
        {
            _Positions = new List<Position>();
            _Positions.Add(new Position() { Row = 6, Column = 0 });
            _Positions.Add(new Position() { Row = 6, Column = 1 });
            _Positions.Add(new Position() { Row = 6, Column = 2 });
            _Positions.Add(new Position() { Row = 6, Column = 3 });
            _Positions.Add(new Position() { Row = 6, Column = 4 });
            _Positions.Add(new Position() { Row = 6, Column = 5 });
            _Positions.Add(new Position() { Row = 5, Column = 6 });
            _Positions.Add(new Position() { Row = 4, Column = 6 });
            _Positions.Add(new Position() { Row = 3, Column = 6 });
            _Positions.Add(new Position() { Row = 2, Column = 6 });
            _Positions.Add(new Position() { Row = 1, Column = 6 });
            _Positions.Add(new Position() { Row = 0, Column = 6 });
            _Positions.Add(new Position() { Row = 0, Column = 7 });
            _Positions.Add(new Position() { Row = 1, Column = 7 });
            _Positions.Add(new Position() { Row = 2, Column = 7 });
            _Positions.Add(new Position() { Row = 3, Column = 7 });
            _Positions.Add(new Position() { Row = 4, Column = 7 });
            _Positions.Add(new Position() { Row = 5, Column = 7 });
            _Positions.Add(new Position() { Row = 6, Column = 7 });
            _Positions.Add(new Position() { Row = 0, Column = 8 });
            _Positions.Add(new Position() { Row = 1, Column = 8 });
            _Positions.Add(new Position() { Row = 2, Column = 8 });
            _Positions.Add(new Position() { Row = 3, Column = 8 });
            _Positions.Add(new Position() { Row = 4, Column = 8 });
            _Positions.Add(new Position() { Row = 5, Column = 8 });
            _Positions.Add(new Position() { Row = 6, Column = 9 });
            _Positions.Add(new Position() { Row = 6, Column = 10 });
            _Positions.Add(new Position() { Row = 6, Column = 11 });
            _Positions.Add(new Position() { Row = 6, Column = 12 });
            _Positions.Add(new Position() { Row = 6, Column = 13 });
            _Positions.Add(new Position() { Row = 6, Column = 14 });
            _Positions.Add(new Position() { Row = 7, Column = 14 });
            _Positions.Add(new Position() { Row = 7, Column = 13 });
            _Positions.Add(new Position() { Row = 7, Column = 12 });
            _Positions.Add(new Position() { Row = 7, Column = 11 });
            _Positions.Add(new Position() { Row = 7, Column = 10 });
            _Positions.Add(new Position() { Row = 7, Column = 9 });
            _Positions.Add(new Position() { Row = 7, Column = 8 });
            _Positions.Add(new Position() { Row = 8, Column = 14 });
            _Positions.Add(new Position() { Row = 8, Column = 13 });
            _Positions.Add(new Position() { Row = 8, Column = 12 });
            _Positions.Add(new Position() { Row = 8, Column = 11 });
            _Positions.Add(new Position() { Row = 8, Column = 10 });
            _Positions.Add(new Position() { Row = 8, Column = 9 });
            _Positions.Add(new Position() { Row = 9, Column = 8 });
            _Positions.Add(new Position() { Row = 10, Column = 8 });
            _Positions.Add(new Position() { Row = 11, Column = 8 });
            _Positions.Add(new Position() { Row = 12, Column = 8 });
            _Positions.Add(new Position() { Row = 13, Column = 8 });
            _Positions.Add(new Position() { Row = 14, Column = 8 });
            _Positions.Add(new Position() { Row = 14, Column = 7 });
            _Positions.Add(new Position() { Row = 13, Column = 7 });
            _Positions.Add(new Position() { Row = 12, Column = 7 });
            _Positions.Add(new Position() { Row = 11, Column = 7 });
            _Positions.Add(new Position() { Row = 10, Column = 7 });
            _Positions.Add(new Position() { Row = 9, Column = 7 });
            _Positions.Add(new Position() { Row = 8, Column = 7 });
            _Positions.Add(new Position() { Row = 14, Column = 6 });
            _Positions.Add(new Position() { Row = 13, Column = 6 });
            _Positions.Add(new Position() { Row = 12, Column = 6 });
            _Positions.Add(new Position() { Row = 11, Column = 6 });
            _Positions.Add(new Position() { Row = 10, Column = 6 });
            _Positions.Add(new Position() { Row = 9, Column = 6 });
            _Positions.Add(new Position() { Row = 8, Column = 5 });
            _Positions.Add(new Position() { Row = 8, Column = 4 });
            _Positions.Add(new Position() { Row = 8, Column = 3 });
            _Positions.Add(new Position() { Row = 8, Column = 2 });
            _Positions.Add(new Position() { Row = 8, Column = 1 });
            _Positions.Add(new Position() { Row = 8, Column = 0 });
            _Positions.Add(new Position() { Row = 7, Column = 0 });
            _Positions.Add(new Position() { Row = 7, Column = 1 });
            _Positions.Add(new Position() { Row = 7, Column = 2 });
            _Positions.Add(new Position() { Row = 7, Column = 3 });
            _Positions.Add(new Position() { Row = 7, Column = 4 });
            _Positions.Add(new Position() { Row = 7, Column = 5 });
            _Positions.Add(new Position() { Row = 7, Column = 6 });
        }

        private void SetHomePosition()
        {
            _HomePosition = new List<Position>();
            _HomePosition.Add(new Position() { Column = 2, Row = 2 });
            _HomePosition.Add(new Position() { Column = 2, Row = 3 });
            _HomePosition.Add(new Position() { Column = 3, Row = 2 });
            _HomePosition.Add(new Position() { Column = 3, Row = 3 });
            _HomePosition.Add(new Position() { Column = 12, Row = 2 });
            _HomePosition.Add(new Position() { Column = 12, Row = 3 });
            _HomePosition.Add(new Position() { Column = 11, Row = 2 });
            _HomePosition.Add(new Position() { Column = 11, Row = 3 });
            _HomePosition.Add(new Position() { Column = 12, Row = 12 });
            _HomePosition.Add(new Position() { Column = 12, Row = 11 });
            _HomePosition.Add(new Position() { Column = 11, Row = 12 });
            _HomePosition.Add(new Position() { Column = 11, Row = 11 });
            _HomePosition.Add(new Position() { Column = 2, Row = 12 });
            _HomePosition.Add(new Position() { Column = 2, Row = 11 });
            _HomePosition.Add(new Position() { Column = 3, Row = 12 });
            _HomePosition.Add(new Position() { Column = 3, Row = 11 });

        }

        private void SetSafePosition()
        {
            _SafePositions = new List<Position>();
            _SafePositions.Add(new Position() { Column = 1, Row = 6 });
            _SafePositions.Add(new Position() { Column = 8, Row = 1 });
            _SafePositions.Add(new Position() { Column = 13, Row = 8 });
            _SafePositions.Add(new Position() { Column = 6, Row = 13 });
        }

        private Grid grid;

        private double GameBoardHeight { get; set; }

        /// <summary>
        /// To Move the piece
        /// </summary>
        /// <param name="sender"></param>
        public async void MovePiece(Piece piece, Button btn, int np, int pi)
        {
            bool IsBeatPiece = false;
            {
                if (rollCount > 0 && Dice.Count != 0)
                {
                    #region To handle beating of the piece

                    if (np != -1)
                    {
                        //index of the current piece in the grid
                        int row = _Positions[piece.CurrentPositionIndex].Row;
                        int column = _Positions[piece.CurrentPositionIndex].Column;

                        //to get a list containing all pieces in a cell of a grid by index
                        var list = grid.Children
                                   .Cast<Piece>()
                                   .Where(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column)
                                   .Select(e => e).ToList();

                        //if there is any piece at current position beat it
                        if (list.Count == 1)
                        {
                            //if the current posiion is a safe position don't beat
                            bool IsPositionSafe = false;
                            foreach (Position item in _SafePositions)
                            {
                                if (item.Column == (_Positions[piece.CurrentPositionIndex]).Column && item.Row == (_Positions[piece.CurrentPositionIndex]).Row)
                                {
                                    IsPositionSafe = true;
                                    break;
                                }
                            }
                            if (!IsPositionSafe)
                            {
                                if (list[0].Type != piece.Type)
                                {
                                    //close the beaten piece and move to home
                                    var NewPosition = _HomePosition[list[0].HomeIndex];
                                    Grid.SetColumn(list[0], NewPosition.Column);
                                    Grid.SetRow(list[0], NewPosition.Row);
                                    list[0].IsOpen = false;
                                    //Panel.SetZIndex(piece, 1);
                                    //to give player a extra dice roll for beating a piece
                                    CanRollDice = true;
                                    //make the piece size fit the cell
                                    piece.Width = grid.Width / 15;
                                    
                                    IsBeatPiece = true;
                                }
                            }
                        }
                        else
                        //check if the cell has all simillar pieces(in that case we can't move our piece to the position)
                        if (list.Count > 1)
                        {
                            if (list[0].Type != ActivePlayerType)
                                np = -1;
                        }

                        //to decrease the size of the piece if there are multiple pieces in the current cell
                        if (list.Count > 0 && !IsBeatPiece)
                        {
                            piece.Width = list[list.Count - 1].ActualWidth / 1.4;
                            piece.Height = list[list.Count - 1].ActualHeight / 1.4;
                            Panel.SetZIndex(piece, Panel.GetZIndex(list[list.Count - 1])+1);
                        }
                        //else make it fit the cell
                        else
                        {
                            piece.Width = grid.Width / 15;
                            piece.Height = grid.Height / 15;
                            //Panel.SetZIndex(piece, 1);
                        }



                    }
                    #endregion

                    #region To move the piece

                    //move the piece
                    if (np != -1)
                    {
                        var NewPosition = _Positions[piece.CurrentPositionIndex];
                        Grid.SetColumn(piece, NewPosition.Column);
                        Grid.SetRow(piece, NewPosition.Row);
                        Dice.Remove(pi);
                    }
                    //show the alert if it can't be moved
                    else if(np == -2)
                    {
                        AlertText = "Congratulations Your Piece is Home!";
                        await Task.Delay(1000);
                        AlertText = "";
                    }
                    else
                    {
                        AlertText = "Can not move the piece";
                        await Task.Delay(1000);
                        AlertText = "";
                    }

                    #endregion

                    #region To alert if a piece is beaten

                    //if piece is beaten show alert
                    if (IsBeatPiece)
                    {
                        AlertText = "Congrats! You have beaten a piece :)";
                        await Task.Delay(3000);
                        AlertText = "";
                        IsBeatPiece = false;
                    }

                    #endregion

                    #region Transfer turn to next player

                    if (Dice.Count == 0 && !CanRollDice)
                    {
                        if (ActivePlayerType < NumberOfPlayer)
                        {
                            ActivePlayerType++;
                            rollCount = 0;
                            Dice.Clear();
                            PlayerTurn = ActivePlayerType.ToString();
                            DiceText = "Roll Dice!";
                        }
                        else
                        {
                            ActivePlayerType = 0;
                            rollCount = 0;
                            Dice.Clear();
                            PlayerTurn = ActivePlayerType.ToString();
                            DiceText = "Roll Dice!";
                        }
                    }

                    #endregion 
                }

            }

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// list of the diced values
        /// </summary>
        public ObservableCollection<int> Dice { get; set; }

        public string PlayerTurn { get; set; }

        public string DiceText { get; set; } = "Roll Dice!";

        public string AlertText { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Command which run whenever the any piece is click
        /// </summary>
        public ICommand PieceCommand { get; set; }

        /// <summary>
        /// Command that rolls the dice
        /// </summary>
        public ICommand RollDiceCommand { get; set; }

        /// <summary>
        /// Command to move to the next player turn
        /// </summary>
        public ICommand MoveNextCommand { get; set; }

        /// <summary>
        /// To restart the game
        /// </summary>
        public ICommand RestartCommand { get; set; }

        #endregion

        #region Command Methods

        /// <summary>
        /// Executes when a piece popup is clicked
        /// </summary>
        /// <param name="sender"></param>
        public async void Piece_Click(object sender)
        {
            var values = sender as object[];
            Button btn = values[0] as Button;
            Piece piece = values[1] as Piece;
            if (ActivePlayerType > NumberOfPlayer)
                return;
            if (ActivePlayerType != piece.Type || piece.IsHome)
                return;
            //pi is Position To Increment
            //cp is Current Position Index
            //np is Next Position

            var pi = Convert.ToInt32(btn.Content);
            var cp = piece.CurrentPositionIndex;
            var np = pi + cp;
            var NewPosition = new Position();

            //if piece is closed and player wants to open it
            if (!piece.IsOpen && pi == 6)
            {
                NewPosition = _Positions[piece.StartIndex];
                Grid.SetColumn(piece, NewPosition.Column);
                Grid.SetRow(piece, NewPosition.Row);
                Dice.Remove(pi);
                piece.IsOpen = true;

                //index of the current piece in the grid
                int row = _Positions[piece.CurrentPositionIndex].Row;
                int column = _Positions[piece.CurrentPositionIndex].Column;

                //to get a list containing all pieces in a cell of a grid by index
                var list = grid.Children
                           .Cast<Piece>()
                           .Where(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column)
                           .Select(e => e).ToList();

                //to decrease the size of the piece if there are multiple pieces in the current cell
                if (list.Count > 1)
                {
                    piece.Width = list[list.Count - 1].ActualWidth / 1.4;
                    piece.Height = list[list.Count - 1].ActualHeight / 1.4;
                    Panel.SetZIndex(piece, Panel.GetZIndex(list[list.Count - 1])+1);
                }
                //else make it fit the cell
                else
                {
                    piece.Width = grid.Width / 15;
                    piece.Height = grid.Height / 15;
                    //Panel.SetZIndex(piece, 2);
                }
            }
            else if (piece.IsOpen)
            {
                switch (piece.Type)
                {
                    case PlayerType.Red:
                        {
                            #region Calculate the next position of the piece

                            //allow piece to only enter in its own(color) home column
                            if ((cp < 13 && (cp + pi) >= 13) || (cp < 32 && (cp + pi) >= 32) || (cp < 51 && (cp + pi) >= 51))
                                np += 6;

                            //if its in home column or about to enter in it
                            if ((cp < 71 && (cp + pi) >= 71) || (cp > 70 && cp < 76))
                            {
                                //if piece is home
                                if (np == 76)
                                {
                                    NewPosition = _HomePosition[piece.HomeIndex];
                                    Grid.SetColumn(piece, NewPosition.Column);
                                    Grid.SetRow(piece, NewPosition.Row);
                                    piece.IsOpen = false;
                                    piece.IsHome = true;
                                    piece.Style = Application.Current.FindResource("DisablePieceButton") as Style;
                                    //arbitrary value to check if piece has already moved
                                    np = -2;

                                    Player currentPlayer = Players.Select(s => s.PlayerType == ActivePlayerType) as Player;
                                    bool IsPlayerWon = true;
                                    foreach (Piece item in currentPlayer.Pieces)
                                    {
                                        if (item.IsHome == false)
                                        {
                                            IsPlayerWon = false;
                                            break;
                                        }
                                    }
                                    if (IsPlayerWon)
                                    {
                                        MessageBox.Show($"Congratulations you have Won the game!");
                                        (Application.Current.MainWindow.DataContext as WindowViewModel).CurrentPage = new StartUpMenu();
                                    }
                                }
                                //if piece can move inside home column
                                else if (np < 76)
                                {
                                    piece.CurrentPositionIndex = np;
                                }
                                //if piece can't move
                                else
                                {
                                    //arbitrary value to check if piece can move
                                    np = -1;
                                }
                            }
                            else
                            //to reset the position if its at _Position's list last index
                            if (np < _Positions.Count)
                            {
                                piece.CurrentPositionIndex = np;
                            }
                            //normal position where reset is not required
                            else
                            {
                                piece.CurrentPositionIndex = np - _Positions.Count;
                            }

                            #endregion
                            MovePiece(piece, btn, np, pi);
                        }
                        break;
                    case PlayerType.Blue:
                        {
                            #region Calculate the next position of the piece

                            //allow piece to only enter in its own(color) home column
                            if ((cp < 32 && (cp + pi) >= 32) || (cp < 51 && (cp + pi) >= 51) || (cp < 71 && (cp + pi) >= 71))
                                np += 6;

                            //if its in home column or about to enter in it
                            if ((cp < 13 && (cp + pi) >= 13) || (cp > 13 && cp < 18))
                            {
                                //if the piece is home
                                if (np == 18)
                                {
                                    NewPosition = _HomePosition[piece.HomeIndex];
                                    Grid.SetColumn(piece, NewPosition.Column);
                                    Grid.SetRow(piece, NewPosition.Row);
                                    piece.IsOpen = false;
                                    piece.IsHome = true;
                                    piece.Background = Brushes.Black;
                                    //arbitrary value to check if piece has already moved
                                    np = -1;
                                }
                                //if piece can move inside home column
                                else if (np < 18)
                                {
                                    piece.CurrentPositionIndex = np;
                                }
                                //if piece can't move
                                else
                                {
                                    //arbitrary value to check if piece can move
                                    np = -1;
                                }
                            }
                            else
                            //to reset the position if its at _Position's list last index
                            if (np < _Positions.Count)
                            {
                                piece.CurrentPositionIndex = np;
                            }
                            //normal position where reset is not required
                            else
                            {
                                piece.CurrentPositionIndex = np - _Positions.Count;
                            }

                            #endregion
                            MovePiece(piece, btn, np, pi);
                        }
                        break;
                    case PlayerType.Green:
                        {
                            #region Calculate the next position of the piece

                            //allow piece to only enter in its own(color) home column
                            if ((cp < 13 && (cp + pi) >= 13) || (cp < 71 && (cp + pi) >= 71) || (cp < 51 && (cp + pi) >= 51))
                                np += 6;

                            //if its in home column or about to enter in it
                            if ((cp < 32 && (cp + pi) >= 32) || (cp > 32 && cp < 37))
                            {
                                //if the piece is home
                                if (np == 37)
                                {
                                    NewPosition = _HomePosition[piece.HomeIndex];
                                    Grid.SetColumn(piece, NewPosition.Column);
                                    Grid.SetRow(piece, NewPosition.Row);
                                    piece.IsOpen = false;
                                    piece.IsHome = true;
                                    piece.Background = Brushes.Black;
                                    //arbitrary value to check if piece has already moved
                                    np = -1;
                                }
                                //if piece can move inside home column
                                else if (np < 37)
                                {
                                    piece.CurrentPositionIndex = np;
                                }
                                //if piece can't move
                                else
                                {
                                    //arbitrary value to check if piece can move
                                    np = -1;
                                }
                            }
                            else
                            //to reset the position if its at _Position's list last index
                            if (np < _Positions.Count)
                            {
                                piece.CurrentPositionIndex = np;
                            }
                            //normal position where reset is not required
                            else
                            {
                                piece.CurrentPositionIndex = np - _Positions.Count;
                            }

                            #endregion
                            MovePiece(piece, btn, np, pi);
                        }
                        break;
                    case PlayerType.Yellow:
                        {
                            #region Calculate the next position of the piece

                            //allow piece to only enter in its own(color) home column
                            if ((cp < 13 && (cp + pi) >= 13) || (cp < 32 && (cp + pi) >= 32) || (cp < 70 && (cp + pi) >= 70))
                                np += 6;

                            //if its in home column or about to enter in it
                            if ((cp < 51 && (cp + pi) >= 51) || (cp > 51 && cp < 56))
                            {
                                //if the piece is home
                                if (np == 56)
                                {
                                    NewPosition = _HomePosition[piece.HomeIndex];
                                    Grid.SetColumn(piece, NewPosition.Column);
                                    Grid.SetRow(piece, NewPosition.Row);
                                    piece.IsOpen = false;
                                    piece.IsHome = true;
                                    piece.Background = Brushes.Black;
                                    //arbitrary value to check if piece has already moved
                                    np = -1;
                                }
                                //if piece can move inside home column
                                else if (np < 56)
                                {
                                    piece.CurrentPositionIndex = np;
                                }
                                //if piece can't move
                                else
                                {
                                    //arbitrary value to check if piece can move
                                    np = -1;
                                }
                            }
                            else
                            //to reset the position if its at _Position's list last index
                            if (np < _Positions.Count)
                            {
                                piece.CurrentPositionIndex = np;
                            }
                            //normal position where reset is not required
                            else
                            {
                                piece.CurrentPositionIndex = np - _Positions.Count;
                            }

                            #endregion
                            MovePiece(piece, btn, np, pi);
                        }
                        break;
                }
            }
            //if piece is closed
            else
            {
                if (Convert.ToInt32(btn.Content) == 6)
                {
                    piece.IsOpen = true;
                    var i = piece.CurrentPositionIndex;
                    Grid.SetColumn(piece, _Positions[i].Column);
                    Grid.SetRow(piece, _Positions[i].Row);
                    Dice.Remove(6);
                }
                else
                {
                    AlertText = "Piece is closed";
                    await Task.Delay(2000);
                    AlertText = "";
                }
            }
        }

        /// <summary>
        /// Move the turn to next player
        /// </summary>
        public void MoveNextCommandMethod()
        {
            if (ActivePlayerType < NumberOfPlayer)
            {
                ActivePlayerType++;
                rollCount = 0;
                Dice.Clear();
                PlayerTurn = ActivePlayerType.ToString();
                DiceText = "Roll Dice!";
            }
            else
            {
                ActivePlayerType = 0;
                rollCount = 0;
                Dice.Clear();
                PlayerTurn = ActivePlayerType.ToString();
                DiceText = "Roll Dice!";
            }
        }

        /// <summary>
        /// Rolls the dice
        /// </summary>
        /// <returns></returns>
        public async void RollDice()
        {
            // if rolled score of dice is 6 and count is less than 2 keep rolling the dice
            if ((value == 6 && rollCount < 4) || rollCount == 0 || CanRollDice)
            {
                value = rollDice.Next(1, 7);
                DiceText = value.ToString();
                Dice.Add(value);
                rollCount++;
                if (value == 6)
                    CanRollDice = true;
                else
                    CanRollDice = false;
                bool IsAllPiecesClosed = true;
                switch (ActivePlayerType)
                {
                    case PlayerType.Red:
                        {
                            foreach (Piece item in RedPlayer.Pieces)
                            {
                                if (item.IsOpen == true)
                                {
                                    IsAllPiecesClosed = false;
                                    break;
                                }
                            }
                        }
                        break;
                    case PlayerType.Blue:
                        {
                            foreach (Piece item in BluePlayer.Pieces)
                            {
                                if (item.IsOpen == true)
                                {
                                    IsAllPiecesClosed = false;
                                    break;
                                }
                            }
                        }
                        break;
                    case PlayerType.Green:
                        {
                            foreach (Piece item in GreenPlayer.Pieces)
                            {
                                if (item.IsOpen == true)
                                {
                                    IsAllPiecesClosed = false;
                                    break;
                                }
                            }
                        }
                        break;
                    case PlayerType.Yellow:
                        {
                            foreach (Piece item in YellowPlayer.Pieces)
                            {
                                if (item.IsOpen == true)
                                {
                                    IsAllPiecesClosed = false;
                                    break;
                                }
                            }
                        }
                        break;
                }
                if (!Dice.Contains(6) && IsAllPiecesClosed)
                {
                    AlertText = "Next player turn";
                    await Task.Delay(1500);
                    if (ActivePlayerType < NumberOfPlayer)
                    {
                        ActivePlayerType++;
                        rollCount = 0;
                        Dice.Clear();
                        PlayerTurn = ActivePlayerType.ToString();
                        DiceText = "Roll Dice!";
                    }
                    else
                    {
                        ActivePlayerType = 0;
                        rollCount = 0;
                        Dice.Clear();
                        PlayerTurn = ActivePlayerType.ToString();
                        DiceText = "Roll Dice!";
                    }
                    AlertText = "";
                }

            }
        }

        /// <summary>
        /// restarts the game
        /// </summary>
        public void Restart()
        {
            (Application.Current.MainWindow.DataContext as WindowViewModel).CurrentPage = new StartUpMenu();
        }
        #endregion
    }
}
