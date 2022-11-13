using SudokuSolver.SudokuModel;

namespace SudokuSolver.SolvingLogicExtension
{
    internal static class CaseExtension
    {
        internal static void ResolveValue(this Case ca, ushort value)
        {
            ca.Value = value;
            ca.MissingValues = Enumerable.Empty<ushort>();
            ca.Resolved = true;
        }
    }
}
