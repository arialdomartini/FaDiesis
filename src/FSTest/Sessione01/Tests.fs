module FSTest.Sessione01.Tests

open System
open Xunit


let twice n = n * 2

// int -> (int -> int)

let sum' = fun a -> fun b -> a + b

let sum'' a b = a + b

let suzm a b = a + b

let sum a b = a + b
let aggiungi10 = sum 10

Assert.Equal(12, aggiungi10 2)

let sum2 (a: int, b: int) : int = a + b

let twiceAndSum a b = sum a (twice b)

// Free Point Style
// Tacit programming

let twiceAndSum2 = twice >> sum

// forse prende dei valori e uno stato globale fa un'operazione e forse restituisce un valore
// prende un valore, restituisce un valore



//
// type OOP() =
//     member this.Sum(a: int, b: int) =
//         a + b
//
// [<Fact>]
// let ``sum of 2 numbers`` () =
//     let result = sum 2 3
//
//     Assert.Equal(5, result)
// //
// [<Fact>]
// let marco() =
//     let result = OOP().Sum(2, 3);
//
//     Assert.Equal(5, result);

type Log = string -> unit

let rec times s n =
    match n with
    | 1 -> s
    | m -> s + times s (m - 1)

let rec timesWithLogger (log: Log) s n =
    log $"Called with params {s} {n}"
    match n with
    | 1 -> s
    | m -> s + timesWithLogger log s (m - 1)

// times s 1 = s
// times s m = s + times s (m - 1)

[<Fact>]
let ``repeat a string n times``() =
    Assert.Equal("foofoofoo", times "foo" 3 )

[<Fact>]
let ``repeat a string n times, with logger``() =

    let consoleLog (s: string) =
        Console.WriteLine(s)

    let noLog _ = ()

    let timesWithConsole =
        timesWithLogger consoleLog

    let timesWithoutLog =
        timesWithLogger noLog

    Assert.Equal("foofoofoo", timesWithConsole "foo" 3 )
