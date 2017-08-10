#r "/Users/jontaylor/.nuget/packages/FsCheck/2.9.0/lib/netstandard1.6/FsCheck.dll"

open FsCheck

let genInt = Arb.generate<int list>

printfn "%A" (Gen.sample 2 5 genInt)
printfn "%A" (Gen.sample 5 2 genInt)

printfn "%A" (Gen.sample 5 5 (Gen.constant 42))
printfn "%A" (Gen.sample 5 5 (Gen.choose (1, 20)))
printfn "%A" (Gen.sample 5 5 (Gen.elements [1..10]))
printfn "%A" (Gen.sample 5 5 (Gen.oneof [
                                Gen.elements [1..10]
                                Gen.elements [11..20]
                                Gen.elements [21..30]
                            ]))
printfn "%A" (Gen.sample 5 10 (Gen.frequency [
                                20, Gen.elements [1..10]
                                30, Gen.elements [11..20]
                                50, Gen.elements [21..30]
                            ]))

// TODO: cover these QuickCheck things (or their nearest equivalent in FsCheck):
//
// listOf
// suchThat
// label
// collect
// classify
// ==>

// Given a Gen<'a> return a Gen<'b> by lifting a function of 'a -> 'b
let map f g = Gen.map f g

let flatMap f g = gen.Bind (g, f)

// TODO: find a better example
let g1 = Gen.constant 42 |> map Gen.constant
printfn "%A" <| Gen.sample 2 2 g1

let g2 = Gen.constant 42 |> flatMap Gen.constant
printfn "%A" <| Gen.sample 2 2 g2
