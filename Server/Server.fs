module Server

open Xunit
open FsUnit.Xunit
open HttpClient
open System.Web
open Swensen.Unquote

[<Fact>]
let ``simple test``() =
  1 |> should equal 1

[<Fact>]
let ``test some stuff`` () =
  ["http://news.bbc.co.uk"
   "http://www.wikipedia.com"
   "http://www.stackoverflow.com"]
  |> List.map (fun url -> createRequest Get url |> getResponseBodyAsync)
  |> Async.Parallel
  |> Async.RunSynchronously
  |> Array.iter (printfn "%s")

type KeyValuePair = {Key: string; Value: string}

[<Fact>]
let ``give http.fs a try`` () =

//  withBody

  let setUA  = withHeader (UserAgent "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2349.2 Safari/537.36")
  
  let resp = createRequest Get "http://usa.visa.com/personal/card-benefits/travel/exchange-rate-calculator.jsp"
              |> setUA
              |> getResponse
  //let 
  let c = resp.Cookies |> Seq.map (fun m -> withCookie {name = m.Key; value = m.Value})
          
//    let fc = c |> List.fold
      
  let request = createRequest Post "http://usa.visa.com/personal/card-benefits/travel/exchange-rate-calculator-results.jsp"
//      |> withBodyEncoded 
                  |> withHeader (ContentType "application/x-www-form-urlencoded")
                  |> setUA

 // let toc c r = r |> withCookie c


  let toFormParam n l = {Key = n.name; Value = n.value} :: l

  let fp = [] |> toFormParam {name = "homCur"; value = "UAH"}
                  |> toFormParam {name = "forCur"; value = "SEK"}
                  |> toFormParam {name = "fee"; value = "1"}
                  |> toFormParam {name = "date"; value = "02/07/2015"}
                  |> toFormParam {name = "rate"; value = "0"}
                  |> toFormParam {name = "submit.x"; value = "117"}
                  |> toFormParam {name = "submit.7"; value = "13"}
                  |> toFormParam {name = "submit"; value = "Calculate Exchange Rates"}
                  |> toFormParam {name = "firstDate"; value = "02/10/2014"}
                  |> toFormParam {name = "lastDate"; value = "02/09/2015"}
                  |> toFormParam {name = "actualDate"; value = "02-07-2015"}

  let bodyFunc acc kvp =
    let e (s:string) = System.Web.HttpUtility.UrlEncode s
    let v = e kvp.Key + "=" + e kvp.Value //|> e
    match acc with
    | "" -> v
    | _ -> acc + "&" + v 
  
  let b = fp |> Seq.fold bodyFunc ""

  let r = c |> Seq.fold (fun rr cc -> cc rr) request
            |> withBody b
    

  let r = r |> getResponseBody

  test <@ r.Contains("SEK") @>
