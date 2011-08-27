public class Targets
{
    public void Default()
    {
        var v = new Version(0, 1, shake.CommandLine.Properties.BuildNumber ?? 9);
        Console.WriteLine("{0} v{1}", shake.CommandLine.Properties.Title ?? "Shake", v);
    }
}