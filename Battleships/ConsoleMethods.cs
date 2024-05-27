using BattleshipLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BattleshipLibrary;

namespace Battleships
{
    public static class ConsoleMethods
    {
        public static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to battleship");
            Console.WriteLine("**********************");
            Console.WriteLine();
        }

        public static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            Console.WriteLine($"Player information for {playerTitle}");

            var output = new PlayerInfoModel();
            output.UsersName = AskForUsersName();
            GameLogic.MakeGrid(output);
            PlaceShips(output);

            Console.Clear();
            return output;
        }

        private static string AskForUsersName()
        {
            Console.Write($"What is your name: ");
            var output = Console.ReadLine();
            return output;
        }

        private static void PlaceShips(PlayerInfoModel player)
        {
            do
            {
                Console.Write($"Where would you like to place ship number {player.ShipLocations.Count + 1}: ");
                var location = Console.ReadLine();

                bool isValidLocation = false;

                try
                {
                    isValidLocation = GameLogic.PlaceShip(player, location);
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"Error: {ex.Message}");
                }

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again.");
                }
            } while (player.ShipLocations.Count < 5);
        }

        public static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            string currentRow = activePlayer.ShotGrid[0].SpotLetter;

            foreach (var gridSpot in activePlayer.ShotGrid)
            {
                if (gridSpot.SpotLetter != currentRow)
                {
                    Console.WriteLine();
                    currentRow = gridSpot.SpotLetter;
                }

                if (gridSpot.Status == GridSpotStatus.Empty)
                {
                    Console.Write($" {gridSpot.SpotLetter}{gridSpot.SpotNumber} ");
                }
                else if (gridSpot.Status == GridSpotStatus.Hit)
                {
                    Console.Write(" X  ");
                }
                else if (gridSpot.Status == GridSpotStatus.Miss)
                {
                    Console.Write(" O  ");
                }
                else
                {
                    Console.Write(" ?  "); // used for testing if something went wrong
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            var isValidShot = false;
            var row = "";
            var column = 0;

            do
            {
                var shot = AskForShot(activePlayer);
                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {
                    isValidShot = false;
                }

                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location, please try again.");
                }
            } while (isValidShot == false);

            var isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

            DisplayShotResult(row, column, isAHit);
        }

        private static void DisplayShotResult(string row, int column, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{row}{column} is a HIT!");
            }
            else
            {
                Console.WriteLine($"{row}{column} is a MISS.");
            }

            Console.WriteLine();
        }

        private static string AskForShot(PlayerInfoModel player)
        {
            Console.Write($"{player.UsersName}, where would you like to shoot: ");
            var output = Console.ReadLine();

            return output;
        }

        public static void GetWinner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UsersName}, You won!");
            Console.WriteLine($"{winner.UsersName} used {GameLogic.GetShotCount(winner)} shots to win");
        }
    }
}
