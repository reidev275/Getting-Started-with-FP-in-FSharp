module Locations.Layout
open Suave.Html

let h t attr x = tag t attr (flatten x)
let style href = tag "link" [ "rel","stylesheet"; "href", href ] empty

let headTag metaTitle = 
    head [
        title metaTitle
        style "/style.css" 
    ]

let master title children =
    html [
        headTag title
        body [ 
            h "nav" [] [
                h "img" ["src", "/fsharp128.png"] []
                h "a" [ "href", "/locations"] [ text "Locations"]
            ]
            children 
        ]
    ]

let redirectPage loc = 
    h "script" [] [
        text ("window.location = '" + loc + "'")
    ] |> xmlToString

let addLocationForm error =
    let input name typeVal = 
        h "div" [] [
            h "label" [ "for", name ] [ text name ]
            h "input" [ "type", typeVal; "id", name; "name", name; "placeholder", name ] []
        ]

    h "div" [] [
        h "h1" [] [ text "Add a location"]
        h "p" ["class", "errors"] [ text error ]
        h "form" [ "action", "/locations/add"; "method", "POST" ] [
            input "City" "text"
            input "State" "text"
            h "input" [ "type", "submit"; "value", "Add Location" ] []
        ]
    ] |> master "Add A Location" |> xmlToString

let cardList cards = h "c-list" [] cards

let coverPage title img nodes =
    h "html" [ "class", "contain"; "style", "background-image: url(" + img + ")"] [
        headTag title
        body nodes
    ] |> xmlToString

let index = 
    [ h "h1" [] [ text "Getting Started with Functional Programming in F#" ]
      h "h3" [] [ text "Codemash 2017 - 1/13/17 - 2:45 Salon H" ]
      h "h1" [] [ text "@ReidNEvans" ]
    ] |> coverPage "Intro" ""

let summary =
    [
      h "h1" [] [ text "Additional Resources"]
      h "h3" [] [ text "FsharpForFunAndProfit.com"]
      h "h3" [] [ text "ReidEvans.tech"]
      h "h3" [] [ text "@ReidNEvans"]
      h "h3" [] [ text "github.com/reidev275/Getting-Started-With-FP-in-FSharp"]
    ] |> coverPage "Summary" ""