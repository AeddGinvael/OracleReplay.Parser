namespace Oracle.Packets.Model
{
    public class Build
    {
        public int BuildNumber { get; set; }
        public int Tick { get; set;  }
    }
    public class BuildNumberRange
    {
        public int? Start { get; }
        public int? End { get; }

        public BuildNumberRange(int? start, int? end)
        {
            Start = start;
            End = end;
        }

        public bool AppliesTo(int buildNumber)
        {
            if (Start == null && End == null) return true;
            return buildNumber != -1 && (Start == null || Start <= buildNumber) && (End == null || End >= buildNumber);
        }

        public override string ToString()
        {
            return $"({Start}, {End})";
        }
    }
}
