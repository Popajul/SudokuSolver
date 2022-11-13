using SudokuSolver;
using SudokuSolver.SolvingLogicExtension;
using SudokuSolver.SudokuModel;
using System.Diagnostics;

var numbers = new ushort[] {
    0, 1, 2,  7, 0, 0,  0, 0, 0,
    0, 7, 0,  6, 0, 8,  0, 0, 0,
    0, 0, 0,  0, 0, 0,  8, 3, 0,
    0, 0, 8,  0, 0, 7,  0, 0, 0,
    0, 0, 0,  0, 0, 0,  3, 5, 0,
    0, 0, 6,  0, 0, 4,  0, 0, 0,
    2, 4, 0,  0, 0, 0,  0, 0, 0,
    0, 0, 0,  0, 5, 0,  0, 1, 6,
    1, 0, 0,  0, 8, 0,  9, 0, 0};

var sw = new Stopwatch();
sw.Start();
var sudo = new Sudoku(numbers);
var solution = sudo.Resolve();
sw.Stop();
Console.WriteLine($"SUDOKU RESOLU EN {sw.Elapsed.TotalMilliseconds} ms");
foreach (var (value,index) in solution.Cases.Select((c,index)=>(c.Value,index)))
{
    if(index%9==0)
        Console.WriteLine();
    Console.Write(value);
}
