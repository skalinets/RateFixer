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


[<Fact(Timeout = 300000)>]
let ``get rates for given dates`` () =
  let tod (s:string) = 
    System.DateTime.ParseExact(s, "d-MMM", System.Globalization.CultureInfo.GetCultureInfo("en-US"))

  let dates = (""" 3-Apr
                2-Apr
                1-Apr
                31-Mar
                30-Mar
                29-Mar
                27-Mar
                26-Mar
                24-Mar
                23-Mar
                22-Mar
                20-Mar
                19-Mar""").Split([| System.Environment.NewLine |], System.StringSplitOptions.RemoveEmptyEntries)
                 |> Seq.map (fun (s:string) -> s.Trim())
                 |> Seq.map tod

  let curr = ["USD";"SEK"]
  let toC = "UAH"
  let fee = 1.0
  
  let getAvalRate c d = getRate c toC fee d
  
  let gr (d, c) =
    (d, c, getAvalRate c d )
  
  let cartesian xs ys = 
    xs |> Seq.collect (fun x -> ys |> Seq.map (fun y -> x, y))
  
  let un (d, l) =
    let cur (_, c, r) = (c, r) 
    let f :: s :: _ = l |> List.ofSeq
    d, f |> cur, s |> cur

  let rates = cartesian dates curr
                |> Seq.map gr
                |> Seq.groupBy (fun (x, _ , _) -> x)
                |> Seq.map un

  rates |> Seq.iter (printfn "%A") |> ignore

  let s = match List.ofSeq dates with
          | s :: _ -> s
   
          
  test <@ s = new System.DateTime(2015, 4, 3) @>
