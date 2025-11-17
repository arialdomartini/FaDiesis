module FSTest.Session07.ListMonad.Maze.Solution.Maze

open ListMonad

type Direction = Up | Down | Left | Right
type Position = int * int

let movePos (x, y) direction =
    match direction with
    | Up -> (x, y + 1)
    | Down -> (x, y - 1)
    | Left -> (x - 1, y)
    | Right -> (x + 1, y)

let isValid (rows, cols) (x, y) (walls: Set<Position>) =
    x >= 0 && x < cols && y >= 0 && y < rows && not (walls.Contains(x, y))

let findAllPaths (rows, cols) (start: Position) (goal: Position) (walls: Set<Position>) =
    let rec explore current path visited =
        list {
            if current = goal then
                return List.rev (current :: path)
            else
                let! nextDir = [Up; Down; Left; Right]
                let nextPos = movePos current nextDir

                if isValid (rows, cols) nextPos walls && not (Set.contains nextPos visited) then
                    let newVisited = Set.add nextPos visited
                    let! result = explore nextPos (current :: path) newVisited
                    return result
        }

    explore start [] (Set.singleton start)

open Xunit
open Swensen.Unquote

(*
7 ###### #
6 #   ## #
5 # #### #
4 # ###  # 
3 #     ##
2 #### ###
1      ###
0  #######
  01234567
*)

[<Fact>]
let ``find the exit!`` () =

    let walls = set [

        (0,7); (1,7); (2,7); (3,7); (4,7); (5,7);        (7,7);
        (0,6);                      (4,6); (5,6);        (7,6);
        (0,5);        (2,5); (3,5); (4,5); (5,5);        (7,5);
        (0,4);        (2,4); (3,4); (4,4);               (7,4);
        (0,3);                                    (6,3); (7,3);
        (0,2); (1,2); (2,2); (3,2);        (5,2); (6,2); (7,2);
                                           (5,1); (6,1); (7,1);      
               (1,0); (2,0); (3,0); (4,0); (5,0); (6,0); (7,0);      
    ]


    let paths = findAllPaths (7, 7) (0,0) (6,6) walls

    test <@ paths |> List.length = 1 @>
