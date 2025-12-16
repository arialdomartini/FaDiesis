module FSTest.Session11.StateMonad.IndexingTrees.Monad


type Tree<'a> =
    | Leaf of 'a
    | Node of Tree<'a> * Tree<'a>

open Xunit
open Swensen.Unquote


type WithCount<'v> = WithCount of (int -> 'v * int)

let run (WithCount f) count = f count

let buildNode l r = Node (l, r)

let buildLeaf v count = Leaf (v, count)

let pure' v = WithCount (fun count -> (v, count))

let (=<<) (f: 'a -> 'b WithCount) (a: 'a WithCount) =
    WithCount(fun count ->
        let va, ca = run a count
        let result = f va
        run result ca)

let (>>=) wca f = f =<< wca

let rec index =
    function
    | Leaf v -> WithCount (fun count -> Leaf(v, count), count + 1)
    | Node(l, r) ->

        let li = index l
        let ri = index r

        let buildNode' l r = pure' (buildNode l r)


        (li >>= fun l ->
            ri >>= fun r -> buildNode' l r)


[<Fact>]
let ``indexes a tree`` () =
    let tree = Node(Leaf "one", Node(Leaf "two", Leaf "three"))

    let indexed, _ = run (index tree) 1

    test <@ indexed = Node(Leaf ("one", 1), Node(Leaf ("two", 2), Leaf ("three", 3))) @>
