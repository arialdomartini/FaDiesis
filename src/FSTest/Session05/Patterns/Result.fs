module FSTest.Session05.Patterns.Result


type Result<'success> =
    | Ok of 'success
    | Failure of string list


let pure' a = Ok a

let map (f: 'a -> 'b) (a: Result<'a>) =
    match a with
    | Ok resultValue -> Ok(f resultValue)
    | Failure errorValue -> Failure errorValue

let (<!>) = map

let ap (f: Result<'a -> 'b>) (a: Result<'a>) =
    match f with
    | Ok fOk ->
        match a with
        | Ok aOK -> Ok(fOk aOK)
        | Failure aFailure -> Failure aFailure
    | Failure fFailure ->
        match a with
        | Ok aOK -> Failure fFailure
        | Failure aFailure -> Failure(aFailure @ fFailure)

let (<*>) = ap

let bind (a: Result<'a>) (f: 'a -> Result<'b>) =
    match a with
    | Ok aOK -> f aOK
    | Failure aFailure -> Failure aFailure

let (>>=) = bind
