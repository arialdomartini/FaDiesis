module FSTest.Session06.Internals.Result

type Result<'success> =
    | Ok of 'success
    | Failure of string list


let pure' a = Ok a

let map f (a: 'a Result) =
    match a with
    | Ok a -> Ok (f a)
    | Failure e -> Failure e

let (<!>) = map

let ap (f: ('a -> 'b) Result) (a: 'a Result) =
    match f with
    | Ok fOk ->
        match a with
        | Ok aValue -> Ok (fOk aValue)
        | Failure aErr -> Failure aErr
    | Failure fErrors ->
        match a with
        | Ok _ -> Failure fErrors
        | Failure aErrors -> Failure (aErrors @ fErrors)


let (<*>) = ap

let bind (a: 'a Result) (f: 'a -> 'b Result) =
    match a with
    | Ok a -> f a
    | Failure errors -> Failure errors

let (>>=) = bind
