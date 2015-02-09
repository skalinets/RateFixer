module Server

open Xunit
open FsUnit.Xunit

[<Fact>]
let ``simple test``() =
  1 |> should equal 1
