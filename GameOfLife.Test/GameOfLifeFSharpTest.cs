using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FSharp.Collections;
using NUnit.Framework;
using GameOfLife.FSharp;

namespace GameOfLife.Test
{

    public static class Extensions
    {
        public static FSharpList<T> ToFSharpList<T>(this IEnumerable<T> source)
        {
            return ListModule.OfSeq(source);
        }
    }

    [TestFixture]
    class GameOfLifeFSharpTest
    {
        [Test]
        public void InitializeSuccessfully()
        {
            var cells = FSharp.GameOfLife.Initialize(10, 20);
            Assert.AreEqual(200, cells.Length);
        }

        [TestCase(0, 10)]
        [TestCase(0, 0)]
        [TestCase(10, 0)]
        [TestCase(-10, -10)]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenInitializeWithNumberSmallerThan1(int x , int y)
        {
            var cells = FSharp.GameOfLife.Initialize(x, y);
            Assert.Fail();
        }

        [Test]
        public void LiveUpCellShouldBeSuccess()
        {
            var cells = FSharp.GameOfLife.Initialize(3, 3);
            var liveCells = CreateLives3StraightPattern();
           
            var newCells = FSharp.GameOfLife.LiveCell(liveCells.ToFSharpList(), cells);

            Assert.AreEqual(true, newCells[1, 0]);
            Assert.AreEqual(true, newCells[1, 1]);
            Assert.AreEqual(true, newCells[1, 2]);
            
        }

        [Test]
        public void ShouldCountAliveNeighboursCorrectly()
        {
            var cells = FSharp.GameOfLife.Initialize(3, 3);
            var liveCells = CreateLives3StraightPattern();

            var world = FSharp.GameOfLife.LiveCell(liveCells.ToFSharpList(), cells);

            Assert.AreEqual(true, world[1, 0]);
            Assert.AreEqual(true, world[1, 1]);
            Assert.AreEqual(true, world[1, 2]);

            Assert.AreEqual(2, FSharp.GameOfLife.CountNeighbours(0, 0, world));
            Assert.AreEqual(3, FSharp.GameOfLife.CountNeighbours(0, 1, world));
            Assert.AreEqual(2, FSharp.GameOfLife.CountNeighbours(0, 2, world));
            Assert.AreEqual(1, FSharp.GameOfLife.CountNeighbours(1, 0, world));
            Assert.AreEqual(2, FSharp.GameOfLife.CountNeighbours(1, 1, world));
            Assert.AreEqual(1, FSharp.GameOfLife.CountNeighbours(1, 2, world));
            Assert.AreEqual(2, FSharp.GameOfLife.CountNeighbours(2, 0, world));
            Assert.AreEqual(3, FSharp.GameOfLife.CountNeighbours(2, 1, world));
            Assert.AreEqual(2, FSharp.GameOfLife.CountNeighbours(2, 2, world));

        }

        [Test]
        public void AliveCellWithFewerThan2LivedNeighboursShouldBeDead()
        {
            var cells = FSharp.GameOfLife.Initialize(3, 3);
            var liveCells = CreateLives3StraightPattern();

            var world = FSharp.GameOfLife.LiveCell(liveCells.ToFSharpList(), cells);

            Assert.AreEqual(true, world[1, 0]);
            Assert.AreEqual(true, world[1, 1]);
            Assert.AreEqual(true, world[1, 2]);

            var nextGen = FSharp.GameOfLife.NextGeneration(world);

            Assert.AreEqual(false, nextGen[0, 0]);
            Assert.AreEqual(true, nextGen[0, 1]);
            Assert.AreEqual(false, nextGen[0, 2]);
            Assert.AreEqual(false, nextGen[1, 0]);
            Assert.AreEqual(true, nextGen[1, 1]);
            Assert.AreEqual(false, nextGen[1, 2]);
            Assert.AreEqual(false, nextGen[2, 0]);
            Assert.AreEqual(true, nextGen[2, 1]);
            Assert.AreEqual(false, nextGen[2, 2]);

        }

