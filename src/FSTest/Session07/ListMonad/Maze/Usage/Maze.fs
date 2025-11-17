module FSTest.Session07.ListMonad.Maze.Usage.Maze

open ListMonad

type Direction = Up | Down | Left | Right
type Position = int * int

let movePos (x, y) direction = failwith "Not yet implemented"


let findAllPaths (rows, cols) (start: Position) (walls: Set<Position>) =
    let isExit (x,y) = failwith "Not yet implemented"

    let isValid (x, y)= failwith "Not yet implemented"

    let neverVisited pos = failwith "Not yet implemented"


    let rec explore current path visited =

        list {
            if current |> isExit then failwith "Not yet implemented"
            else
                let possibleDirections = [Up; Down; Left; Right]
                let nextPos = failwith "Not yet implemented"

                if nextPos |> isValid
                   && nextPos |> neverVisited then
                    failwith "Not yet implemented"
        }

    explore start [] (Set.singleton start)



let toInstructions (directions: Direction list) = failwith "Not yet implemented"
