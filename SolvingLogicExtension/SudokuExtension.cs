using SudokuSolver.SudokuModel;

namespace SudokuSolver.SolvingLogicExtension
{
    internal static class SudokuExtension
    {
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

        private static bool IsValid(this Sudoku sudoku)
        {
            // un chiffre est présent plusieurs fois dans un même cluster
            var badCondition1 = sudoku.Cases
                 .SelectMany(c => c.Clusters).Distinct()
                 .Select(clu => clu.Cases.Where(c => c.Resolved)
                 .Select(c => c.Value))
                 .Any(e => e.Distinct().Count() < e.Count());

            // Une case non résolue ne contient aucune valeur potentielle
            var badCondition2 = sudoku.CaseToResolve.Any(c => !c.MissingValues.Any());

            if(badCondition1 || badCondition2)
                return false;
            return true;
        }

        private static Sudoku MakeHypothesis(this Sudoku sudoku)
        {
            Case @case = sudoku.CaseToResolve.MinBy(c => c.MissingValues.Count())?? throw new ApplicationException("SudokuExtension.MakeHypothesis Exception");
            var value = @case.MissingValues.First();
            sudoku.CurrentCaseHypothesis = new CaseHypothesis(@case, value);
            @case.Value = value;
            return new Sudoku(sudoku);
        }

        internal static Sudoku Resolve(this Sudoku sudoku)
        {
            sudoku.SetValuesInLoop();
            bool isValid = sudoku.IsValid();

            if(isValid && !sudoku.CaseToResolve.Any())
                return sudoku;

            if (isValid)
            {
                sudoku = sudoku.MakeHypothesis();
                return sudoku.Resolve();
            }
            else
            {
                var parent = sudoku.Parent ?? throw new ApplicationException("SudokuExtension.Resolve Exception : Le sudoku n'est pas resolvable");
                var hypothesis = parent.CurrentCaseHypothesis ?? throw new ApplicationException("SudokuExtension.Resolve Exception");
                hypothesis.Case.ForbiddenValues.Add(hypothesis.HypothesisValue);
                hypothesis.Case.Value = 0;
                return parent.Resolve();
            }
        }
    }
}
