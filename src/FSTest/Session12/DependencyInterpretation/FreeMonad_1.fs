module FSharpBits.ParserCombinators.XPUG.FreeMonad_1

open System
open Xunit
open Swensen.Unquote

type Product = { name: string; price: decimal }
type Cart = { id: int; products: Product list }

module Api =
    let getCart cartId : Cart =
        { id = cartId
          products = [ { name = "book"; price = 42m }
                       { name = "keyboard"; price = 350.99m } ] }

type Program<'a> =
    |  GetCart of (int * (Cart -> 'a Program))
    |  WriteLine of (string * (unit -> 'a Program))
    |  Value of 'a

// Smart Constructors
let getCart cartId = GetCart (cartId, fun cart -> Value cart)
let writeLine s = WriteLine (s, fun () -> Value ())

// Free Monad
let rec andThen (m: 'a Program) (nextContinuation: 'a -> 'b Program) =
    match m with
    | GetCart (cartId, continuation) ->
        GetCart (cartId, fun cart ->
            let pA = continuation cart
            andThen pA nextContinuation)

    | WriteLine (s, continuation) ->
        WriteLine (s, fun () ->
            andThen (continuation ()) nextContinuation)
    | Value a ->
        nextContinuation a

let (>>=) = andThen

type InjectBuilder() =
    member this.Bind(m, f) = m >>= f
    member this.Return(v) = Value v

let inject = InjectBuilder()

let program'' () : decimal Program =
    let cartId = 42
    (getCart cartId) >>=
        (fun cart ->

            if Seq.length cart.products > 10 then
                writeLine "Too many, I am sorry" >>= (fun () ->
                    Value 0m)
            else
                Value (cart.products |> Seq.sumBy _.price)
        )

let program () : decimal Program =
    inject {
        let cartId = 42
        let! cart = getCart cartId

        if Seq.length cart.products > 10 then
            do! writeLine "Too many, I am sorry"
            return 0m
        else
            return (cart.products |> Seq.sumBy _.price)
    }

let rec int (p: 'a Program): 'a =
    match p with
    | GetCart (cartId, c)  ->
        let cart = Api.getCart cartId
        int (c cart)

    | WriteLine (s, c) ->
        Console.WriteLine s
        int (c ())
    | Value a -> a

let program' () : decimal =
    let cartId = 42
    let cart = Api.getCart cartId

    if Seq.length cart.products > 10 then
        Console.WriteLine "Too many, I am sorry"
        0m
    else
        cart.products |> Seq.sumBy _.price


let rec interpret (program: 'a Program) : 'a =
    match program with
    | GetCart (cartId, continuation) ->
        let cart = Api.getCart cartId
        let p: Program<'a> = continuation cart
        interpret p

    | WriteLine (s, continuation) ->
        Console.WriteLine s
        interpret (continuation ())

    | Value a -> a

let rec testTnterpret (program: 'a Program) : 'a =
    match program with
    | GetCart (cartId, continuation) ->
        let cart = Api.getCart cartId
        let p: Program<'a> = continuation cart
        interpret p

    | WriteLine (s, continuation) ->
        Console.WriteLine s
        interpret (continuation ())

    | Value a -> a


[<Fact>]
let ``hard to test`` () =

    let prog: decimal Program = program ()

    let result = interpret prog

    test <@ result = 392.99m @>
