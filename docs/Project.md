#### Project service
Supports project-file level operations.

#### Properties
{code:c#}
public string Directory { get; }
{code:c#}

#### Methods
{code:c#}
public string DirectoryCombine(string dir)
{code:c#}

#### Example
{code:c#}
var projFileDir = shake.Project.Directory; // NAnt: ${project::get-base-directory()}

var docDir = shake.Project.DirectoryCombine("doc"); // relative to project's dir
{code:c#}