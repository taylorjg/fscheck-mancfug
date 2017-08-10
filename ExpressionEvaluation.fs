module ExpressionEvaluation

open System
open Xunit
open FsCheck

// http://booksites.artima.com/scalacheck/examples/html/ch06.html#sec2

////////////////////////////////////////////////////////////////////////////////
// Implementation code
////////////////////////////////////////////////////////////////////////////////

// trait Expression {
//   override def toString = show(this)
// }

// case class Const(n: Int) extends Expression

// case class Add(
//   e1: Expression, e2: Expression
// ) extends Expression

// case class Mul(
//   e1: Expression, e2: Expression
// ) extends Expression

// def eval(e: Expression): Int = e match {
//   case Const(n) => n
//   case Add(e1,e2) => eval(e1) + eval(e2)
//   case Mul(e1,e2) => eval(e1) * eval(e2)
// }

// def show(e: Expression): String = e match {
//   case Const(n) => n.toString
//   case Add(e1,e2) => "("+show(e1)+" + "+show(e2)+")"
//   case Mul(e1,e2) => "("+show(e1)+" * "+show(e2)+")"
// }

// def rewrite(e: Expression): Expression = e match {
//   case Add(e1, e2) if e1 == e2 => Mul(Const(2), e1)
//   case Mul(Const(0), e) => Const(0)
//   case Add(Const(1), e) => e
//   case _ => e
// }

////////////////////////////////////////////////////////////////////////////////
// Custom generators
////////////////////////////////////////////////////////////////////////////////

// val genExpr: Gen[Expression] = Gen.sized { sz =>
//   Gen.frequency(
//     (sz, genConst),
//     (sz - math.sqrt(sz).toInt, Gen.resize(sz/2, genAdd)),
//     (sz - math.sqrt(sz).toInt, Gen.resize(sz/2, genMul))
//   )
// }

// val genConst = Gen.choose(0, 10).map(Const(_))

// val genAdd = for {
//   e1 <- genExpr; e2 <- genExpr
// } yield Add(e1, e2)

// val genMul = for {
//   e1 <- genExpr; e2 <- genExpr
// } yield Mul(e1, e2)

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
//   eval(rewrite(expr)) == eval(expr)
// }

// val propRewrite = forAll(genExpr) { expr =>
//   val rexpr = rewrite(expr)
//   (eval(rexpr) ?= eval(expr)) :| "rewritten = "+rexpr
// }
