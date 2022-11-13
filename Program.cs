using SudokuSolver;
using SudokuSolver.SolvingLogicExtension;
using SudokuSolver.SudokuModel;

var numbers = new ushort[] {
    2, 7, 0,  0, 9, 0,  0, 1, 6,
    0, 0, 6,  0, 0, 0,  8, 0, 0,
    0, 0, 0,  1, 0, 0,  3, 0, 0,
    0, 9, 0,  0, 0, 5,  0, 0, 0,
    0, 0, 0,  0, 0, 0,  6, 0, 0,
    0, 0, 7,  8, 0, 0,  0, 4, 2,
    4, 0, 0,  0, 8, 0,  0, 0, 0,
    0, 0, 2,  4, 0, 0,  0, 7, 1,
    0, 0, 0,  0, 0, 0,  0, 3, 0};

var sudo = new Sudoku(numbers);
var solution = sudo.Resolve();
foreach (var (value,index) in solution.Cases.Select((c,index)=>(c.Value,index)))
{
    if(index%9==0)
        Console.WriteLine();
    Console.Write(value);
}
