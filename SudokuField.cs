using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory1
{
    public class SudokuField
    {
        public int row, col;
        public List<int> availableMoves;
        public SudokuField(int row, int col, List<int>availableMoves)
        {
            this.row = row;
            this.col = col;
            this.availableMoves = availableMoves;
        }
    }
}
