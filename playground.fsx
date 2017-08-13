#r "../../.nuget/packages/FsCheck/2.9.0/lib/netstandard1.6/FsCheck.dll"

open FsCheck

let genInts = Arb.generate<int list>

printfn "%A" (Gen.sample 2 5 genInts)
printfn "%A" (Gen.sample 5 2 genInts)

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
// example of too many discarded tests (e.g. list of odd numbers)

// ----------------------------------------------------------------------
// Functor
// ----------------------------------------------------------------------

// Given a Gen<'a> return a Gen<'b> by lifting a function of 'a -> 'b
// let map f g = Gen.map f g

let g1 = Gen.constant 42 |> Gen.map (fun n -> n + 1)
printfn "%A" <| Gen.sample 2 2 g1

// let g1 = Gen.constant 42 |> Gen.map Gen.constant
// printfn "%A" <| Gen.sample 2 2 g1

// ----------------------------------------------------------------------
// Monad
// ----------------------------------------------------------------------

let flatMap f g = g >>= f
// let flatMap f g = gen.Bind (g, f)

// TODO: find a better example
let g2 = Gen.constant 42 |> flatMap Gen.constant
printfn "%A" <| Gen.sample 2 2 g2

let g3 = Gen.constant 42 >>= Gen.constant
printfn "%A" <| Gen.sample 2 2 g3

// ----------------------------------------------------------------------
// Applicative
// ----------------------------------------------------------------------

let fn n s1 s2 = String.length (sprintf "%d%s%s" n s1 s2) > 4

let gn = Arb.generate<int>
let gs1 = gn |> Gen.map (string)
let gs2 = gn |> Gen.map (string)

let g4 = Gen.map3 fn gn gs1 gs2
printfn "%A" <| Gen.sample 5 10 g4

let g5 = gn >>= (fun n ->
    gs1 >>= (fun s1 ->
        gs2 |> Gen.map (fun s2 ->
            String.length (sprintf "%d%s%s" n s1 s2) > 4)))
printfn "%A" <| Gen.sample 5 10 g5

let g6 = fn <!> gn <*> gs1 <*> gs2
printfn "%A" <| Gen.sample 5 10 g6
