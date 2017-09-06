#### Subversion (SVN) task
Using svn command line client.
Supports checkout, update, commit.

#### Properties
{code:c#}
string Username { get; set; }
string Password { get; set; }
{code:c#}

#### Methods
{code:c#}
public void Checkout(string path, string url)
public void Update(string path)
public void Add(string path)
public void Commit(string path, string message)
{code:c#}

#### Example
{code:c#}
var svnPath = @"c:\temp\";

var svnTask = new SvnTask();
svnTask.Checkout(svnPath, "https://foo.svn.codeplex.com/svn");
// or
svnTask.Update(svnPath);
{code:c#}