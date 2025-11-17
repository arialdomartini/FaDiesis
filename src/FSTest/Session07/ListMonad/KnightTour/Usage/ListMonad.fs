module FSTest.Session07.ListMonad.KnightTour.Usage.ListMonad

type ListBuilder() =
    member _.Return(x) = [x]
    member _.Bind(xs, f) = List.collect f xs
    member _.ReturnFrom(xs) = xs
    member _.Zero() = []

let list = ListBuilder()
