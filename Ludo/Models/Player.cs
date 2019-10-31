using System.Collections.Generic;
using System.Windows;

namespace Ludo
{
    public class Player
    {
        public Player(int StartPosition, PlayerType PlayerType)
        {
            //initialize the pieces list
            Pieces = new List<Piece>();

            //sets the player type
            this.PlayerType = PlayerType;

            //style variable for the piece style
            var Style = new Style();

            //to select the piece template accroding to player type
            switch(PlayerType)
            {
                case PlayerType.Red:
                    Style = Application.Current.FindResource("RedPieceButton") as Style;
                    break;
                case PlayerType.Blue:
                    Style = Application.Current.FindResource("BluePieceButton") as Style;
                    break;
                case PlayerType.Green:
                    Style = Application.Current.FindResource("GreenPieceButton") as Style;
                    break;
                case PlayerType.Yellow:
                    Style = Application.Current.FindResource("YellowPieceButton") as Style;
                    break;
            }

            //initialize the pieces
            for (int i = 0; i < 4; i++)
            {
                var Piece = new Piece() { CurrentPositionIndex = StartPosition , StartIndex = StartPosition , Style = Style   , Type = PlayerType};
                //add the pieces to the list
                Pieces.Add(Piece);
            }
            
        }
        
        public List<Piece> Pieces { get; set; }

        public PlayerType PlayerType { get; set; }
    }
}
