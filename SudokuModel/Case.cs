namespace SudokuSolver.SudokuModel
{
    internal class Case
    {
        internal ushort Value { get; set; }
        internal Cluster[] Clusters { get; set; }
        internal IEnumerable<ushort> MissingValues { get; set; }
        internal bool Resolved { get; set; }
        internal List<ushort> ForbiddenValues { get; set; }


        internal Case(ushort value)
        {
            Value = value;
            Resolved = value != 0;
            ForbiddenValues = new List<ushort>();
        }
    }
}
