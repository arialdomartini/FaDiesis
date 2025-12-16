module FSTest.Session11.StateMonad.IndexingTrees.Mutable


type Tree<'a> =
    | Leaf of 'a
    | Node of Tree<'a> * Tree<'a>

let rec map f tree =
    match tree with
    | Leaf v -> Leaf(f v)
    | Node(l, r) -> Node(map f l, map f r)


open Xunit
open Swensen.Unquote

[<Fact>]
let ``indexes a tree`` () =

    let mutable counter = 1
    let index v =
        let indexedLeaf = (v, counter)
        counter <- counter + 1
        indexedLeaf


    let tree = Node(Leaf "one", Node(Leaf "two", Leaf "three"))

    let indexed = map index tree

    test <@ indexed = Node(Leaf ("one", 1), Node(Leaf ("two", 2), Leaf ("three", 3))) @>
