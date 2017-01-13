module Locations.Slides

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Html
open Locations.Layout

let takeaways = 
    coverPage "Takeaways" "" [
                h "h1" [] [text "Takeaways"]
                h "h3" [] [text "Defaults matter"]
                h "h3" [] [text "Type Providers == Awesome"]
                h "h3" [] [text "No more null!"]
                h "h3" [] [text "Composing small functions"]
            ]

let slides = 
    choose [
        pathRegex "(.*)\.(css|jpg|png)" >=> Files.browseHome
        path "/" >=> OK index
        path "/domaincodomain" >=> OK (coverPage "Domain Codomain" "/domain-codomain.png" [])
        path "/summary" >=> OK summary
        path "/takeaways" >=> OK takeaways
        path "/null" >=> OK (coverPage "Null" "" [ h "h3" [] [text "If everything can be null, you have to check for null everywhere"]])
    ]
