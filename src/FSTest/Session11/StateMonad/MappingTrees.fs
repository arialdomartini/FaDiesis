module FSTest.Session11.StateMonad.MappingTrees

type Tree<'a> =
    | Leaf of 'a
    | Node of Tree<'a> * Tree<'a>

let rec map f tree =
    match tree with
    | Leaf v -> Leaf(f v)
    | Node(l, r) -> Node(map f l, map f r)


let (^) = map

open Xunit
open Swensen.Unquote

[<Fact>]
let ``calculate the leaves' content length`` () =
    let treeOfWords = Node(Leaf "one", Node(Leaf "two", Leaf "three"))
    let treeOfNumbers = Node(Leaf 3, Node(Leaf 3, Leaf 5))

    let (^) = map

    let treeOfLengths = String.length^ treeOfWords

    test <@ treeOfLengths = treeOfNumbers @>
