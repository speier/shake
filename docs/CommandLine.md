#### CommandLine service
Access command line properties as dynamic and execute commands.

#### Examples
{code:c#}
public class Targets
{
    public void Default()
    {
        var v = new Version(0, 1, shake.CommandLine.Properties.BuildNumber ?? 9);
        Console.WriteLine("{0} v{1}", shake.CommandLine.Properties.Title ?? "Shake", v);
    }
}
{code:c#}

{code:c#}
shake.CommandLine.Exec("dir", @"c:\windows\system32");
{code:c#}