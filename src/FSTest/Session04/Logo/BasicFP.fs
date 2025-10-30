module FSTest.Session04.Logo.BasicFP

open Common
open FSTest.Session04.Logo.Common

module Turtle =

    type TurtleState =
        { position: Position
          angle: Angle
          penColor: PenColor
          penState: PenState }

    let move log distance state =
        let newPosition = calculateNewPosition distance state.angle state.position

        match state.penState with
        | PenUp -> log $"Move to ({newPosition.x}, {newPosition.y})"
        | PenDown -> drawDummyLine log state.position newPosition state.penColor

        { state with position = newPosition }

    let turn log angle state =
        let newAngle = (state.angle + angle) % 360.0<Degrees>
        log $"Turn: {angle} from {state.angle} -> {newAngle}"
        { state with angle = newAngle }

    let penUp state = { state with penState = PenUp }

    let penDown state = { state with penState = PenDown }

    let setColor penColor state = { state with penColor = penColor }


open Xunit
open Turtle

// pipe
// (a -> b) -> a -> b
// let a |> f = f a


// (State -> State) -> (State -> State) -> (State -> State)
//  move 100         -> turn 120        -> move+turn


[<Fact>]
let ``draw a triangle`` () =
    // Composition Root
    let log = stringLog
    // Distance -> State -> State
    let move = move log

    // Angle -> State -> State
    let turn = turn log

    let initialState =
        { position = { x = 0; y = 0 }
          angle = 0.0<Degrees>
          penColor = Black
          penState = PenDown }

    // functional style
    let move100 = move 100

    let turn120 = turn 120.0<Degrees>

    let triangle state =
        state
        |> move 100
        |> turn120
        |> penUp
        |> move100
        |> turn120
        |> penDown
        |> setColor Red
        |> move100
        |> turn120
        |> ignore


    // Tacit , Point-Free
    let paintTriangle =
        move100
        >> turn120
        >> penUp
        >> move100
        >> turn120
        >> penDown
        >> setColor Red
        >> move100
        >> turn120

    let side = move100 >> turn 90.0<Degrees>
    let square = side >> side >> side >> side
    let squareAndThenTriangle = square >> paintTriangle

    triangle initialState

    Assert.Equal(
        """
Draw Black line from (0, 0) -> (100, 0)
Turn: 120 from 0 -> 120
Move to (50, 86.6)
Turn: 120 from 120 -> 240
Draw Red line from (50, 86.6) -> (-0, -0)
Turn: 120 from 240 -> 0""",
        logS
    )
