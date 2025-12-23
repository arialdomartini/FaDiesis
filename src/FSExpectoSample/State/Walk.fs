module State.Walk

module StateMonad =
    type State<'s, 'v> = State of ('s -> 'v * 's)

    let pure' v = State (fun count -> (v, count))

    let run (State f) count = f count
    let exec f count =
        let _, s = run f count
        s

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

    type Man =
        | Walking of Pole
        | FallenDown

    let (>>==) man f =
        match man with
        | Walking pole -> f pole
        | FallenDown -> FallenDown

    let mapMan f man =
        match man with
        | Walking pole -> Walking (f pole)
        | FallenDown -> FallenDown

    let landLeft n (left, right) =
        let newLeft = left + n
        if abs(newLeft - right) < 4 then
            Walking (newLeft, right)
        else
            FallenDown

    let landRight n (left, right) =
        let newRight = right + n
        if abs(left - newRight) < 4 then
            Walking (left, newRight)
        else
            FallenDown

    let landLeftM n =
        state {
            let! manState = read ()
            match manState with
            | FallenDown -> return FallenDown  // Se già caduto, non fare nulla
            | Walking pole ->
                match landLeft n pole with
                | Walking newPole ->
                    do! write (Walking newPole)
                    return Walking newPole
                | FallenDown ->
                    do! write FallenDown
                    return FallenDown
        }

    let landRightM n =
        state {
            let! manState = read ()
            match manState with
            | FallenDown -> return FallenDown  // Se già caduto, non fare nulla
            | Walking pole ->
                match landRight n pole with
                | Walking newPole ->
                    do! write (Walking newPole)
                    return Walking newPole
                | FallenDown ->
                    do! write FallenDown
                    return FallenDown
        }

    // let landLeftM n =
    //     state {
    //         let! manState = read ()
    //         let newState = manState >>== (landLeft n)
    //         do! write newState
    //         return ()
    //     }
    //
    // let landRightM n =
    //     state {
    //         let! manState = read ()
    //         let newState = manState >>== (landRight n)
    //         do! write newState
    //         return ()
    //     }
