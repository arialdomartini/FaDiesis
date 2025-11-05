// module FSTest.Session05.Tree
//
// type Tree<'v> = CompleteMe of 'v
//
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
// // Functor
// // ('a -> 'b) -> (Tree<'a> -> Tree<'b>)
// let rec map = failwith "Not yet implemented"
// let (<!>) = map
//
// [<Fact>]
// let ``length of string in leaves in a tree using map`` () =
//     let treeWith3Leaves =
//         Node (Leaf "one", Node (Leaf "two", Leaf "three"))
//
//     let len (s: string) = s.Length
//
//     let tree = len <!> treeWith3Leaves
//
//     let expected =
//         Node (Leaf 3, Node (Leaf 3, Leaf 5))
//
//     test <@ tree = expected @>
