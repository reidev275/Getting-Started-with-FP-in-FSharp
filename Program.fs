module Locations.App
open Suave
open Suave.Filters
open Suave.Html
open Suave.Operators
open Suave.Successful
open Suave.RequestErrors
open Suave.Utils
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

let getLocationById id =
    use query = new SqlCommandProvider<"Select * from Locations where id = @id", db, SingleRow=true>(db)
    query.Execute(id)
    |> Option.map (fun loc -> { Id = loc.Id; City = loc.City; State = loc.State })

let routeLocationById maybeLocation =
    match maybeLocation with 
    | None -> never
    | Some loc -> 
        { Id = loc.Id; City = loc.City; State = loc.State }
        |> locationCard
        |> fun x -> 
            h "div" [] [ 
                x 
                h "form" ["action", "/locations/delete/" + (string loc.Id); "method", "POST"] [ 
                    h "input" ["type", "submit"; "value", "Delete Location"] [] 
                ]
            ]
        |> master (loc.City + ", " + loc.State)
        |> xmlToString
        |> OK

let locationById id = 
    id
    |> getLocationById  // data access
    |> routeLocationById  // view rendering





let getAllLocations () =
    async {
        use query = new SqlCommandProvider<"Select * from Locations", db>(db)
        let! locations = query.AsyncExecute()
        return locations
               |> Seq.map (fun x -> { Id = x.Id; City = x.City; State = x.State }) 
               |> Seq.toList
    }

let displayLocations getLocations =
    async {
        let! locations = getLocations()
        return locations
                |> List.map locationCard 
                |> cardList 
                |> fun x -> 
                    h "div" [] [ 
                        x
                        h "a" ["role", "button"; "href", "/locations/add"] [text "Add Location"]
                    ]
                |> master "Locations"
                |> xmlToString
    }


let app = 
    choose [
        Slides.slides
        path "/locations" >=> 
            fun (x: HttpContext) ->
                async { 
                    let! html = displayLocations getAllLocations
                    return! OK html x
                }
        GET >=> pathScan "/locations/%d" locationById
        NOT_FOUND "This isn't the page you're looking for :handwave:"
        ]
        
startWebServer defaultConfig app