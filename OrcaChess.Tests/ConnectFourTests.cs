using System;
using Xunit;
using OrcaChess.ConnectFour;

namespace OrcaChess.Tests
{
    public class ConnectFourTests
    {
        [Fact]
        public void TestNewGameHasSevenAvailableMoves()
        {
            // Arrange
            var gameState = new ConnectFourState();

            // Act
            var moves = gameState.GetAvailableMoves();

            // Assert
            Assert.True((moves.Count == 7));
        }
    }
}
