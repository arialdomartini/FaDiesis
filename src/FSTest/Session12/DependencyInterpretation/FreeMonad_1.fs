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


type Program<'t> =
    | GetCart of (int * (Cart -> 't Program))
    | WriteLine of (string  * (unit -> 't Program))
    | Value of 't

let program' () : decimal =

        let cartId = 42
        let cart = Api.getCart cartId

        if Seq.length cart.products > 10 then
            do Console.WriteLine "Too many, I am sorry"
            0m
        else
            cart.products |> Seq.sumBy _.price

// smart constructors
let getCart cartId =
    GetCart (cartId, (fun cart -> Value cart))

let writeLine s =
    WriteLine (s, (fun () -> Value ()))

let rec bind (program: 'a Program) (continuation: 'a -> 'b Program) =
    match program with
    | GetCart (cartId, f: Cart -> 'a Program) ->
        let fC (cart:Cart): 'b Program =
            bind (f cart) continuation
        GetCart (cartId, fC)

    | WriteLine (s, f) ->
        let fC () =
            bind (f ()) continuation
        WriteLine (s, fC)
    | Value value -> continuation value

let (>>=) = bind

type InjectBuilder() =
    member _.Bind(m, f) = m >>= f
    member _.Return(v) = Value v
    member _.ReturnFrom(m) = m

let inject = InjectBuilder ()

let program () : decimal Program =
    inject {
        let cartId = 42
        let! cart = getCart cartId

        if Seq.length cart.products > 10 then
            do! writeLine "Too many, I am sorry"
            return 0m
        else
           return cart.products |> Seq.sumBy _.price
    }

let rec interpret (program: decimal Program) : decimal =
    match program with
    | GetCart (cartId, f) ->
        let cart = Api.getCart cartId
        interpret (f cart)
    | WriteLine (s, f) ->
        Console.WriteLine s
        interpret (f ())
    | Value value ->
        value


[<Fact>]
let ``hard to test`` () =
    let programRappresentation = program ()
    let result = interpret programRappresentation

    test <@ result = 392.99m @>
