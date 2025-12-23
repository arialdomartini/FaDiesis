module FSharpBits.ParserCombinators.XPUG.FreeMonad_2

// type Product = { name: string; price: decimal }
// type Cart = { id: int; products: Product list }
//
// module Api =
//     let getCart cartId : Cart =
//         { id = cartId
//           products = [ { name = "book"; price = 42m }
//                        { name = "keyboard"; price = 350.99m } ] }
//
// type GetCart = GetCart of int
// type WriteLine = WriteLine of string
// type Value = Value of decimal
//
// let program () : Value decimal =
//     let cartId = 42
//     let command1 = GetCart cartId
//
//     if Seq.length cart.products > 10 then
//         let command2 = WriteLine "Too many, I am sorry"
//         Value 0m
//     else
//         Value (cart.products |> Seq.sumBy _.price)
//
