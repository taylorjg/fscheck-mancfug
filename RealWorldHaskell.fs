module RealWorldHaskell

open System
open Xunit
open FsCheck

// http://book.realworldhaskell.org/read/testing-and-quality-assurance.html

////////////////////////////////////////////////////////////////////////////////
// Implementation code
////////////////////////////////////////////////////////////////////////////////

let rec qsort = function
  | x :: xs ->
    let lt a = a < x
    let gte a = a >= x
    let lhs = List.filter lt xs
    let rhs = List.filter gte xs
    qsort lhs @ List.singleton x @ qsort rhs
  | _ ->
    []

////////////////////////////////////////////////////////////////////////////////
// Prelude functions
////////////////////////////////////////////////////////////////////////////////

let minimum = List.reduce min

let maximum = List.reduce max

////////////////////////////////////////////////////////////////////////////////
// Property test
////////////////////////////////////////////////////////////////////////////////

[<Fact>]
let ``prop_idempotent``() =
  Check.QuickThrowOnFailure <| fun (xs: int list) ->
    qsort (qsort xs) = qsort xs
  
[<Fact>]
let ``prop_minimum``() =
  Check.QuickThrowOnFailure <| fun (xs: int list) ->
    not (List.isEmpty xs) ==>
      lazy (List.head (qsort xs) = minimum xs)

[<Fact>]
let ``prop_maximum``() =
  Check.QuickThrowOnFailure <| fun (xs: int list) ->
    not (List.isEmpty xs) ==>
      lazy (List.last (qsort xs) = maximum xs)

[<Fact>]
let ``prop_append``() =
  Check.QuickThrowOnFailure <| fun (xs: int list) (ys: int list) ->
    (not (List.isEmpty xs) && not (List.isEmpty ys)) ==>
      lazy (List.head (qsort (xs @ ys)) = min (minimum xs) (minimum ys))

[<Fact>]
let ``prop_permutation``() =
  Check.QuickThrowOnFailure <| fun (xs: int list) ->
    let permutation xs ys =
      List.isEmpty (List.except xs ys) &&
      List.isEmpty (List.except ys xs)
    permutation xs (qsort xs)

[<Fact>]
let ``prop_ordered``() =
  Check.QuickThrowOnFailure <| fun (xs: int list) ->
    let rec ordered = function
      | [] -> true
      | [_] -> true
      | x :: y :: xs -> x <= y && ordered (y :: xs)
    ordered (qsort xs)
