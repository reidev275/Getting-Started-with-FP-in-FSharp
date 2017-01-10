module Locations.App
open Suave
open Suave.Filters
open Suave.Html
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Locations.Layout
open FSharp.Data

[<Literal>]
let db = "Server=localhost\SQLEXPRESS;Database=Locations;Trusted_Connection=True;" 

type Location = {
    Id : int
    City : string
    State : string
}

let locationCard location =
    h "a" [ "href", "/locations/" + (string location.Id)] [
        h "c-card" [] [
            h "h3" [] [ location.City + ", " + location.State |> text ]
            h "img" [ "src", "https://maps.googleapis.com/maps/api/staticmap?center=" + location.City + "," + location.State + "&size=300x150&key=AIzaSyDXuYY2dFShepbNHfMwXP9mbgof-SAr3XI"] []
        ]
    ]

let getAllLocations () =
    use query = new SqlCommandProvider<"Select * from Locations", db>(db)
    query.Execute()
    |> Seq.map (fun x -> { Id = x.Id; City = x.City; State = x.State }) 
    |> Seq.toList

let displayLocations getLocations =
    getLocations()
    |> List.map locationCard 
    |> cardList 
    |> fun x -> 
        h "div" [] [ 
            x
            h "a" ["role", "button"; "href", "/locations/add"] [text "Add Location"]
        ]
    |> master "Locations"
    |> xmlToString


let app = 
    choose [
        Slides.slides
        path "/locations" >=> GET >=> request (fun _ -> OK (displayLocations getAllLocations))
        NOT_FOUND "This isn't the page you're looking for :handwave:"
        ]
        
startWebServer defaultConfig app