public class Targets
{
    public void Default()
    {
        var anonym = new { who = "Shake", how = "uses Mono's C# compiler as a service", when = DateTime.Now };

        Console.WriteLine("{0} {1}, right now. ({2})", anonym.who, anonym.how, anonym.when);
    }
}