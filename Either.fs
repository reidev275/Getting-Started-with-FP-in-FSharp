module Locations.Either

//type Option<'T> =
//    | Some of 'T
//    | None

type EitherT<'Failure, 'Success> =
    | Failure of 'Failure
    | Success of 'Success


let map mapping either =
    match either with
    | Failure l -> Failure l
    | Success r -> Success (mapping r)

let bind binding either =
    match either with
    | Failure l -> Failure l
    | Success r -> binding r

type EitherBuilder() =
    member this.Bind(m, f) = bind f m
    member this.Return(x) = Success x

let either = new EitherBuilder()