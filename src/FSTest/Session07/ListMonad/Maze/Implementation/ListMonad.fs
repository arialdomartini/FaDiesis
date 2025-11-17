module FSTest.Session07.ListMonad.Maze.Implementation.ListMonad


type ListBuilder() =
    member _.Return(x) = failwith "Not yet implemented"
    member _.Bind(xs, f) = failwith "Not yet implemented"
    member _.ReturnFrom(xs) = failwith "Not yet implemented"
    member _.Zero() = failwith "Not yet implemented"

let list = ListBuilder()
