module FSTest.Session03.FunctionalRefactoring.Tests


open FSTest.Session03.App
open Xunit
open Swensen.Unquote


let spyStorage (saved: Cart ref) (item: Cart) =
    saved.Value <- item
    ()


[<Fact>]
let ``happy path`` () =
    let cartId = CartId "some-gold-cart"
    let saved = ref missingCart
    let storage = spyStorage saved


    applyDiscount cartId storage

    let expected = { Cart.id = CartId "some-gold-cart"; customerId = CustomerId "gold-customer"; amount = Amount 50m }

    test <@ saved.Value = expected  @>

[<Fact>]
let ``no discount`` () =
    let cartId = CartId "some-normal-cart"
    let saved = ref missingCart
    let storage = spyStorage saved

    applyDiscount cartId storage

    test <@ saved.Value = missingCart  @>


[<Fact>]
let ``missing cart`` () =
    let cartId = CartId "missing cart"
    let saved = ref missingCart
    let storage = spyStorage saved

    applyDiscount cartId storage

    test <@ saved.Value = missingCart  @>
