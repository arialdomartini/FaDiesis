module FSTest.Session07.ListMonad.Maze.Implementation.Maze

open ListMonad

type Direction = Up | Down | Left | Right
type Position = int * int

let movePos (x, y) direction =
    match direction with
    | Up -> (x, y + 1)
    | Down -> (x, y - 1)
    | Left -> (x - 1, y)
    | Right -> (x + 1, y)


let findAllPaths (rows, cols) (start: Position) (walls: Set<Position>): Direction list list =
    let isExit (x,y) =
        x = 0
        || x = rows - 1
        || y = 0
        || y = cols - 1

    let isValid (x, y)=
        x >= 0 && x < cols && y >= 0 && y < rows && not (walls.Contains(x, y))


    let rec explore current path visited =

        let neverVisited pos =
            not (Set.contains pos visited)

        list {
            if current |> isExit then
                return List.rev path
            else
                let! nextDir = [Up; Down; Left; Right]
                let nextPos = movePos current nextDir

                if nextPos |> isValid
                   && nextPos |> neverVisited then
                    let newVisited = Set.add nextPos visited
                    return! explore nextPos (nextDir :: path) newVisited
        }

    explore start [] (Set.singleton start)



let toInstructions (directions: Direction list) =
    let folder currentDirection (instructions: (int * Direction) list) =
        match instructions with
        | (count, lastDirection) ::rest when lastDirection = currentDirection -> (count + 1, lastDirection) :: rest
        | instructionsSoFar -> (1, currentDirection) :: instructionsSoFar

    List.foldBack folder directions []
    |> List.map (fun (count, dir) -> $"{count} {dir}")
