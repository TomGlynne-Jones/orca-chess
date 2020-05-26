using System;
using OrcaChess.Game.Interfaces;

namespace OrcaChess.Player.Interfaces

{
    public interface IPlayer
    {
        public string GetMove(IGameState gameState);
    }
}