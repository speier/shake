/* shakefile to build shake :) */

using ICSharpCode.SharpZipLib.Zip;

public class Targets
{
    string Solution = @"src\shake\shake.csproj";
    string AsmInfoDir = @"src\shake\properties";
    string BuildDir = Path.Combine(shake.Project.Directory, "build");
    string DeployDir = @"c:\build\shake";

    private Version AsmVer;

    public void Default()
    {
        Build();
        Deploy();
    }

    public void Build()
    {
        var asmInfo = new AssemblyInfoTask(AsmInfoDir);
        asmInfo.SetBuildNumber(shake.CommandLine.Properties.BuildNumber ?? 0);
        asmInfo.Save();

        AsmVer = asmInfo.AssemblyVersion;
        Console.WriteLine("Building {0} ...", AsmVer);

        dynamic msb = new MsBuildTask(Solution);
        msb.Properties.OutputPath = BuildDir;
        msb.Properties.Configuration = shake.CommandLine.Properties.Configuration;
        msb.Properties.WarningLevel = 0;
        msb.Properties.DebugType = "none";
        msb.Targets.Add("Rebuild");
        msb.Build();
    }

    public void Deploy()
    {
        FileTask.CreateDir(DeployDir);

        FileTask.CopyDir(shake.Project.Directory, BuildDir, null, new[] { "LICENSE.txt", "README.txt" });
        FileTask.CopyDir(shake.Project.DirectoryCombine("doc"), Path.Combine(BuildDir, "Documentation"));
        FileTask.CopyDir(shake.Project.DirectoryCombine(@"src\Shake.Samples"), Path.Combine(BuildDir, "Samples"));

        var zipName = string.Format("shake-{0}-{1}-{2}-bin.zip", AsmVer.Major, AsmVer.Minor, AsmVer.Build);
        var zip = new FastZip();
        zip.CreateZip(Path.Combine(DeployDir, zipName), BuildDir, true, "");

        FileTask.RemoveDir(BuildDir);
    }
}