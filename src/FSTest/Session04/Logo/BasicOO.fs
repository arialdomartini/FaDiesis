module FSTest.Session04.Logo.BasicOO
open Common


type Turtle(log) =
    let initialPosition = {x = 0; y = 0}

    // member private val currentPosition = initialPosition with get, set
    let mutable currentPosition = initialPosition


    member this.Move(distance) =
        let newPosition = calculateNewPosition distance currentAngle currentPosition
        failwith "Not yet implemented"

    member this.Turn(angle) =
        let newAngle = (currentAngle + angle) % 360.0<Degrees>
        log $"Turn: {angle} from {currentAngle} -> {newAngle}"
        failwith "Not yet implemented"



open Xunit

[<Fact>]
let ``draw a triangle`` () =
    let turtle = Turtle(stringLog)

    turtle.Move 100
    turtle.Turn 120.0<Degrees>
    turtle.PenUp()
    turtle.Move 100
    turtle.Turn 120.0<Degrees>
    turtle.PenDown()
    turtle.SetColor Red
    turtle.Move 100
    turtle.Turn 120.0<Degrees>

    Assert.Equal("""
Draw Black line from (0, 0) -> (100, 0)
Turn: 120 from 0 -> 120
Move to (50, 86.6)
Turn: 120 from 120 -> 240
Draw Red line from (50, 86.6) -> (-0, -0)
Turn: 120 from 240 -> 0""", logS)
