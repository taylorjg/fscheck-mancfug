module TheoryExample

open Xunit

[<Theory>]
[<InlineData(1, 1, 2)>]
[<InlineData(21, 21, 42)>]
[<InlineData(3, 4, 7)>]
[<InlineData(4, 3, 7)>]
[<InlineData(10, 0, 10)>]
[<InlineData(0, 10, 10)>]
let additionTest (a: int, b: int, c: int) = Assert.Equal(c, a + b)
