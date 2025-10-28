module FSTest.Session04.Logo.Common

open System

type Distance = float

[<Measure>]
type Degrees

// can also be written
// type [<Measure>] Degrees

type Angle = float<Degrees>

type PenState =

type PenColor =

type Position = { x: float;


let round (n: float) = Math.Round(n, 2)

let calculateNewPosition (distance: Distance) (angle: Angle) (currentPosition: Position) =

    let project projection n =
        let angleInRads = angle * (Math.PI/180.0 * 1.0<1/Degrees>)
        n + distance * (projection angleInRads)

    let newX = (project cos >> round) currentPosition.x
    let newY = (project sin >> round) currentPosition.y



let drawDummyLine log currentPosition newPosition color =
    log $"Draw {color} line from ({currentPosition.x}, {currentPosition.y}) -> ({newPosition.x}, {newPosition.y})"

let mutable logS = ""

let stringLog msg =
    logS <- $"{logS}\r\n{msg}"
    ()
