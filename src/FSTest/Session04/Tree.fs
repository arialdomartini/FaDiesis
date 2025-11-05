module FSTest.Session04.Tree

type Tree<'v> =
    | Node of (Tree<'v> * Tree<'v>)
    | Leaf of 'v

open Xunit
open Swensen.Unquote

let rec numberOfLeaves (tree: 'v Tree) =
    match tree with
    | Leaf _ -> 1
    | Node (l, r) -> numberOfLeaves l + numberOfLeaves r

[<Fact>]
let ``counts the number of leaves in a tree`` () =
    let treeWith3Leaves: Tree<int> =
        Node (Leaf 1, Node (Leaf 2, Leaf 3))

    let leaves = numberOfLeaves treeWith3Leaves

    test <@ leaves = 3 @>

let (++) l r = Node(l, r)

let rec traverseTree (tree: string Tree) =
    match tree with
    | Leaf s -> Leaf s.Length
    | Node (l, r) ->
        traverseTree l ++ traverseTree r

// box metaphor
// Tree<'a> -> ('a -> 'b) -> Tree<'b>
let rec map' (tree: 'a Tree) f =
    match tree with
    | Leaf s -> Leaf (f s)
    | Node (l, r) ->
        let l' = map' l f
        let r' = map' r f
        Node (l', r')

// Functor
// ('a -> 'b) -> (Tree<'a> -> Tree<'b>)
let rec map f (tree: 'a Tree) =
    match tree with
    | Leaf s -> Leaf (f s)
    | Node (l, r) ->
        let l' = map f l
        let r' = map f r
        Node (l', r')


let (<!>) = map

[<Fact>]
let ``length of string in leaves in a tree`` () =
    let treeWith3Leaves =
        Node (Leaf "one", Node (Leaf "two", Leaf "three"))

    let tree = traverseTree treeWith3Leaves

    let expected =
        Node (Leaf 3, Node (Leaf 3, Leaf 5))

    test <@ tree = expected @>

[<Fact>]
let ``length of string in leaves in a tree using map`` () =
    let treeWith3Leaves =
        Node (Leaf "one", Node (Leaf "two", Leaf "three"))

    let len (s: string) = s.Length

    let tree = len <!> treeWith3Leaves

    let expected =
        Node (Leaf 3, Node (Leaf 3, Leaf 5))

    test <@ tree = expected @>
