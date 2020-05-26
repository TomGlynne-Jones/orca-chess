using OrcaChess.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OrcaChess.Tests")]
namespace OrcaChess.ConnectFour
{
    class ConnectFourState : IGameState
    {
        private const int DefaultNumCols = 7;
        private const int DefaultNumRows = 6;
        private const int DefaultNumToConnect = 4;

        private int _numCols;
        private int _numRows;
        private byte[,] _board;

        private int _numToConnect;

        private byte _currentPlayer;

        private Stack<string> _moveHistory;
        
        public ConnectFourState()
        {
            _numCols = DefaultNumCols;
            _numRows = DefaultNumRows;
            _board = new byte[_numRows, _numCols];
            _numToConnect = DefaultNumToConnect;
            _moveHistory = new Stack<string>();
            _currentPlayer = 1;
        }

        /// <summary>
        /// Configurable Constructor
        /// </summary>
        /// <param name="numColumns"></param>
        /// <param name="numRows"></param>
        /// <param name="numToConnect"></param>
        public ConnectFourState(int numColumns, int numRows, int numToConnect)
        {
            if (numToConnect > numColumns || numToConnect > numRows) 
            {
                throw new ArgumentException("Number to connect must not exceed number of columns or number of rows");
            }

            _numCols = numColumns;
            _numRows = numRows;
            _board = new byte[_numRows, _numCols];
            _numToConnect = numToConnect;
            _moveHistory = new Stack<string>();
            _currentPlayer = 1;
        }

        public List<string> GetAvailableMoves()
        {
            var availableMoves = new List<string>();
            for (int colIdx=0; colIdx < _numCols; colIdx++)
            {
                for (int rowIdx = 0; rowIdx < _numRows; rowIdx++)
                {
                    if (_board[rowIdx, colIdx] == 0)
                    {
                        availableMoves.Add(colIdx.ToString());
                        break;
                    }
                }
            }
            // TODO decouple column index from move name
            return availableMoves;
        }

        private string _GetPieceCharacterForPlayer(byte gridValue)
        {
            string piece;
            if (gridValue == 0) 
            {
                piece = " ";
            }
            else if (gridValue == 1) 
            {
                piece = "O";
            }
            else 
            {
                piece = "X";
            }
            return piece;
        }

        /// <summary>
        /// Alternate between _currentPlayer = 1 and _currentPlayer = 2
        /// </summary>
        private void _AlternatePlayers()
        {
            _currentPlayer = (byte) (3 - _currentPlayer);
        }

        /// <summary>
        /// TODO 
        /// </summary>
        /// <param name="move"></param>
        public void MakeMove(string move)
        {
            var availableMoves = GetAvailableMoves();
            if (! availableMoves.Contains(move))
            {
                throw new ArgumentException("Invalid move");
            }
            var colIdx = int.Parse(move);
            for (int rowIdx = _numRows - 1; rowIdx >= 0; rowIdx--)
                {
                if (_board[rowIdx, colIdx] == 0)
                {
                    _board[rowIdx, colIdx] = _currentPlayer;
                    break;
                }
            }
            _moveHistory.Push(move);
            _AlternatePlayers();
        }

        /// <summary>
        /// Unmake the last move. Useful for efficient AI search.
        /// </summary>
        public void UnMakeLastMove() 
        {
            string lastMove = _moveHistory.Pop().ToString();
            var colIdx = int.Parse(lastMove);
            for (int rowIdx = _numRows - 1; rowIdx >= 0; rowIdx--)
            {
                if (_board[rowIdx, colIdx] != 0)
                {
                    _board[rowIdx, colIdx] = 0;
                    break;
                }
            }
            _AlternatePlayers();
        }

        /// <summary>
        /// TODO Fix this
        /// </summary>
        public void PrintBoard()
        {
            for (int rowIdx = 0; rowIdx < _numRows; rowIdx++)
            {
                var line = "|";
                for (int colIdx = 0; colIdx < _numCols; colIdx++)
                {
                    string piece = _GetPieceCharacterForPlayer(_board[rowIdx, colIdx]);
                    line += piece + "|";
                }
                Console.WriteLine(line);
            }
            Console.WriteLine(" 0 1 2 3 4 5 6 " + Environment.NewLine);
        }
    }
}
