module ExpressionEvaluation

open System
open Xunit
open FsCheck

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

// implicit val shrinkExpr: Shrink[Expression] = Shrink({
//   case Const(n) => shrink(n) map Const
//   case Add(e1, e2) => Stream.concat(
//     Stream(e1, e2),
//     shrink(e1) map (Add(_, e2)),
//     shrink(e2) map (Add(e1, _))
//   )
//   case Mul(e1, e2) => Stream.concat(
//     Stream(e1, e2),
//     shrink(e1) map (Mul(_, e2)),
//     shrink(e2) map (Mul(e1, _))
//   )
// })

////////////////////////////////////////////////////////////////////////////////
// Property test
////////////////////////////////////////////////////////////////////////////////

// val propRewrite = forAll(genExpr) { expr =>
//   val rexpr = rewrite(expr)
//   (eval(rexpr) ?= eval(expr)) :| "rewritten = "+rexpr
// }

[<Fact>]
let propRewrite() =
  let arb = Arb.fromGen genExpr
  let p expr = eval (rewrite expr) = eval expr
  Prop.forAll arb p |> Check.QuickThrowOnFailure
