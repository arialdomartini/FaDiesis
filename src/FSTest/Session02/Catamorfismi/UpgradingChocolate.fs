module FSTest.Session02.Catamorfismi.UpgradingChocolate

open FSTest.Session02.Catamorfismi.ManualRecursion
open RecursiveTypes
open Xunit
open Swensen.Unquote

// x * y = 34 * y
// Eta Reduction

// Gift -> Gift
let upgradeChocolate (gift: Gift): Gift =
    let fBook = Book
    let fChocolate chocolate = Chocolate { chocolateType = SeventyPercent; price= chocolate.price }

    let fWrapped = Wrapped
    let fBoxed = Boxed
    let fWithACard = WithACard

    cata fBook fChocolate fWrapped fBoxed fWithACard gift




[<Fact>]
let ``upgrades chocolate, maintaining the structure`` () =

    let blackChocolate =
        Chocolate
            { Chocolate.chocolateType = ChocolateType.Black; price = 2m }

    let inABox = Boxed blackChocolate
    let withACard = WithACard(inABox, "Choco!")

    let upgraded =
        Chocolate
            { Chocolate.chocolateType = ChocolateType.SeventyPercent; price = 2m }

    test <@ upgradeChocolate blackChocolate = upgraded @>
    test <@ upgradeChocolate inABox = Boxed upgraded @>
    test <@ upgradeChocolate withACard = WithACard(Boxed upgraded, "Choco!") @>
