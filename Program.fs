module Locations.App
open Suave
open Suave.RequestErrors
open Locations.Layout

type Location = {
    Id : int
    City : string
    State : string
}


let app = 
    choose [
        Slides.slides
        NOT_FOUND "This isn't the page you're looking for :handwave:"
        ]
        
startWebServer defaultConfig app