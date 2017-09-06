#### AssemblyInfo task
Load/Parse, Create and Save AssemblyInfo files.

#### Example
{code:c#}
var asmInfo = new AssemblyInfoTask(@"c:\temp\project\properties"); // filename is optional, default is "AssemblyInfo.cs"
asmInfo.AssemblyVersion = new Version("1.2.3.4");
asmInfo.Save();
{code:c#}