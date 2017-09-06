#### File task
Supports copying files and directories, remove and create directory.

#### Methods
{code:c#}
public static void RemoveDir(string path)
public static void CreateDir(string path)
public static void ReCreateDir(string path)
public static void CopyDir(string sourcePath, string destPath, string[]()() excludePatterns = null, string[]()() includePatterns = null)
{code:c#}

#### Example
{code:c#}
FileTask.CreateDir(@"c:\test");

FileTask.CopyDir(shake.Project.Directory, @"c:\test", null, new[]() { "LICENSE.txt", "README.txt" });
{code:c#}