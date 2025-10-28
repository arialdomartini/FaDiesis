module FSTest.Session03.App

open System

type Error = string
type Code = { message: string; codeNumber: int }
type Result<'a> =
    | Success of 'a
    | Failure of Error

let number: Result<int> = Success 42
let number2: Result<int> = Failure "Not found"

// Option<'a> -> ('a -> 'b) -> Option<'b>
// 'a option -> ('a -> 'b) -> 'b option
let apply (v: Result<'a>) f =
    match v with
    | Success a -> Success (f a)
    | Failure error -> Failure error

let (<!!>) f v = apply v f

// computation expression
type ResultBuilder() =
    member _.Bind(x: Result<'a>, f: 'a -> Result<'b>) : Result<'b> =
        match x with
        | Success a -> f a
        | Failure e -> Failure e

    member _.Return(x: 'a) : Result<'a> =
        Success x
    member _.ReturnFrom(x: Result<'a>) : Result<'a> = x

let result = ResultBuilder()

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
    | "some-gold-cart" -> Success { Cart.id = id; customerId = CustomerId("gold-customer"); amount= Amount 100m }
    | "some-normal-cart" -> Success { Cart.id = id; customerId = CustomerId("normal-customer"); amount= Amount 100m }
    | _ -> Failure "Cart not found"


let lookupDiscountRule (CustomerId id) =
    let halfDiscountRule = half
    let noDiscountRule = (fun _ -> raise (InvalidOperationException "no discount"))

    match id with
    | "gold-customer" -> Success halfDiscountRule
    | _ -> Failure "Discount not applicable"


let save (cart: Cart) (flush: IStorage<Cart>) =
    flush cart
    cart

let updateAmount(cart: Cart) (Amount discount) =
    let (Amount cartAmount) = cart.amount
    { cart with amount = Amount(cartAmount - discount) }


// a -> b
let twice n = n * 2

let v: Result<int> = Success 42
let r = twice 42
let rr = twice <!!> (Success 42) // Some 84
let vv = twice <!!> Failure "Obluraschi"
// Some 84

let applyDiscountRuleAndSave cart storage rule =
    let discount = rule cart
    let updatedCart = updateAmount cart discount
    save updatedCart storage

let tryApplyDiscount (storage: IStorage<Cart>) cart =
        let discountRule = lookupDiscountRule cart.customerId;
        (applyDiscountRuleAndSave cart storage) <!!> discountRule

let applyDiscount(cartId: CartId) (storage: IStorage<Cart>) =
    //
    // let cart = loadCart cartId
    // (tryApplyDiscount storage) <!!> cart
    //
    let x =
        result {
            let! cart = loadCart cartId
            let! cartUpdated = tryApplyDiscount storage cart
            return cartUpdated }

    match x with
    | Success cart -> failwith "todo"
    | Failure s -> failwith "todo"
