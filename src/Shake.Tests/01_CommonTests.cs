// using ICSharpCode.SharpZipLib.Zip;

public class Helper
{
    public static string GetSomeText()
    {
        return "Hello from a helper class!";
    }
}

public class Targets
{
    public Targets()
    {
        Console.WriteLine(Helper.GetSomeText());
        //Default_();
    }

    public void Default()
    {
        //RefAssembly();
        //TestServices1();
        TestServices2();
        TestDynamic();
        TestAnon();
    }
    
    public void RefAssembly()
    {
        // var fZip = new FastZip();
        // fZip.CreateZip(@"D:\sharp.zip", @"D:\Development\Speier\Shake\lib", true, "");
    }

    public void TestServices1()
    {
        string bd = Path.Combine(shake.Project.Directory, "build");
        Console.WriteLine(bd);

        // NAnt: ${project::get-base-directory()}
        Console.WriteLine(shake.Project.Directory);

        var x = shake.CommandLine.Properties.buildNumber;
        Console.WriteLine(x ?? "empty");

        shake.CommandLine.Exec("dir", @"c:\windows\system32");
    }

    public void TestServices2()
    {
        var p = shake.CommandLine.Properties;
        var b = p.BuildNumber;
        var v = new Version(0, 1, b ?? 0);
        Console.WriteLine("v{0}", v);
        Console.WriteLine("{0} v{1}", shake.CommandLine.Properties.Title ?? "TestApp", v);
    }

    public void TestDynamic()
    {
        dynamic d = new ExpandoObject();
        d.DynProp = "Hello Dynamic!\n";
        Console.WriteLine(d.DynProp);

        dynamic msb = new MsBuildTask("dummy.sln");
        msb.Targets.Add("Rebuild");
        msb.Properties.Configuration = "Release";
        msb.Properties.WarningLevel = 0;
        msb.Properties.OutPUtPaTh = @"c:\build";
        // msb.Build();

        Console.WriteLine(msb);

        Console.WriteLine(msb.Properties.outputPath);
    }

    public void TestAnon()
    {
        var anontype = new { param1 = ".net", param2 = 4 };
        Console.WriteLine("{0} {1}", anontype.param1, anontype.param2);
    }
}