module FSTest.Session11.StateMonad.IndexingTrees.Applicative


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

let (<*>) (f: ('a -> 'b) WithCount) (v: 'a WithCount) =
    WithCount (fun count ->
        let fv, fc = run f count
        let vv, vc = run v fc
        let rv = fv vv
        (rv, vc))

let (<*) (a: 'a WithCount) (b: 'b WithCount) =
    let f a _ = a
    pure' f <*> a <*> b


let getCount () =
    WithCount (fun count -> (count, count))

let incrementCount () =
    WithCount (fun count -> ((), count + 1))

let rec index =
    function
    | Leaf v ->
        pure' buildLeaf <*> pure' v <*> getCount () <* incrementCount ()
    | Node(l, r) ->

            pure' buildNode <*> (index l) <*> (index r)


[<Fact>]
let ``indexes a tree`` () =
    let tree = Node(Leaf "one", Node(Leaf "two", Leaf "three"))

    let indexed, _ = run (index tree) 1

    test <@ indexed = Node(Leaf ("one", 1), Node(Leaf ("two", 2), Leaf ("three", 3))) @>
