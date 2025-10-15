module FSTest.Session03.App

open System

type CartId = CartId of string
type CustomerId = CustomerId of string
type Amount = Amount of decimal
type Cart = { id: CartId; customerId: CustomerId; amount: Amount }
type IStorage<'t> = 't -> unit

let missingCart = { Cart.id = CartId ""; customerId= CustomerId ""; amount = Amount 0m };

type DiscountRule = Cart -> Amount

let half (cart: Cart) =
    let (Amount cartAmount) = cart.amount
    Amount (cartAmount / 2m)

let loadCart (id: CartId) =
    let (CartId idValue) = id
    match idValue with
    | "some-gold-cart" -> { Cart.id = id; customerId = CustomerId("gold-customer"); amount= Amount 100m }
    | "some-normal-cart" -> { Cart.id = id; customerId = CustomerId("normal-customer"); amount= Amount 100m }
    | _ -> missingCart;

let lookupDiscountRule (CustomerId id) =
    let halfDiscountRule = half
    let noDiscountRule = (fun _ -> raise (InvalidOperationException "no discount"))

    match id with
    | "gold-customer" -> (true, halfDiscountRule)
    | _ -> (false, noDiscountRule)


let save (cart: Cart) (flush: IStorage<Cart>) =
    flush cart

let updateAmount(cart: Cart) (Amount discount) =
    let (Amount cartAmount) = cart.amount
    { cart with amount = Amount(cartAmount - discount) }


let applyDiscount(cartId: CartId) (storage: IStorage<Cart>) =

    let cart = loadCart cartId
    if cart <> missingCart
    then
        let discountRule = lookupDiscountRule cart.customerId;
        let someDiscount, lrule = discountRule
        if someDiscount
        then
            let discount = rule cart
            let updatedCart = updateAmount cart discount
            save updatedCart storage
