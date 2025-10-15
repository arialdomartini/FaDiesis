module FSTest.Session01.Combinators


open System
open Xunit

// string -> int -> string
let rec times s n =
    match n with
    | 1 -> s
    | m -> s + times s (m - 1)

// (string -> int -> string) -> int -> string -> string
let swap f =
    fun n s -> f s n

[<Fact>]
let ``swap parameters`` () =
    // int -> string -> string
    let timesSwapped = swap times

    Assert.Equal("foofoo", timesSwapped 2 "foo")

// (a -> b) -> (b -> c) -> (a -> c)
let (>>) f g =
    fun a ->
        g (f a)

let (<<) g f = f >> g

[<Fact>]
let ``function combination`` () =

    // string -> string
    // 'a -> 'b
    let scream (s: string) = s.ToUpper()

    // string -> string
    // 'b -> 'c
    let twice (s: string) = times s 2

    let screamTwice = scream >> twice

    Assert.Equal("FOOFOO", screamTwice "foo")

type Predicate<'a> = 'a -> bool


//let (|>) (a:'a) (f: 'a -> 'b)
// a -> (a -> b) -> b
//let (|>) a f = f a
let (<|) f a = f a

[<Fact>]
let ``pipe operator`` () =

    // int -> int
    // 'a -> 'b
    let twice a = a * 2

    let r = twice <| 42
    let result =
        3
        |> twice
        |> twice

    // twice 3

    Assert.Equal(12, result)

// ('a -> bool) -> ('a -> unit) -> ('a -> unit)

let onlyIf p f =
    fun a ->
        if p a
        then f a
        else ()


[<Fact>]
let ``apply unless`` () =

    // Predicati
    // 'a -> bool
    let isPositive n =
        n > 0
    let isEven n =
        n % 2 = 0

    // funzione
    // 'a -> unit
    let printN (n: int) = Console.WriteLine(n)

    // funzioni derivate
    let printEven = onlyIf isEven printN
    let printPositive = printN |> onlyIf isPositive

    printEven 12
    printEven 11
    printPositive 11
