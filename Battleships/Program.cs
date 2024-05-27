using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipLibrary;
using BattleshipLibrary.Models;


namespace Battleships
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ConsoleMethods.WelcomeMessage();

            var activePlayer = ConsoleMethods.CreatePlayer("player 1");
            var opponent = ConsoleMethods.CreatePlayer("player 2");
            PlayerInfoModel winner = null;

            do
            {
                ConsoleMethods.DisplayShotGrid(activePlayer);
                ConsoleMethods.RecordPlayerShot(activePlayer, opponent);
                var doesGameContinue = GameLogic.PlayerStillActive(opponent);

                if (doesGameContinue == true)
                {
                    //// swap using a temp variable used before C# 7.0
                    //var tempHolder = opponent;
                    //opponent = activePlayer;
                    //activePlayer = tempHolder;

                    (activePlayer, opponent) = (opponent, activePlayer); // Tuple
                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            ConsoleMethods.GetWinner(winner);

            Console.ReadLine();
        }
    }
}
