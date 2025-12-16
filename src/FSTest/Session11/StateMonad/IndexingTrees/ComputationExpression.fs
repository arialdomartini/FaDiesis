module FSTest.Session11.StateMonad.IndexingTrees.ComputationExpression

open Xunit
open Swensen.Unquote

type Tree<'a> =
    | Leaf of 'a
    | Node of Tree<'a> * Tree<'a>

let buildNode l r = Node (l, r)


type State<'s, 'v> = State of ('s -> 'v * 's)

let pure' v = State (fun count -> (v, count))

let run (State f) count = f count

let (=<<) (f: 'a -> State<'s, 'b>) (a: State<'s, 'a>) =
    State(fun count ->
        let va, ca = run a count
        let result = f va
        run result ca)

let (>>=) wca f = f =<< wca

type StateBuilder() =
    member this.Bind(f, m) = f >>= m
    member this.Return(v) = pure' v



let read () =
    State (fun count -> (count, count))

let write v =
    State (fun _ -> ((), v))


[<Fact>]
let ``indexes a tree`` () =

    let state = StateBuilder()

    let rec index =
        function
        | Leaf v ->
            state {
                let! count = read ()
                do! write (count + 1)
                return Leaf(v, count)
            }

        | Node(l, r) ->
            state {
                let! li = index l
                let! ri = index r

                return buildNode li ri
            }


    let tree = Node(Leaf "one", Node(Leaf "two", Leaf "three"))

    let indexed, _ = run (index tree) 1

    test <@ indexed = Node(Leaf ("one", 1), Node(Leaf ("two", 2), Leaf ("three", 3))) @>
