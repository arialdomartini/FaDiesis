module FSTest.Session02.Catamorfismi.ManualRecursion

open FSTest.Session02.Catamorfismi.SampleValues
open RecursiveTypes

// let rec description (gift: Gift) =
//     match gift with
//     | Book book -> $"Book titled '{book.title}'"
//     | Chocolate chocolate -> $"{chocolate.chocolateType} chocolate"
//     | Wrapped(innerGift, wrappingPaperStyle) -> $"{description innerGift} wrapped in {wrappingPaperStyle} paper"
//     | Boxed innerGift -> $"{description innerGift} in a box"
//     | WithACard(innerGift, message) -> $"{description innerGift} with a card saying {message}"



// type Gift =
//     | Book of Book
//     | Chocolate of Chocolate
//     | Wrapped of gift: Gift * wrappingPaperStyle: WrappingPaperStyle
//     | Boxed of Gift
//     | WithACard of gift: Gift * message: string

let rec cata fBook fChocolate fWrapped fBoxed fWithACard (gift: Gift) :'r =

    let recur = cata fBook fChocolate fWrapped fBoxed fWithACard

    match gift with
    | Book book -> fBook book
    | Chocolate chocolate -> fChocolate chocolate

    | Wrapped(inner, wrappingPaperStyle) ->
        let innerResult = recur inner
        fWrapped (innerResult, wrappingPaperStyle)
    | Boxed inner ->
        let innerResult = recur inner
        fBoxed innerResult
    | WithACard(inner, message) ->
        let innerResult = recur inner
        fWithACard (innerResult, message)



// Gift -> string
let description (gift: Gift) : string =
    let fBook book = $"Book titled '{book.title}'"
    let fChocolate chocolate = $"{chocolate.chocolateType} chocolate"
    let fWrapped(innerDescription, wrappingPaperStyle) =
        $"{innerDescription} wrapped in {wrappingPaperStyle} paper"
    let fBoxed innerDescription = $"{innerDescription} in a box"
    let fWithACard(innerDescription, message) =
        $"{innerDescription} with a card saying {message}"

    cata fBook fChocolate fWrapped fBoxed fWithACard gift



// Gift -> decimal
let totalCost gift =
    let fBook (book: Book) = book.price
    let fChocolate chocolate = chocolate.price

    let fWrapped (innerCost, _) = wrappedPrice + innerCost
    let fBoxed innerCost = boxedPrice + innerCost
    let fWithACard (innerCost, _) = withACardPrice + innerCost

    cata fBook fChocolate fWrapped fBoxed fWithACard gift

open Xunit
open Swensen.Unquote

[<Fact>]
let ``description of sample values`` () =
    test <@ description bookGift = "Book titled 'Wolf Hall'" @>
    test <@ description chocolate = "SeventyPercent chocolate" @>
    test <@ description gift1 = "Book titled 'Wolf Hall' wrapped in HappyBirthday paper with a card saying Happy Birthday" @>
    test <@ description gift2 = "SeventyPercent chocolate in a box wrapped in HappyHolidays paper" @>



[<Fact>]
let ``total cost of sample values`` () =
    test <@ totalCost chocolate = 5m @>
    test <@ totalCost bookGift = 20m @>
    test <@ totalCost gift1 = 22.5m @>
    test <@ totalCost gift2 = 6.5m @>


let twice x = 2 * x
