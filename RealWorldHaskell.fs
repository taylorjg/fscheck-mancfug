module RealWorldHaskell

open System
open FsCheck
open FsCheck.Xunit

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

[<Property>]
let ``prop_idempotent`` (xs: int list) =
  qsort (qsort xs) = qsort xs
  
[<Property>]
let ``prop_minimum`` (xs: int list) =
  not (List.isEmpty xs) ==>
    lazy (List.head (qsort xs) = minimum xs)

[<Property>]
let ``prop_maximum`` (xs: int list) =
  not (List.isEmpty xs) ==>
    lazy (List.last (qsort xs) = maximum xs)

[<Property>]
let ``prop_append`` (xs: int list) (ys: int list) =
  (not (List.isEmpty xs) && not (List.isEmpty ys)) ==>
    lazy (List.head (qsort (xs @ ys)) = min (minimum xs) (minimum ys))

[<Property>]
let ``prop_permutation`` (xs: int list) =
  let permutation xs ys =
    List.isEmpty (List.except xs ys) &&
    List.isEmpty (List.except ys xs)
  permutation xs (qsort xs)

[<Property>]
let ``prop_ordered`` (xs: int list) =
  let rec ordered = function
    | [] -> true
    | [_] -> true
    | x :: y :: xs -> x <= y && ordered (y :: xs)
  ordered (qsort xs)
