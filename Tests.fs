module Tests

open System
open Xunit
open FsCheck

let rec runLengthEnc (xs: 'a list): (int * 'a) list =
  match xs with
  | hd :: _ ->
    // let (hds, remainder) = List.partition (fun x -> x = hd) xs
    let hds = List.takeWhile (fun x -> x = hd) xs
    let n = List.length hds
    let (_, remainder) = List.splitAt n xs
    (n, hd) :: runLengthEnc remainder
  | _ ->
    []

let runLengthDec (r: (int * 'a) list): 'a list =
  List.collect (fun t -> List.replicate (fst t) (snd t)) r

let alphaNumChar: char Gen = Gen.elements <| ['a'..'z'] @ ['A'..'Z'] @ ['0'..'9']

let rleItem: (int * char) Gen =
  gen {
    let! n = Gen.choose (1, 20)
    let! c = alphaNumChar
    return n, c
  }

let rec rleList(size: int): (int * char) list Gen =
  if (size <= 1) then Gen.map List.singleton rleItem
  else gen {
    let! tail = rleList (size - 1)
    let (_, c1) = List.head tail
    let! head = Gen.where (fun t -> snd t <> c1) rleItem
    return head :: tail
  }

let genOutput: (int * char) list Gen = Gen.sized rleList

[<Fact>]
let ``run length encoding round trip``() =
  let prop (r: (int * char) list) = runLengthEnc(runLengthDec(r)) = r
  let arb = Arb.fromGen genOutput
  Prop.forAll arb prop |> Check.QuickThrowOnFailure
