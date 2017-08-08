module Tests

#nowarn "988"

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

[<Fact>]
let ``run length encoding round trip``() =

  let lowerCaseChar: char Gen = Gen.choose ((int)'a', (int)'z') |> Gen.map (char)
  let upperCaseChar: char Gen = Gen.choose ((int)'A', (int)'Z') |> Gen.map (char)
  let numChar: char Gen = Gen.choose ((int)'0', (int)'9') |> Gen.map (char)

  let runLengthDec (r: (int * 'a) list): 'a list =
    List.collect (fun t -> List.replicate (fst t) (snd t)) r

  let rleItem =
    gen {
      let! n = Gen.choose (1, 20)
      let! c = lowerCaseChar
      return n, c
    }

  let rec rleList(size: int): (int * char) list Gen =
    if (size <= 1) then Gen.map List.singleton rleItem
    else
      gen {
        let! tail = rleList (size - 1)
        let (_, c1) = List.head tail
        let! head = Gen.where (fun t -> snd t <> c1) rleItem
        return head :: tail
      }

  let genOutput: (int * char) list Gen = Gen.sized rleList

  let prop (r: (int * char) list) = runLengthEnc(runLengthDec(r)) = r
  let arb = Arb.fromGen genOutput
  Prop.forAll arb prop |> Check.QuickThrowOnFailure
