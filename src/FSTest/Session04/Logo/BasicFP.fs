// module FSTest.Session04.Logo.BasicFP
// open Common
//
// module Turtle =
//
//     type TurtleState =
//         { position: Position }
//
//     let move log distance state =
//         let newPosition = calculateNewPosition distance state.angle state.position
//         failwith "Not yet implemented"
//
//
// open Xunit
// open Turtle
//
// [<Fact>]
// let ``draw a triangle`` () =
//
//     let initialState =
//         { position = {x = 0; y = 0}
//           angle = 0.0<Degrees>
//           penColor = Black
//           penState = Down }
//
//     let log = stringLog
//
//     let state1 = move log 100 initialState
//     let state2 = turn log 120.0<Degrees> state1
//     let state3 = penUp state2
//     let state4 = move log 100 state3
//     let state5 = turn log 120.0<Degrees> state4
//     let state6 = penDown state5
//     let state7 = secColor Red state6
//     let state8 = move log 100 state7
//     let state9 = turn log 120.0<Degrees> state8
//     state9|> ignore
//
//
//     Assert.Equal("""
// Draw Black line from (0, 0) -> (100, 0)
// Turn: 120 from 0 -> 120
// Move to (50, 86.6)
// Turn: 120 from 120 -> 240
// Draw Red line from (50, 86.6) -> (-0, -0)
// Turn: 120 from 240 -> 0""", logS)
