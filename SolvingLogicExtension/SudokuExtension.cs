using SudokuSolver.SudokuModel;
using System.Linq;

namespace SudokuSolver.SolvingLogicExtension
{
    internal static class SudokuExtension
    {
        private static ushort _currentSudokuDepth = 0;
        private static List<ushort> _sudokuLevels = new List<ushort>() { 0 };
        private static bool SetValues(this Sudoku sudoku)
        {
            var casesToSet = sudoku.CaseToResolve.Where(c => c.MissingValues.Count() == 1);
            if (!casesToSet.Any())
                return false;
            while (casesToSet.Any())
            {
                var c = casesToSet.First();
                var value = c.MissingValues.Single();
                c.ResolveValue(value);
            }
            return true;
        }

        private static void SetValuesInLoop(this Sudoku sudoku)
        {
            bool @continue;
            do
                @continue = sudoku.SetValues();
            while (@continue);
        }

        private static bool IsValid(this Sudoku sudoku) => !sudoku.CaseToResolve.Any(c => !c.MissingValues.Any());
       
        private static Sudoku MakeHypothesis(this Sudoku sudoku)
        {
            Case @case = sudoku.CaseToResolve.First();
            var value = @case.MissingValues.First();
            sudoku.CurrentCaseHypothesis = new CaseHypothesis(@case, value);
            @case.Value = value;
            return new Sudoku(sudoku);
        }

        internal static Sudoku Resolve(this Sudoku sudoku)
        {
            sudoku.SetValuesInLoop();
            bool isValid = sudoku.IsValid();

            if (isValid && !sudoku.CaseToResolve.Any())
            {
                Console.WriteLine($"SudoKu resolu avec le shema suivant : \n {String.Join('-',_sudokuLevels)}");
                return sudoku;
            }
                

            if (isValid)
            {
                _currentSudokuDepth += 1;
                _sudokuLevels.Add(_currentSudokuDepth);
                sudoku = sudoku.MakeHypothesis();
                return sudoku.Resolve();
            }
            else
            {
                _currentSudokuDepth -= 1;
                _sudokuLevels.Add(_currentSudokuDepth);
                var parent = sudoku.Parent ?? throw new ApplicationException("SudokuExtension.Resolve Exception : Le sudoku n'est pas resolvable");
                var hypothesis = parent.CurrentCaseHypothesis ?? throw new ApplicationException("SudokuExtension.Resolve Exception");
                hypothesis.Case.ForbiddenValues.Add(hypothesis.HypothesisValue);
                hypothesis.Case.Value = 0;
                return parent.Resolve();
            }
        }
    }
}
