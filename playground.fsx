#r "/Users/jontaylor/.nuget/packages/FsCheck/2.9.0/lib/netstandard1.6/FsCheck.dll"

open FsCheck

let genInt = Arb.generate<int list>

printfn "sample: %A" (Gen.sample 2 5 genInt)
printfn "sample: %A" (Gen.sample 5 2 genInt)

printfn "sample: %A" (Gen.constant 42 |> Gen.sample 2 2)
printfn "sample: %A" (Gen.constant 42 |> Gen.map (string) |> Gen.sample 2 2)
printfn "sample: %A" (Gen.constant 42 |> Gen.map Gen.constant |> Gen.sample 2 2)

printfn "sample: %A" (Gen.constant 42 |> (fun g -> gen.Bind (g, Gen.constant)) |> Gen.sample 2 2)
