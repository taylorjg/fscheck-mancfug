module TheoryExample

open Xunit
open FsCheck
open FsCheck.Xunit

[<Theory>]
[<InlineData(1, 1)>]
[<InlineData(21, 21)>]
[<InlineData(3, 4)>]
[<InlineData(4, 3)>]
[<InlineData(10, 0)>]
[<InlineData(0, 10)>]
let additionCommutative (a: int) (b: int) =
    Assert.Equal(a + b, b + a)

[<Fact>]
let ``prop_additionCommutative``() =
  Check.QuickThrowOnFailure <| fun (a: int) (b: int) ->
    a + b = b + a

[<Property>]
let ``prop_additionCommutative2``(a: int) (b: int) =
  a + b = b + a
