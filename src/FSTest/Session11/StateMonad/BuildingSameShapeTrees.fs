module FSTest.Session11.StateMonad.BuildingSameShapeTrees

type Tree<'a> =
    | Leaf of 'a
    | Node of Tree<'a> * Tree<'a>

let baseCase _ = 1
let baseCase' v = Leaf(String.length v)
let (^+) l r = Node (l, r)

let rec numberOfLeaves =
    function
    | Leaf v     -> baseCase v
    | Node(l, r) -> numberOfLeaves l + numberOfLeaves r

let rec lengths =
    function
    | Leaf v     -> baseCase' v
    | Node(l, r) -> lengths l ^+ lengths r


open Xunit
open Swensen.Unquote

[<Fact>]
let ``calculate the leaves' content length`` () =
    let treeOfWords = Node(Leaf "one", Node(Leaf "two", Leaf "three"))
    let treeOfNumbers = Node(Leaf 3, Node(Leaf 3, Leaf 5))

    let treeOfLengths = lengths treeOfWords

    test <@ treeOfLengths = treeOfNumbers @>
