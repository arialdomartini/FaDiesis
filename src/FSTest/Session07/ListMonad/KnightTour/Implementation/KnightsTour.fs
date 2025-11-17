module FSTest.Session07.ListMonad.KnightTour.Implementation.KnightsTour

open ListMonad

module Knight =
    type Position = int * int

    let nextPositions (x: int, y: int) : Position list =
        [ (x + 1, y + 2)
          (x + 2, y + 1)
          (x + 2, y - 1)
          (x + 1, y - 2)
          (x - 1, y - 2)
          (x - 2, y - 1)
          (x - 2, y + 1)
          (x - 1, y + 2) ]

    let doTour (boardSize: int) (start: Position) =
        let totalSquares = boardSize * boardSize

        let withinTheBoard ((x, y): Position) =
            x >= 0 && x < boardSize && y >= 0 && y < boardSize

        let rec explore current path visited =
            let notYetVisited position = not (Set.contains position visited)
            let markAsVisited position = Set.add position visited
            let visitedAll = visited |> Set.count = totalSquares

            list {
                if visitedAll then
                    return current :: path
                else
                    let! nextPosition = nextPositions current

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
