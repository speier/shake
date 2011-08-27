## Shake - C# Make

Shake is a simple build tool (like Rake or FAKE and others with internal DSLs).
But with a twist Shake uses Mono's C# compiler as a service, allowing the use of the C# syntax in your Shakefiles (equivalent of Makefiles in make).

Tasks

* MSBuild task (using msbuild command line, supports targets and dynamic properties)
* SVN task (using svn command line client, supports checkout, update, commit)
* AssemblyInfo task (load/parse, create and save AssemblyInfo file)
* Basic file manipulation tasks (copy, create and remove directories, etc.)

Services (API)

* Command line service
 * access command line properties
 * start new command line process

Requirements

* .NET 4 Client Profile

(c) 2010 Kalman Speier