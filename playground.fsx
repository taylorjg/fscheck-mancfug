#r "../../.nuget/packages/FsCheck/2.9.0/lib/netstandard1.6/FsCheck.dll"

open FsCheck
open Gen

let genInt = Arb.generate<int>
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

printfn "%A" (sample 5 5 (genInt |> where (fun n -> n > 10)))

// label

// collect

// classify

// ==>

// example of too many discarded tests (e.g. list of odd numbers)

// ----------------------------------------------------------------------
// Functor
// ----------------------------------------------------------------------

let g1 = constant 42 |> map (fun n -> n + 1)
printfn "%A" <| sample 2 2 g1

// ----------------------------------------------------------------------
// Monad
// ----------------------------------------------------------------------

// Trying to implement Gen.oneof using 'map'.
let oneofv1 (gens: Gen<_> seq) =
  choose(0, Seq.length gens - 1)
  |> map (fun index -> Seq.item index gens)

// Implementing Gen.oneof using 'flatMap'.
let oneofv2 (gens: Gen<_> seq) =
  choose(0, Seq.length gens - 1)
  >>= (fun index -> Seq.item index gens)

let g2 = oneofv2 [constant 1; constant 2; constant 3]
printfn "%A" <| sample 2 10 g2

// ----------------------------------------------------------------------
// Applicative
// ----------------------------------------------------------------------

let fn n s1 s2 = String.length (sprintf "%d%s%s" n s1 s2) > 5

let gn = Arb.generate<int>
let gs1 = gn |> map (string)
let gs2 = gn |> map (fun n -> (string) <| n * 10)

let g3 = map3 fn gn gs1 gs2
printfn "%A" <| sample 5 10 g3

let g4 = gen {
  let! n = gn
  let! s1 = gs1
  let! s2 = gs2
  return fn n s1 s2
}
printfn "%A" <| sample 5 10 g4

// Gen<(a -> b -> c -> d)> <*> Gen<a>
// Gen<(b -> c -> d)> <*> Gen<b>
// Gen<(c -> d)> <*> Gen<c>
// Gen<(d)>

let g5 = constant fn <*> gn <*> gs1 <*> gs2
printfn "%A" <| sample 5 10 g5

// if r is (b -> c -> d)
// (a -> b -> c -> d)
// (a -> r)
// map (a -> r) Gen<a> => Gen<r>
// map (a -> b -> c -> d) Gen<a> => Gen<(b -> c -> d)>

let dummy1 = map fn gn

let g6 = map fn gn <*> gs1 <*> gs2
printfn "%A" <| sample 5 10 g6

let dummy2 = fn <!> gn

let g7 = fn <!> gn <*> gs1 <*> gs2
printfn "%A" <| sample 5 10 g7
