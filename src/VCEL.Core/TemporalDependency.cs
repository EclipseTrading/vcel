namespace VCEL;

public class TemporalDependency : IDependency
{
    private TemporalDependency(Granularity granularity)
    {
        Granularity = granularity;
    }
    public Granularity Granularity { get; }
    public string Name => Granularity.ToString();

    public static TemporalDependency Now { get; } = new TemporalDependency(Granularity.Now);
    public static TemporalDependency Today { get; } = new TemporalDependency(Granularity.Today);
}