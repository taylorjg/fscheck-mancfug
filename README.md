## Description

I've been asked to give a talk on [FsCheck](https://fscheck.github.io/FsCheck/) at the next [Manchester F# User Group](https://www.meetup.com/Manchester-F-User-Group/) meetup (August 2017). This repo contains some example code.

## Project creation

I used the following commands to create a .NET Core `xUnit Test Project` in `F#` with `FsCheck`:

```sh
mkdir fscheck-mancfug
cd fscheck-mancfug
dotnet new xunit -lang f#
dotnet add package FsCheck
dotnet restore
dotnet test
```

## Links

* [FsCheck](https://fscheck.github.io/FsCheck/)
* [Test.QuickCheck package documentation](https://hackage.haskell.org/package/QuickCheck-2.10.0.1/docs/Test-QuickCheck.html)
* [QuickCheck: An Automatic Testing Tool for Haskell](http://www.cse.chalmers.se/~rjmh/QuickCheck/manual.html)
* [Chapter 11 of Real World Haskell](http://book.realworldhaskell.org/read/testing-and-quality-assurance.html)
* [ScalaCheck: The Definitive Guide](http://booksites.artima.com/scalacheck)
