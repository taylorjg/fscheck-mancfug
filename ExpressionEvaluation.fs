module ExpressionEvaluation

open System
open FsCheck
open FsCheck.Xunit

// http://booksites.artima.com/scalacheck/examples/html/ch06.html#sec2

////////////////////////////////////////////////////////////////////////////////
// Implementation code
////////////////////////////////////////////////////////////////////////////////

type Expression =
  | Const of n: int
  | Add of e1: Expression * e2: Expression
  | Mul of e1: Expression * e2: Expression

let rec eval = function
  | Const n -> n
  | Add (e1, e2) -> eval e1 + eval e2
  | Mul (e1, e2) -> eval e1 * eval e2

let rewrite = function
  | Add (e1, e2) when e1 = e2 -> Mul (Const 2, e1)
  | Mul (Const 0, e) -> Const 0
  | Add (Const 1, e) -> e
  | e -> e

////////////////////////////////////////////////////////////////////////////////
// Custom generators
////////////////////////////////////////////////////////////////////////////////

let genConst = Gen.choose (0, 10) |> Gen.map Const

#nowarn "40"
let rec genExpr = Gen.sized <| fun sz ->
  Gen.frequency [
    sz, genConst
    sz - (int) (Math.Sqrt ((float)sz)), Gen.resize (sz/2) genAdd
    sz - (int) (Math.Sqrt ((float)sz)), Gen.resize (sz/2) genMul
  ]
and genAdd = gen {
    let! e1 = genExpr
    let! e2 = genExpr
    return Add (e1, e2)
  }
and genMul = gen {
    let! e1 = genExpr
    let! e2 = genExpr
    return Mul (e1, e2)
  }

let shrinkExpr = function
  | Const n -> Arb.shrink n |> Seq.map Const
  | Add (e1, e2) ->
    Seq.concat [
      seq [e1; e2]
      Arb.shrink(e1) |> Seq.map (fun e1' -> Add (e1', e2))
      Arb.shrink(e2) |> Seq.map (fun e2' -> Add (e1, e2'))
    ]
  | Mul (e1, e2) ->
    Seq.concat [
      seq [e1; e2]
      Arb.shrink(e1) |> Seq.map (fun e1' -> Mul (e1', e2))
      Arb.shrink(e2) |> Seq.map (fun e2' -> Mul (e1, e2'))
    ]

type CustomArbitraries =
  static member Expression() =
    { new Arbitrary<Expression>() with
      override x.Generator = genExpr
      override x.Shrinker e = shrinkExpr e
    }

Arb.register<CustomArbitraries>() |> ignore

////////////////////////////////////////////////////////////////////////////////
// Property test
////////////////////////////////////////////////////////////////////////////////

[<Property>]
let propRewrite expr =
  let rexpr = rewrite expr
  eval rexpr = eval expr |@ sprintf "rewritten = %A" rexpr
