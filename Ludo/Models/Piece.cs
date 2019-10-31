using System.Collections.Generic;
using System.Windows.Controls;

namespace Ludo
{
    /// <summary>
    /// The piece in the game board
    /// </summary>
    public class Piece : Button
    {

        /// <summary>
        /// Current index according to Position list
        /// </summary>
        public int CurrentPositionIndex { get; set; }

        /// <summary>
        /// tells if the piece is at home (means arive at its final destination)
        /// </summary>
        public bool IsHome { get; set; } = false;

        /// <summary>
        /// tells if the piece is open
        /// </summary>
        public bool IsOpen { get; set; } = false;

        /// <summary>
        /// the score or position of the piece
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Type of player
        /// </summary>
        public PlayerType Type { get; set; }

        /// <summary>
        /// Home index of the piece according to player color
        /// </summary>
        public int HomeIndex { get; set; }

        /// <summary>
        /// index to start at when opens
        /// </summary>
        public int StartIndex { get; set; }

        
    }
}
