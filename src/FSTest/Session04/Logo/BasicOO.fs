module FSTest.Session04.Logo.BasicOO
open Common


type Turtle(log) =
    let initialPosition = {Position.x = 0; y  = 0}

    // member private val currentPosition = initialPosition with get, set
    let mutable currentPosition = initialPosition
    let mutable currentAngle: float<Degrees> = 0.0<Degrees>
    let mutable currentPenStatus = PenDown
    let mutable currentPenColor = Black

    member this.Move(distance) =
        let newPosition = calculateNewPosition distance currentAngle currentPosition
        match currentPenStatus with
        | PenUp -> log $"Move to ({newPosition.x}, {newPosition.y})"
        | PenDown -> drawDummyLine log currentPosition newPosition currentPenColor

        currentPosition <- newPosition

    member this.Turn(angle) =
        let newAngle = (currentAngle + angle) % 360.0<Degrees>
        log $"Turn: {angle} from {currentAngle} -> {newAngle}"
        currentAngle <- newAngle

    member this.PenUp() =
        currentPenStatus <- PenUp

    member this.PenDown() =
        currentPenStatus <- PenDown

    member this.SetColor(penColor) =
        currentPenColor <- penColor


open Xunit

[<Fact(Skip = "Cannot be run in parallel, because of mutability!")>]
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
