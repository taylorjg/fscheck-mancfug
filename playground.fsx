#r "../../.nuget/packages/FsCheck/2.9.0/lib/netstandard1.6/FsCheck.dll"

open FsCheck
open Gen

let genInts = Arb.generate<int list>

printfn "%A" (sample 2 5 genInts)
printfn "%A" (sample 5 2 genInts)

printfn "%A" (sample 5 5 (constant 42))
printfn "%A" (sample 5 5 (choose (1, 20)))
printfn "%A" (sample 5 5 (elements [1..10]))
printfn "%A" (sample 5 5 (oneof [
                                elements [1..10]
                                elements [11..20]
                                elements [21..30]
                            ]))
printfn "%A" (sample 5 10 (frequency [
                                20, elements [1..10]
                                30, elements [11..20]
                                50, elements [21..30]
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
// let map f g = map f g

let g1 = constant 42 |> map (fun n -> n + 1)
printfn "%A" <| sample 2 2 g1

// let g1 = constant 42 |> map constant
// printfn "%A" <| sample 2 2 g1

// ----------------------------------------------------------------------
// Monad
// ----------------------------------------------------------------------

let flatMap f g = g >>= f
// let flatMap f g = gen.Bind (g, f)

// TODO: find a better example
let g2 = constant 42 |> flatMap constant
printfn "%A" <| sample 2 2 g2

let g3 = constant 42 >>= constant
printfn "%A" <| sample 2 2 g3

// ----------------------------------------------------------------------
// Applicative
// ----------------------------------------------------------------------

let fn n s1 s2 = String.length (sprintf "%d%s%s" n s1 s2) > 5

let gn = Arb.generate<int>
let gs1 = gn |> map (string)
let gs2 = gn |> map (fun n -> (string) <| n * 10)

let g4 = map3 fn gn gs1 gs2
printfn "%A" <| sample 5 10 g4

let g5 =
  gn >>= (fun n ->
    gs1 >>= (fun s1 ->
      gs2 |> map (fun s2 ->
        fn n s1 s2)))
printfn "%A" <| sample 5 10 g5

let g6 = gen {
  let! n = gn
  let! s1 = gs1
  let! s2 = gs2
  return fn n s1 s2
}
printfn "%A" <| sample 5 10 g6

let g7 = fn <!> gn <*> gs1 <*> gs2
printfn "%A" <| sample 5 10 g7
