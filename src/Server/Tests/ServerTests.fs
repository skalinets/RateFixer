module Tests

open Xunit
open Swensen.Unquote
open Server


[<Fact>]
let ``check get rate for known date`` () =
  let fromC = "SEK"
  let toC = "UAH"
  let fee = 1.0
  let asOfDate = new System.DateTime(2015, 2, 7) // "02/07/2015"

  let r = getRate fromC toC fee asOfDate

  test <@ r = 3.131489 @>