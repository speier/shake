#### MSBuild task
Using msbuild command line.
Supports targets and dynamic properties.

#### Example
{code:c#}
dynamic msb = new MsBuildTask("foo.sln");
msb.Targets.Add("Rebuild");
msb.Properties.Configuration = "Release";
msb.Properties.OutputPath = Path.Combine(shake.Project.Directory, "build");
msb.Build();
{code:c#}