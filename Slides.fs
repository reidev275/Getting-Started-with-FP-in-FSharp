module Locations.Slides

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Locations.Layout

let slides = 
    choose [
        pathRegex "(.*)\.(css|jpg|png)" >=> Files.browseHome
        path "/" >=> OK index
        path "/domaincodomain" >=> OK (coverPage "Domain Codomain" "contain" "/domain-codomain.png" [])
        path "/summary" >=> OK summary
    ]
