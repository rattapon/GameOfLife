using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using GameOfLife.Lib;

namespace GameOfLife.Test
{
    [TestFixture]
    public class GameOfLifeTest
    {

        [Test]
        public void CellShouldBeDeadAtInitial()
        {
            var cell = new Cell();

            Assert.AreEqual(cell.IsAlive, false);
        }

        [Test]
        public void InitializeBoardSuccess()
        {
            var lifeBoard = new LifeBoard(9, 9);

            Assert.That(9, Is.EqualTo(lifeBoard.Rows));
            Assert.That(9, Is.EqualTo(lifeBoard.Columns));
            Assert.That(81, Is.EqualTo(lifeBoard.CellsCount));
            
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void InitiateLifeBoardWithZeroShouldThrowException()
        {
            var lifeBoard = new LifeBoard(0, 0);
            Assert.Fail();
        }

        [Test]
        public void DeadCellShouldBeAliveWhenToggle()
        {
            var lifeBoard = new LifeBoard(10, 10);
            Assert.That(false, Is.EqualTo(lifeBoard[5,5].IsAlive));
            lifeBoard.ToggleCell(5, 5);
            Assert.That(true, Is.EqualTo(lifeBoard[5,5].IsAlive));
        }

        [Test]
        public void AliveCellShouldBeDeadWhenToggle()
        {
            var lifeBoard = new LifeBoard(10, 10);
            Assert.That(false, Is.EqualTo(lifeBoard[5, 5].IsAlive));
            lifeBoard.ToggleCell(5, 5);
            Assert.That(true, Is.EqualTo(lifeBoard[5, 5].IsAlive));
            lifeBoard.ToggleCell(5, 5);
            Assert.That(false, Is.EqualTo(lifeBoard[5, 5].IsAlive));
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void GetLifeCellWithInvalidIndexShouldThrowException()
        {
            var lifeBoard = new LifeBoard(3, 3);
            var cell = lifeBoard[3, 3];
            Assert.Fail();
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void ToggleLifeBoardCellWithInvalidIndexShouldThrowException()
        {
            var lifeBoard = new LifeBoard(3, 3);
            lifeBoard.ToggleCell(-1, 0);
            Assert.Fail();

        }

        [Test]
        public void ShouldCountNumberOfAliveNeiboursCorrectly()
        {
            var lifeBoard = new LifeBoard(3, 3);
            
            lifeBoard.ToggleCell(0, 0);
            lifeBoard.ToggleCell(0, 1);
            lifeBoard.ToggleCell(0, 2);

            Assert.AreEqual(1, lifeBoard.CountAliveNeighbours(0, 0));
            Assert.AreEqual(2, lifeBoard.CountAliveNeighbours(0, 1));
            Assert.AreEqual(1, lifeBoard.CountAliveNeighbours(0, 2));
            Assert.AreEqual(2, lifeBoard.CountAliveNeighbours(1, 0));
            Assert.AreEqual(3, lifeBoard.CountAliveNeighbours(1, 1));
            Assert.AreEqual(2, lifeBoard.CountAliveNeighbours(1, 2));
            Assert.AreEqual(0, lifeBoard.CountAliveNeighbours(2, 0));
            Assert.AreEqual(0, lifeBoard.CountAliveNeighbours(2, 1));
            Assert.AreEqual(0, lifeBoard.CountAliveNeighbours(2, 2));
        }

        [Test]
        public void AliveCellWithFewerThan2LivedNeighboursShouldBeDead()
        {
            var lifeBoard = new LifeBoard(3, 3);
            lifeBoard.ToggleCell(1,1);
            Assert.That(true, Is.EqualTo(lifeBoard[1,1].IsAlive));
            lifeBoard.ProcessNext();
            Assert.That(false, Is.EqualTo(lifeBoard[1,1].IsAlive));
            Assert.That(2, Is.EqualTo(lifeBoard.Generation));

            lifeBoard.ToggleCell(0, 0);
            lifeBoard.ToggleCell(0, 1);
            lifeBoard.ToggleCell(0, 2);
            Assert.That(true, Is.EqualTo(lifeBoard[0, 0].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[0, 1].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[0, 2].IsAlive));
            lifeBoard.ProcessNext();
            Assert.That(false, Is.EqualTo(lifeBoard[0, 0].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[0, 1].IsAlive));
            Assert.That(false, Is.EqualTo(lifeBoard[0, 2].IsAlive));
            Assert.That(3, Is.EqualTo(lifeBoard.Generation));
            
        }

        [Test]
        public void AliveCellWithMoreThan3NeighboursShouldBeDead()
        {
            var lifeBoard = new LifeBoard(3, 3);
            lifeBoard.ToggleCell(0, 0);
            lifeBoard.ToggleCell(0, 1);
            lifeBoard.ToggleCell(0, 2);
            lifeBoard.ToggleCell(1, 0);
            lifeBoard.ToggleCell(1, 1);
            lifeBoard.ToggleCell(1, 2);
            lifeBoard.ToggleCell(2, 0);
            lifeBoard.ToggleCell(2, 1);
            lifeBoard.ToggleCell(2, 2);

            lifeBoard.ProcessNext();
            Assert.That(true, Is.EqualTo(lifeBoard[0, 0].IsAlive));
            Assert.That(false, Is.EqualTo(lifeBoard[0, 1].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[0, 2].IsAlive));
            Assert.That(false, Is.EqualTo(lifeBoard[1, 0].IsAlive));
            Assert.That(false, Is.EqualTo(lifeBoard[1, 1].IsAlive));
            Assert.That(false, Is.EqualTo(lifeBoard[1, 2].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[2, 0].IsAlive));
            Assert.That(false, Is.EqualTo(lifeBoard[2, 1].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[2, 2].IsAlive));
            Assert.That(2, Is.EqualTo(lifeBoard.Generation));
            
        }

        [Test]
        public void AliveCellWith2Or3NeiboursShouldBeAliveInNextGeneration()
        {
            var lifeBoard = new LifeBoard(3, 3);
            lifeBoard.ToggleCell(0, 1);
            lifeBoard.ToggleCell(1, 0);
            lifeBoard.ToggleCell(1, 1);
            lifeBoard.ToggleCell(1, 2);
           
            lifeBoard.ProcessNext();
            Assert.That(true, Is.EqualTo(lifeBoard[0, 1].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[1, 0].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[1, 1].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[1, 2].IsAlive));
            Assert.That(2, Is.EqualTo(lifeBoard.Generation));
        }

        [Test]
        public void DeadCellWithExact3AliveNeiboursShouldBecomeAlive()
        {
            var lifeBoard = new LifeBoard(3, 3);
            lifeBoard.ToggleCell(0, 1);
            lifeBoard.ToggleCell(1, 0);
            lifeBoard.ToggleCell(1, 1);
            lifeBoard.ToggleCell(1, 2);
            lifeBoard.ToggleCell(2, 1);

            lifeBoard.ProcessNext();
            Assert.That(true, Is.EqualTo(lifeBoard[0, 0].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[0, 2].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[2, 0].IsAlive));
            Assert.That(true, Is.EqualTo(lifeBoard[2, 2].IsAlive));
            Assert.That(2, Is.EqualTo(lifeBoard.Generation));
        }


    }
}


