## Description

I've been asked to give a talk on [FsCheck](https://fscheck.github.io/FsCheck/) at the next [Manchester F# User Group](https://www.meetup.com/Manchester-F-User-Group/) meetup (22nd August 2017). This repo contains some example code.

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
* [QuickCheck](https://hackage.haskell.org/package/QuickCheck)
* [QuickCheck: An Automatic Testing Tool for Haskell](http://www.cse.chalmers.se/~rjmh/QuickCheck/manual.html)
* [The Design and Use of QuickCheck](https://begriffs.com/posts/2017-01-14-design-use-quickcheck.html)
* [Chapter 11 of Real World Haskell](http://book.realworldhaskell.org/read/testing-and-quality-assurance.html)
* [ScalaCheck: The Definitive Guide](http://booksites.artima.com/scalacheck)
* [Fuzz testing distributed systems with QuickCheck](https://making.pusher.com/fuzz-testing-distributed-systems-with-quickcheck/)
* [Practical testing in Haskell](https://jaspervdj.be/posts/2015-03-13-practical-testing-in-haskell.html)
