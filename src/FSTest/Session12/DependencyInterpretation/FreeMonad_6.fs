module FSharpBits.ParserCombinators.XPUG.FreeMonad_6

open Xunit
open Swensen.Unquote

type Product = { name: string; price: decimal }
type Cart = { id: int; products: Product list }

module Api =
    let getCart cartId : Cart =
        { id = cartId
          products = [ { name = "book"; price = 42m }; { name = "keyboard"; price = 350.99m } ] }

type Program<'a> =
    | GetCart of int * (Cart -> Program<'a>)
    | WriteLine of string * (unit -> Program<'a>)
    | Value of 'a

let value v = Value v
let getCart id = GetCart (id, value)
let writeLine s = WriteLine (s, fun () -> value ())

let rec andThen (p: 'a Program) (f: 'a -> 'b Program) =
    match p with
    | GetCart(cartId, cartFunc) ->
        GetCart (cartId,
                 fun cart ->
                    let v = cartFunc cart
                    andThen v f)
    | WriteLine(s, unitFunc) ->
        WriteLine (s,
                   fun () ->
                       let v = unitFunc ()
                       andThen v f)
    | Value a -> f a

let (>>=) = andThen

let program () : decimal Program =
    let cartId = 42

    getCart cartId
    >>= (fun cart ->
        if List.length cart.products > 10 then
            writeLine "Too many, I am sorry"
            >>= (fun () -> value 0m)
        else
            value(cart.products |> Seq.sumBy _.price))



let rec interpret (program: 'a Program) =
    match program with
    | GetCart(cartId, cartFunc) ->
        let cart = Api.getCart cartId
        interpret (cartFunc cart)
    | WriteLine(s, unitFunc) ->
        System.Console.WriteLine s
        interpret (unitFunc ())
    | Value a -> a


let mutable out = ""

let getCartTest _ =
    let testProduct = { name = "test product"; price = 1m }

    { id = 42
      products = List.replicate 100 testProduct }

let rec testInterpret (program: 'a Program) =
    match program with
    | GetCart(cartId, cartFunc) ->
        let cart = getCartTest cartId
        testInterpret (cartFunc cart)
    | WriteLine(s, unitFunc) ->
        out <- s
        testInterpret (unitFunc ())
    | Value a -> a


[<Fact>]
let ``mock API and Console`` () =

    let p = program ()

    let result = testInterpret p

    test <@ result = 0m @>
    test <@ out = "Too many, I am sorry" @>