        [Test]
        public void AliveCellWithMoreThan3NeighboursShouldBeDead()
        {
            var cells = FSharp.GameOfLife.Initialize(3, 3);
            var liveCells = CreateLives9LivePattern();

            var world = FSharp.GameOfLife.LiveCell(liveCells.ToFSharpList(), cells);

            Assert.AreEqual(true, world[0, 0]);
            Assert.AreEqual(true, world[0, 1]);
            Assert.AreEqual(true, world[0, 2]);
            Assert.AreEqual(true, world[1, 0]);
            Assert.AreEqual(true, world[1, 1]);
            Assert.AreEqual(true, world[1, 2]);
            Assert.AreEqual(true, world[2, 0]);
            Assert.AreEqual(true, world[2, 1]);
            Assert.AreEqual(true, world[2, 2]);

            var nextGen = FSharp.GameOfLife.NextGeneration(world);

            Assert.AreEqual(true, nextGen[0, 0]);
            Assert.AreEqual(false, nextGen[0, 1]);
            Assert.AreEqual(true, nextGen[0, 2]);
            Assert.AreEqual(false, nextGen[1, 0]);
            Assert.AreEqual(false, nextGen[1, 1]);
            Assert.AreEqual(false, nextGen[1, 2]);
            Assert.AreEqual(true, nextGen[2, 0]);
            Assert.AreEqual(false, nextGen[2, 1]);
            Assert.AreEqual(true, nextGen[2, 2]);

        }

        [Test]
        public void AliveCellWith2Or3NeiboursShouldBeAliveInNextGeneration()
        {
            var cells = FSharp.GameOfLife.Initialize(3, 3);
            var liveCells = new List<Tuple<int, int>>
            {
                Tuple.Create(0, 1),
                Tuple.Create(1, 0),
                Tuple.Create(1, 1),
                Tuple.Create(1, 2),
            };

            var world = FSharp.GameOfLife.LiveCell(liveCells.ToFSharpList(), cells);
            Assert.AreEqual(true, world[0, 1]);
            Assert.AreEqual(true, world[1, 0]);
            Assert.AreEqual(true, world[1, 1]);
            Assert.AreEqual(true, world[1, 2]);

            var nextGen = FSharp.GameOfLife.NextGeneration(world);

            Assert.AreEqual(true, nextGen[0, 1]);
            Assert.AreEqual(true, nextGen[1, 0]);
            Assert.AreEqual(true, nextGen[1, 1]);
            Assert.AreEqual(true, nextGen[1, 2]);
        }

        [Test]
        public void DeadCellWithExact3AliveNeiboursShouldBecomeAlive()
        {
            var cells = FSharp.GameOfLife.Initialize(3, 3);
            var liveCells = CreateCrossPattern();

            var world = FSharp.GameOfLife.LiveCell(liveCells.ToFSharpList(), cells);
            Assert.AreEqual(false, world[0, 0]);
            Assert.AreEqual(true, world[0, 1]);
            Assert.AreEqual(false, world[0, 2]);
            Assert.AreEqual(true, world[1, 0]);
            Assert.AreEqual(true, world[1, 1]);
            Assert.AreEqual(true, world[1, 2]);
            Assert.AreEqual(false, world[2, 0]);
            Assert.AreEqual(true, world[2, 1]);
            Assert.AreEqual(false, world[2, 2]);

            var nextGen = FSharp.GameOfLife.NextGeneration(world);

            Assert.AreEqual(true, nextGen[0, 0]);
            Assert.AreEqual(true, nextGen[0, 2]);
            Assert.AreEqual(true, nextGen[2, 0]);
            Assert.AreEqual(true, nextGen[2, 2]);
        }
        
        private IEnumerable<Tuple<int, int>> CreateLives3StraightPattern()
        {
            return new List<Tuple<int, int>>
            {
                Tuple.Create(1, 0),
                Tuple.Create(1, 1),
                Tuple.Create(1, 2)
            };
        }

        private IEnumerable<Tuple<int, int>> CreateLives9LivePattern()
        {
            return new List<Tuple<int, int>>
            {
                Tuple.Create(0, 0),
                Tuple.Create(0, 1),
                Tuple.Create(0, 2),
                Tuple.Create(1, 0),
                Tuple.Create(1, 1),
                Tuple.Create(1, 2),
                Tuple.Create(2, 0),
                Tuple.Create(2, 1),
                Tuple.Create(2, 2)
            };
        }

        private IEnumerable<Tuple<int, int>> CreateCrossPattern()
        {
            return new List<Tuple<int, int>>
            {
                Tuple.Create(0, 1),
                Tuple.Create(1, 0),
                Tuple.Create(1, 1),
                Tuple.Create(1, 2),
                Tuple.Create(2, 1),
            };
        }

        
    }
}
