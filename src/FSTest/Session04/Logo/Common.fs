module FSTest.Session04.Logo.Common

open System
open Microsoft.FSharp.Core.CompilerServices

type Distance = float
type MioTipo = MioTipo of float

[<Measure>]
type Degrees


[<Measure>]
type Meters
[<Measure>]
type Seconds
[<Measure>]
type Acceleration = Meters / (Seconds ^ 2)

let x: float<Acceleration> = 12.0<Meters> / 1.0<Seconds> / 2.0<Seconds>

// can also be written
// type [<Measure>] Degrees

type Angle = float<Degrees>

type PenState =
    | PenUp
    | PenDown

type PenColor =
    | Black
    | Red

type Position = { x: float; y: float }


let round (n: float) = Math.Round(n, 2)


// Distance -> Angle -> Position -> Position
let calculateNewPosition (distance: Distance) (angle: Angle) (currentPosition: Position) =

    let project projection n =
        let angleInRads = angle * (Math.PI/180.0 * 1.0<1/Degrees>)
        n + distance * (projection angleInRads)

    let newX = (project cos >> round) currentPosition.x
    let newY = (project sin >> round) currentPosition.y

    {x = newX; y  = newY}

let drawDummyLine log (currentPosition: Position) newPosition (color: PenColor) =
    log $"Draw {color} line from ({currentPosition.x}, {currentPosition.y}) -> ({newPosition.x}, {newPosition.y})"

let mutable logS: string = ""

let stringLog msg =
    logS <- logS + $"\r\n{msg}"
//    logS <- $"{logS}\r\n{msg}"
    ()
