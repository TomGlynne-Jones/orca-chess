using System;
using System.Collections.Generic;

namespace OrcaChess.Game.Interfaces
{
	public interface IGameState
    {
        public List<string> GetAvailableMoves();

        public void MakeMove(string move);

        public void UnMakeLastMove();

        public void PrintBoard();
    }
}
