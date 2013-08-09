using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Lib
{
    public class Cell
    {
        public Cell()
        {
            IsAlive = false;
        }

        public bool IsAlive { get; set; }

        public void Toggle()
        {
            IsAlive = !IsAlive;
        }

        public override string ToString()
        {
            return IsAlive ? "x" : "-";
        }
    }
}
