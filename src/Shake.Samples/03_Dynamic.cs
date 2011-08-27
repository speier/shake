public class Targets
{
    public void Default()
    {
        dynamic project = new ExpandoObject();
        project.Name = "Shake";
        project.Desc = "simple build tool";
        project.Like = "Rake";

        Console.WriteLine("{0} is a {1}, inspired by {2}.", project.Name, project.Desc, project.Like);
    }
}