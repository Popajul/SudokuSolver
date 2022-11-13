namespace SudokuSolver.SudokuModel
{
    internal class CaseHypothesis
    {
        internal Case Case { get; set; }
        internal ushort HypothesisValue { get; set; }
        internal CaseHypothesis(Case @case, ushort hypothesisValue)
        {
            Case = @case;
            HypothesisValue = hypothesisValue;
        }   
    }
}
