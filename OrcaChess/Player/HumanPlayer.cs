using System;
using OrcaChess.Player.Interfaces;
using OrcaChess.Game.Interfaces;

namespace OrcaChess.Player

{
	class HumanPlayer : IPlayer
    {
        public string GetMove(IGameState gameState)
        {
            string move;
            var availableMoves = gameState.GetAvailableMoves();
            while (true) {
                Console.WriteLine("Enter Move...");
                move = Console.ReadLine();
                if (availableMoves.Contains(move))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Invalid move. Available moves: {string.Join(", ", availableMoves)}");
                }
            }

            
            return move;
        }
    }
}