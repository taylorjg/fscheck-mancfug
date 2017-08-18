module EuclideanDistance

open FsCheck
open FsCheck.Xunit

// https://en.wikipedia.org/w/index.php?title=Distance_function

////////////////////////////////////////////////////////////////////////////////
// Implementation code
////////////////////////////////////////////////////////////////////////////////

type Point = {
    x: float
    y: float
}

let distance p1 p2 = sqrt (pown (p2.x - p1.x) 2 + pown (p2.y - p1.y) 2)

////////////////////////////////////////////////////////////////////////////////
// Overrides
////////////////////////////////////////////////////////////////////////////////

type NonNanOrInfiniteFloat =
  static member NonNanOrInfiniteFloat() =
    Arb.Default.Float()
      |> Arb.filter (fun f ->
        not <| System.Double.IsNaN(f) &&
        not <| System.Double.IsInfinity(f))

////////////////////////////////////////////////////////////////////////////////
// Property test
////////////////////////////////////////////////////////////////////////////////

[<Property(Arbitrary = [| typeof<NonNanOrInfiniteFloat> |])>]
let propNonNegativity x y =
  distance x y >= 0.0

[<Property(Arbitrary = [| typeof<NonNanOrInfiniteFloat> |])>]
let propIdentity x =
  distance x x = 0.0

[<Property(Arbitrary = [| typeof<NonNanOrInfiniteFloat> |])>]
let propSymmetry x y =
  distance x y = distance y x

[<Property(Arbitrary = [| typeof<NonNanOrInfiniteFloat> |])>]
let propTriangleInequality x y z =
  distance x z <= distance x y + distance y z
