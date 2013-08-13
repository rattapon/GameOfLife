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
            var liveCells = new List<Tuple<int, int>>()
            {
                Tuple.Create(0, 0),
                Tuple.Create(0, 1),
                Tuple.Create(0, 2),
            };
           
            var newCells = FSharp.GameOfLife.LiveCell(liveCells.ToFSharpList(), cells);

            Assert.AreEqual(true, newCells[0, 0]);
            Assert.AreEqual(true, newCells[0, 1]);
            Assert.AreEqual(true, newCells[0, 2]);
            
        }
    }
}
