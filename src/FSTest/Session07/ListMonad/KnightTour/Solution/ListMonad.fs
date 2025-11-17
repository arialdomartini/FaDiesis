module FSTest.Session07.ListMonad.KnightTour.Solution.ListMonad

let bind (xs: 'a list) (f: 'a -> 'b list) =
    [ for x in xs do
        for y in f x do
            yield y ]

let bind' (xs: 'a list) (f: 'a -> 'b list) : 'b list =
    let rec loop acc rest =
        match rest with
        | [] -> List.rev acc
        | x :: xs ->
            let ys = f x
            loop (List.append ys acc) xs
    loop [] xs

let bind'' = List.collect

let bind''' (xs: 'a list) (f: 'a -> 'b list) : 'b list =
    let results = System.Collections.Generic.List<'b>() // Mutable C#'s list
    for x in xs do
        let ys = f x
        results.AddRange ys
        // for y in ys do
        //     results.Add(y)
    List.ofSeq results


type ListBuilder() =
    member _.Return(x) = [x]
    member _.Bind(xs, f) = bind xs f
    member _.ReturnFrom(xs) = xs
//    member _.Zero() = []

let list = ListBuilder()
