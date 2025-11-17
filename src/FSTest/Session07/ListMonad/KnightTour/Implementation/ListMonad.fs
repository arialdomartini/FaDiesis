module FSTest.Session07.ListMonad.KnightTour.Implementation.ListMonad


let bind xs f = failwith "Not yet implemented"

type ListBuilder() =
    member _.Return(x) = failwith "Not yet implemented"
    member _.Bind(xs, f) = bind xs f
    member _.ReturnFrom(xs) = failwith "Not yet implemented"
    member _.Zero() = failwith "Not yet implemented"

let list = ListBuilder()
