using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameOfLife.Lib
{
    public class LifeBoard
    {

        private Cell[,] currentGen;
        private Cell[,] nextGen;

        public LifeBoard(int row, int column)
        {
            if (row <= 0 || column <= 0) throw new ArgumentException("Rows and Columns must be greater than 0");

            Rows = row;
            Columns = column;
            Generation = 1;
            currentGen = new Cell[row, column];
            nextGen = new Cell[row, column];
            Initialize();
        }

        private void Initialize()
        {
            Parallel.For(0, Rows, x => Parallel.For(0, Columns, y =>
            {
                currentGen[x, y] = new Cell();
                nextGen[x, y] = new Cell();
            }
                ));
        }

        public Cell this[int x, int y]
        {
            get { return ValidatedIndex(x, y) ? currentGen[x, y] : null; }
        }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public int CellsCount
        {
            get { return Rows*Columns; }
        }

        public int Generation { get; private set; }

        public void ToggleCell(int x, int y)
        {
            this[x, y].Toggle();
        }

        private bool ValidatedIndex(int x, int y)
        {
            if (x >= 0 && y >= 0)
            {
                if (x < Rows && y < Columns)
                    return true;
            }

            throw new ArgumentException("Coordinate is out of bound");
        }

        public void ProcessNext()
        {
            Parallel.For(0, Rows, x => Parallel.For(0, Columns, y =>
            {
                    var alive = this[x, y].IsAlive;
                    var numAliveNeigbours = CountAliveNeighbours(x, y);

                    var aliveNext = (alive && numAliveNeigbours >= 2 && numAliveNeigbours <= 3) ||
                            (!alive && numAliveNeigbours == 3);   

                    UpdateNextGen(x, y, aliveNext);
               
            }));

            UpdateLifeBoard();
        }

        private void UpdateLifeBoard()
        {
            UpdateCurrentGen();
            Generation++;
        }

        private void UpdateCurrentGen()
        {
            Parallel.For(0, Rows, x => Parallel.For(0, Columns, y =>
            {
                currentGen[x, y].IsAlive = nextGen[x, y].IsAlive;
            }));
        }

        private void UpdateNextGen(int x, int y, bool alive)
        {
            ValidatedIndex(x, y);
            nextGen[x, y].IsAlive = alive;
        }

        public int CountAliveNeighbours(int x, int y)
        {
            return IsAliveNeighbour(x - 1, y + 1) + IsAliveNeighbour(x, y + 1) + IsAliveNeighbour(x + 1, y + 1) +
                   IsAliveNeighbour(x - 1, y) + IsAliveNeighbour(x + 1, y) + IsAliveNeighbour(x - 1, y - 1) +
                   IsAliveNeighbour(x, y - 1) + IsAliveNeighbour(x + 1, y - 1);
        }

        private int IsAliveNeighbour(int x, int y)
        {
            if (x >= 0 && y >= 0)
            {
                if(x < Rows && y < Columns)
                    return currentGen[x, y].IsAlive ? 1 : 0;
            }

            return 0;
        }

}

}
