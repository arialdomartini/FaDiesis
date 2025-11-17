module FSTest.Session07.ListMonad.KnightTour.Usage.KnightsTour

open ListMonad

module Knight =
    type Position = int * int

    let nextPositions (x: int, y: int) : Position list = failwith "Not yet implemented"

    let doTour (boardSize: int) (start: Position) =
        let totalSquares = boardSize * boardSize

        let withinTheBoard ((x, y): Position) = failwith "Not yet implemented"

        let rec explore current path visited =
            let notYetVisited position = failwith "Not yet implemented"
            let markAsVisited position = failwith "Not yet implemented"
            let visitedAll = failwith "Not yet implemented"

            list {
                if visitedAll then failwith "Not yet implemented"
                else
                    let! nextPosition = failwith "Not yet implemented"

                    if nextPosition |> withinTheBoard && nextPosition |> notYetVisited then
                        let newVisited = markAsVisited nextPosition
                        return! explore nextPosition (current :: path) newVisited
                    else
                        return! []
            }

        explore start [] (Set.singleton start)

open Xunit
open Swensen.Unquote

[<Fact>]
let ``find resolution`` () =
    let solutions = Knight.doTour 5 (0, 0)

    let length = solutions |> List.length
    test <@ length = 304 @>
