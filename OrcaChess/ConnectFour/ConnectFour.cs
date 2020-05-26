using OrcaChess.Game.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

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
        /// Return the indexes of any columns that are not filled
        /// </summary>
        /// <returns></returns>
        private List<int> _openColumns()
        {
            var availableCols = new List<int>();
            for (int colIdx = 0; colIdx < _numCols; colIdx++)
            {
                for (int rowIdx = 0; rowIdx < _numRows; rowIdx++)
                {
                    if (_board[rowIdx, colIdx] == 0)
                    {
                        availableCols.Add(colIdx);
                        break;
                    }
                }
            }
            return availableCols;
        }

        public List<string> GetAvailableMoves()
        {
            var availableColumns = _openColumns();
            return availableColumns.Select(x => x.ToString()).ToList();
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

        public bool IsDraw()
        {
            if (_openColumns().Count==0) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool _PlayerHasVerticalFourInARow(int player)
        {
            for (int colIdx = 0; colIdx <_numCols; colIdx++)
            {
                // Start at the bottom of the board/array (higher indexes)
                for (int startIdx = _numRows - 4; startIdx >= 0; startIdx--)
                {
                    var fourInARow = true;
                    for (int offset = 0; offset <= 3; offset++)
                    {
                        var rowIdx = startIdx + offset;
                        if (_board[rowIdx, colIdx] != player)
                        {
                            fourInARow = false;
                            break;
                        }
                    }
                    if (fourInARow)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool _PlayerHasHorizontalFourInARow(int player)
        {
            // Start at the bottom of the board/array (higher indexes)
            for (int rowIdx = _numRows-1; rowIdx >=0; rowIdx--)
            {
                for (int startIdx = 0; startIdx <= _numCols - 4; startIdx++)
                {
                    var fourInARow = true;
                    for (int offset = 0; offset <= 3; offset++)
                    {
                        var colIdx = startIdx + offset;
                        if (_board[rowIdx, colIdx] != player)
                        {
                            fourInARow = false;
                            break;
                        }
                    }
                    if (fourInARow)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool _PlayerHasDiagonalFourInARow(int player)
        {
            // Start at the bottom of the board/array (higher indexes)
            for (int startRowIdx = _numRows -4; startRowIdx >= 0; startRowIdx--)
            {
                for (int startColIdx = 0; startColIdx <= _numCols - 4; startColIdx++)
                {
                    // Check \-direction
                    var fourInARow = true;
                    for (int offset = 0; offset <= 3; offset++)
                    {
                        var colIdx = startColIdx + offset; 
                        var rowIdx = startRowIdx + offset;
                        if (_board[rowIdx, colIdx] != player)
                        {
                            fourInARow = false;
                            break;
                        }
                    }
                    if (fourInARow)
                    {
                        return true;
                    }

                    // Check /-direction
                    fourInARow = true;
                    var upwardStartRowIdx = startRowIdx + 3;

                    for (int offset = 0; offset <= 3; offset++)
                    {
                        var colIdx = startColIdx + offset;
                        var rowIdx = upwardStartRowIdx - offset;
                        if (_board[rowIdx, colIdx] != player)
                        {
                            fourInARow = false;
                            break;
                        }
                    }
                    if (fourInARow)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public bool _PlayerHasFourInARow(int player)
        {
            return (_PlayerHasHorizontalFourInARow(player) || _PlayerHasVerticalFourInARow(player) || _PlayerHasDiagonalFourInARow(player));
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
