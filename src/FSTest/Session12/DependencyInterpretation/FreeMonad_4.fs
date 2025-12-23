module FSharpBits.ParserCombinators.XPUG.FreeMonad_4

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

let program () : decimal Program =
    let value1 = Value 0m
    
    let command2 = WriteLine ("Too many, I am sorry", fun () -> value1)
    
    let cartFunc cart =
        if Seq.length cart.products > 10 then
            command2
        else
            Value (cart.products |> Seq.sumBy _.price)

    let cartId = 42    
    let command1 = GetCart (cartId, cartFunc)

    command1



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
