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
// Custom generators
////////////////////////////////////////////////////////////////////////////////

let genPoint =
  let genNormalFloat = Arb.Default.NormalFloat().Generator
  gen {
    let! x = genNormalFloat
    let! y = genNormalFloat
    return { x = float x; y = float y }
  }

type CustomArbitraries =
    static member Point() = Arb.fromGen genPoint

////////////////////////////////////////////////////////////////////////////////
// Property test
////////////////////////////////////////////////////////////////////////////////

[<Properties(Arbitrary=[| typeof<CustomArbitraries> |])>]
module EuclideanDistanceWithProperties = 

  let tolerance = 0.001

  let (=~) a b = abs(a - b) < tolerance

  [<Property>]
  let propNonNegativity x y =
    distance x y >= 0.0

  [<Property>]
  let propIdentity x =
    distance x x =~ 0.0

  [<Property>]
  let propSymmetry x y =
    distance x y =~ distance y x

  [<Property>]
  let propTriangleInequality x y z =
    distance x z <= distance x y + distance y z
