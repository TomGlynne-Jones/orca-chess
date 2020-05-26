using System;
using System.Collections;
using OrcaChess.Game.Interfaces;
using OrcaChess.Player;
using OrcaChess.Player.Interfaces;
using OrcaChess.ConnectFour;

namespace OrcaChess.Game
{
    class Game
    {
        private IGameState _gameState;
        private int _currentPlayerIdx = 0;
        private IPlayer[] _players;


        public Game()
        {
            _gameState = new ConnectFourState();
            _players = new IPlayer[] { new HumanPlayer(), new HumanPlayer()}; 
        }

        public void Play()
        {
            while (true) 
            {
                _gameState.PrintBoard();
                IPlayer player = _players[_currentPlayerIdx];
                var move = player.GetMove(_gameState);
                _gameState.MakeMove(move);
            }
        }
        private void _AlternatePlayers()
        {
            _currentPlayerIdx += 1;
            _currentPlayerIdx = _currentPlayerIdx % 2;
        }
    }
}