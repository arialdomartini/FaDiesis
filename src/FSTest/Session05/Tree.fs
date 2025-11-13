module FSTest.Session05.Tree
//
//
//
// type Tree<'v> =
//     | Leaf of 'v
//     | Node of ('v Tree * 'v Tree)
//
// // len : string ->int
// // lenT : Tree<string> -> Tree<int>
// // lenT : map len
//
// open Xunit
// open Swensen.Unquote
//
// let numberOfLeaves (tree: 'v Tree) = failwith "Not yet implemented"
//
// [<Fact>]
// let ``counts the number of leaves in a tree`` () =
//     let treeWith3Leaves: Tree<int> =
//         Node (Leaf 1, Node (Leaf 2, Leaf 3))
//
//     let leaves = numberOfLeaves treeWith3Leaves
//
//     test <@ leaves = 3 @>
//
//
// // (a -> b) -> (Tree<a> -> Tree<b>)
// let rec map (f: 'a -> 'b) : (Tree<'a> -> Tree<'b>) =
//     fun (tA: Tree<'a>) ->
//         match tA with
//         | Leaf a -> Leaf (f a)
//         | Node (l, r) -> Node (map f l, map f r)
//
//
// let (<!>) = map
// let len (s: string) = s.Length
//
// let lenT = map len
//
// [<Fact>]
// let ``manual length of string in leaves in a tree using map`` () =
//     let treeWith3Leaves =
//         Node (Leaf "one", Node (Leaf "two", Leaf "three"))
//
//     let tree = lenT treeWith3Leaves
//
//     let expected =
//         Node (Leaf 3, Node (Leaf 3, Leaf 5))
//
//     test <@ tree = expected @>
//
// [<Fact>]
// let ``length of string in leaves in a tree using map`` () =
//     let treeWith3Leaves: Tree<string> =
//         Node (Leaf "one", Node (Leaf "two", Leaf "three"))
//
//     let len (s: string) = s.Length
//
//     let tree = len <!> treeWith3Leaves
//     // let lenF = map len
//     // let tree = lenF treeWith3Leaves
//     //
//     let expected =
//         Node (Leaf 3, Node (Leaf 3, Leaf 5))
//
//     test <@ tree = expected @>
//
// //    Node
