using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GameOfLife.Lib;

namespace GameOfLife.Console
{
    class Program
    {
        private static char charAliveCell = (char) 2;

        private static void Main(string[] args)
        {
            var totalGen = 50;
            var rows = 31;
            var columns = 31;


            var lifeBoard = new LifeBoard(rows, columns);

            for (int i = 0; i < rows; i++)
            {
                lifeBoard.ToggleCell(i, 15);
                for (int j = 0; j < columns; j++)
                {
                    lifeBoard.ToggleCell(15, j);
                }
            }

            var done = false;
            while (!done)
            {
                PrintCells(lifeBoard);
                lifeBoard.ProcessNext();
                var key = System.Console.ReadLine();
                if (key == "e" || key == "E")
                {
                    done = true;
                }
                
            }
    }

        private static void PrintCells(LifeBoard lifeBoard)
        {
            System.Console.WriteLine("Generation : {0} ", lifeBoard.Generation);
            System.Console.WriteLine();
            for (int x = 0; x < lifeBoard.Rows; x++)
            {
                for (int y = 0; y < lifeBoard.Columns; y++)
                {
                    System.Console.Write("{0} ", lifeBoard[x,y].IsAlive ? charAliveCell.ToString(CultureInfo.InvariantCulture) : "-");
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine();
            System.Console.WriteLine(@"Press any key to Continue or Enter ""e|E"" to exit");
        }
    }
}
