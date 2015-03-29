module Server

open HttpClient

let getRate fromC toC fee (asOfDate : System.DateTime) =
  let date = asOfDate.ToString("MM/dd/yyyy")

  let setUA  = withHeader (UserAgent "Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2349.2 Safari/537.36")
  
  let resp = createRequest Get "http://usa.visa.com/personal/card-benefits/travel/exchange-rate-calculator.jsp"
              |> setUA
              |> getResponse
  let c = resp.Cookies |> Seq.map (fun m -> withCookie {name = m.Key; value = m.Value})
          
  let request = createRequest Post "http://usa.visa.com/personal/card-benefits/travel/exchange-rate-calculator-results.jsp"
                  |> withHeader (ContentType "application/x-www-form-urlencoded")
                  |> setUA

  let toFormParam n l = n :: l

  let fp = [] |> toFormParam {name = "homCur"; value = toC}
                  |> toFormParam {name = "forCur"; value = fromC}
                  |> toFormParam {name = "fee"; value = string fee}
                  |> toFormParam {name = "date"; value = date}
                  |> toFormParam {name = "rate"; value = "0"}
                  |> toFormParam {name = "submit.x"; value = "117"}
                  |> toFormParam {name = "submit.7"; value = "13"}
                  |> toFormParam {name = "submit"; value = "Calculate Exchange Rates"}
                  |> toFormParam {name = "firstDate"; value = date}
                  |> toFormParam {name = "lastDate"; value = date}
                  |> toFormParam {name = "actualDate"; value = date}

  let bodyFunc acc kvp =
    let e (s:string) = System.Web.HttpUtility.UrlEncode s
    let v = e kvp.name + "=" + e kvp.value
    match acc with
    | "" -> v
    | _ -> acc + "&" + v 
  
  let b = fp |> Seq.fold bodyFunc ""

  let r = c |> Seq.fold (fun rr cc -> cc rr) request
            |> withBody b

  let r = r |> getResponseBody
  let rate = System.Text.RegularExpressions.Regex.Match(r, @"\<p class=""rate""\>1 " + fromC + " = (?<rate>(\d|.)+) " + toC + "\</p\>").Groups.["rate"].Value

  double rate
