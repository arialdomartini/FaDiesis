module FSTest.Session11.StateMonad.IndexingTrees.WalkTheLine

open Xunit
open Swensen.Unquote

module StateMonad =
    type State<'s, 'v> = State of ('s -> 'v * 's)

    let pure' v = State (fun count -> (v, count))

    let run (State f) count = f count

    let (>>=) a f =
        State(fun count ->
            let va, ca = run a count
            let result = f va
            run result ca)

    type StateBuilder() =
        member this.Bind(m, f) = m >>= f
        member this.Return(v) = pure' v
        member this.Zero() = pure' ()

    let state = StateBuilder()

    let read () =
        State (fun count -> (count, count))

    let write v =
        State (fun _ -> ((), v))


open StateMonad

module Walk =

    type Birds = int
    type Pole = Birds * Birds

    let landLeft n (left, right) : Pole option =
        let newLeft = left + n
        if abs(newLeft - right) < 4 then
            Some (newLeft, right)
        else
            None

    let landRight n (left, right) : Pole option =
        let newRight = right + n
        if abs(left - newRight) < 4 then
            Some (left, newRight)
        else
            None

    let landLeftM n =
        state {
            let! pole = read ()
            match landLeft n pole with
            | Some newPole ->
                do! write newPole
                return Some newPole
            | None ->
                return None
        }

    let landRightM n =
        state {
            let! pole = read()
            match landRight n pole with
            | Some newPole ->
                do! write newPole
                return Some newPole
            | None ->
                return None
        }

open Walk

type WalkTheLineTests() =

    [<Fact>]
    member _.``walk the line`` () =
        let routine =
            state {
                let! r1 = landLeftM 1
                let! r2 = landRightM 4
                let! r3 = landLeftM -1
                let! r4 = landRightM -2
                return r4
            }

        let result = run routine (0, 0)

        test <@ result = (Some (2, 2), (2, 2)) @>
