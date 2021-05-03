# VCEL

VCEL is short for View Client Expression Language. Its a simple expression language designed as a replacement for [SPEL](https://github.com/spring-projects/spring-net/tree/master/src/Spring/Spring.Core/Expressions) used in an internal project called ViewClient.

The main thing that differentiates this project from SPEL (and many other expression languages) is that it gives us control over how things are composed in the expression using a Monad pattern. This allows things like gracefully handling null references without exceptions or handling asynchronous operations whilst also keeping the expression simple and intuitive to the user, and abstracting away such concerns.

VCEL currently supports C# and JavaScript runtimes.

## CLI

A simple CLI tool is also included as a way to execute and test VCEL expressions easily from the command line.

This can be installed using the [.NET tool command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install) which downloads the latest package version from NuGet.

`dotnet tool install --global VCEL.Cli`

The application `vcel` will now be available as a global command.
