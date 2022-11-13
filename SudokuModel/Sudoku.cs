namespace SudokuSolver.SudokuModel
{
    internal class Sudoku
    {
        private readonly ushort[] _numbers = new ushort[9] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        internal Case[] Cases { get; set; }
        internal IEnumerable<Case> CaseToResolve{get;set;}
        internal CaseHypothesis? CurrentCaseHypothesis { get; set; }
        internal Sudoku? Parent { get; set; }
        internal Sudoku(ushort[] numbers)
        {
            var lines = new Cluster[9];
            var columns = new Cluster[9];
            var squares = new Cluster[9];
            for (int i = 0; i < 9; i++)
            {
                lines[i] = new Cluster()
                {
                    Cases = new Case[9]
                };
                columns[i] = new Cluster()
                {
                    Cases = new Case[9]
                };
                squares[i] = new Cluster()
                {
                    Cases = new Case[9]
                };
            }
            var cases = numbers.Select((u, index) => new { Case = new Case(u) , index }).ToArray();

            foreach (var c in cases)
            {
                var lineIndex = c.index / 9;
                var columnIndex = c.index % 9;
                lines[lineIndex].Cases[columnIndex] = c.Case;
                columns[columnIndex].Cases[lineIndex] = c.Case;

                var squareIndex = (lineIndex switch
                {
                    < 3 => new List<int>() { 0, 1, 2 },
                    < 6 => new List<int>() { 3, 4, 5 },
                    _ => new List<int>() { 6, 7, 8 }

                }).Intersect(columnIndex switch
                {
                    < 3 => new List<int>() { 0, 3, 6 },
                    < 6 => new List<int>() { 1, 4, 7 },
                    _ => new List<int>() { 2, 5, 8 }

                }).Single();

                var indexInSquare = ((lineIndex % 3) switch
                {
                    0 => new List<int>() { 0, 1, 2 },
                    1 => new List<int>() { 3, 4, 5 },
                    _ => new List<int>() { 6, 7, 8 },
                }).Intersect((columnIndex % 3) switch
                {
                    0 => new List<int>() { 0, 3, 6 },
                    1 => new List<int>() { 1, 4, 7 },
                    _ => new List<int>() { 2, 5, 8 },
                }).Single();
                squares[squareIndex].Cases[indexInSquare] = c.Case;

                c.Case.Clusters = new Cluster[3] { lines[lineIndex], columns[columnIndex], squares[squareIndex] };
            }

            Cases = cases.Select(c=>c.Case).ToArray();
            CaseToResolve = Cases.Where(c => !c.Resolved);
            foreach(var @case in Cases)
            {
                @case.MissingValues = @case.Value != 0 ? Enumerable.Empty<ushort>() : _numbers.Except(@case.Clusters.SelectMany(clu => clu.Cases, (clu, ca) => ca.Value)).Except(@case.ForbiddenValues);

            }
        }

        internal Sudoku(Sudoku sudoku) : this(sudoku.Cases.Select(c => c.Value).ToArray())
        {
            Parent = sudoku;           
        }
    }

}
