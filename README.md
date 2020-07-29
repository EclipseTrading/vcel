# VCEL

VCEL is short for View Client Expression Language. Its a simple expression language designed as a replacement for [SPEL](https://github.com/spring-projects/spring-net/tree/master/src/Spring/Spring.Core/Expressions) used in an internal project called ViewClient. 

The main thing that differentiates this project from SPEL (and many other expression languages) is that it gives us control over how things are composed in the expression using a Monad pattern. This allows things like gracefully handling null references without exceptions or handling asynchronous operations whilst also keeping the expression simple and intuitive to the user, and abstracting away such concerns.

VCEL currently supports a C# target, but longer term goals include a typescript target also.
