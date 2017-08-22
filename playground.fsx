#r "../../.nuget/packages/FsCheck/2.9.0/lib/netstandard1.6/FsCheck.dll"

open FsCheck
open Gen

let genInt = Arb.generate<int>
let genInts = Arb.generate<int list>

printfn "genInts 2 5: %A" (sample 2 5 genInts)
printfn "genInts 5 2: %A" (sample 5 2 genInts)

printfn "constant: %A" (sample 5 5 (constant 42))

printfn "choose: %A" (sample 5 5 (choose (1, 20)))

printfn "elements: %A" (sample 5 5 (elements [1..10]))

printfn "oneof: %A" (sample 5 5 (oneof [
                                  elements [1..10]
                                  elements [11..20]
                                  elements [21..30]
                                ]))

printfn "frequency: %A" (sample 5 10 (frequency [
                                        20, elements [1..10]
                                        30, elements [11..20]
                                        50, elements [21..30]
                                      ]))

printfn "listOf: %A" (sample 5 5 (choose (1, 10) |> Gen.listOf))

printfn "listOfLength: %A" (sample 5 5 (choose (1, 10) |> Gen.listOfLength 5))

printfn "shuffle: %A" (sample 5 5 (shuffle [1;2;3;4;5]))

printfn "where: %A" (sample 5 5 (genInt |> where (fun n -> n > 10)))

// label

// collect

// classify

// ==>

// example of too many discarded tests (e.g. list of odd numbers)

// ----------------------------------------------------------------------
// Functor
// ----------------------------------------------------------------------

let s1 = Seq.singleton 42 |> Seq.map string
printfn "%A" s1

let l1 = List.singleton 42 |> List.map string
printfn "%A" l1

let a1 = Array.singleton 42 |> Array.map string
printfn "%A" a1

let g1 = Gen.constant 42 |> Gen.map string
printfn "g1: %A" <| sample 0 1 g1

// ----------------------------------------------------------------------
// Applicative
// ----------------------------------------------------------------------

let fn n s1 s2 = String.length (sprintf "%d%s%s" n s1 s2) > 5

let gn = genInt
let gs1 = genInt |> map string
let gs2 = genInt |> map (fun n -> (string)(n * 10))

let g2 = map3 fn gn gs1 gs2
printfn "g2: %A" <| sample 5 10 g2

let g3 = gen {
  let! n = gn
  let! s1 = gs1
  let! s2 = gs2
  return fn n s1 s2
}
printfn "g3: %A" <| sample 5 10 g3

// Gen<(a -> b -> c -> d)> <*> Gen<a>
// Gen<(b -> c -> d)> <*> Gen<b>
// Gen<(c -> d)> <*> Gen<c>
// Gen<d>

let g4 = constant fn <*> gn <*> gs1 <*> gs2
printfn "g4: %A" <| sample 5 10 g4

// if r is (b -> c -> d)
// (a -> b -> c -> d)
// (a -> r)
// map (a -> r) Gen<a> => Gen<r>
// map (a -> b -> c -> d) Gen<a> => Gen<(b -> c -> d)>

let dummy1 = map fn gn

let g5 = map fn gn <*> gs1 <*> gs2
printfn "g5: %A" <| sample 5 10 g5

let dummy2 = fn <!> gn

let g6 = fn <!> gn <*> gs1 <*> gs2
printfn "g6: %A" <| sample 5 10 g6

// ----------------------------------------------------------------------
// Monad
// ----------------------------------------------------------------------

// Trying to implement Gen.oneof using 'map'.
let oneofv1 (gens: Gen<_> seq) =
  choose(0, Seq.length gens - 1)
  |> map (fun index -> Seq.item index gens)

let g7 = oneofv1 [constant 1; constant 2; constant 3]
printfn "g7: %A" <| sample 2 10 (g7 >>= id)

// Implementing Gen.oneof using '>>='.
let oneofv2 (gens: Gen<_> seq) =
  choose(0, Seq.length gens - 1)
  >>= (fun index -> Seq.item index gens)

let g8 = oneofv2 [constant 1; constant 2; constant 3]
printfn "g8: %A" <| sample 2 10 g8

// Implementing Gen.oneof using a 'gen' computation expression.
let oneofv3 (gens: Gen<_> seq) = gen {
  let! index = choose(0, Seq.length gens - 1)
  return! Seq.item index gens
}

let g9 = oneofv3 [constant 1; constant 2; constant 3]
printfn "g9: %A" <| sample 2 10 g9
