#### What is Shake?
Shake is a simple build tool (like Rake or FAKE and others with internal DSLs).

#### How it works?
Shake uses [Mono's C# compiler as a service](http://tirania.org/blog/archive/2010/Apr-27.html), allowing the use of the C# syntax in your Shakefiles (equivalent of Makefiles in make).

#### Why Shake?
Because C# is more readable than XML.

Quick sample:
{code:c#}
dynamic msb = new MsBuildTask("c:\foo\foo.sln");
msb.Properties.OutputPath = "c:\foo\build";
msb.Build();
{code:c#}

#### Features
Shake has builtin [Tasks](Tasks) and it's own [API](API) with [Services](Services).

#### Getting started
See the [Documentation](Documentation)

_Currently I am working on the project’s core, documentation and more tasks._
_For more recent information check out my blog at_ [http://blog.kalmanspeier.com](http://blog.kalmanspeier.com)