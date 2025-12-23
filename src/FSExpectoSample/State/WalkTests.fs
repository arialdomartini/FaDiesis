module State.WalkTests

open Expecto
open State.Walk.StateMonad
open State.Walk.Walk

[<Tests>]
let tests =
    testList
        "Walk the line"
        [ test "walk the line" {

              let routine =
                  state {
                      do! landLeftM 1
                      do! landRightM 2
                      do! landLeftM -1
                      do! landRightM -2
                  }

              let result = exec routine (Walking (0, 0))

              Expect.equal result (Walking (0, 0)) "free again!"

          }

          test "the poor guy falls" {

              let routine =
                  state {
                      do! landLeftM 1
                      do! landLeftM 2
                      do! landLeftM 5
                      do! landRightM 1
                  }

              let result = exec routine (Walking (0, 0))

              Expect.equal result FallenDown "same amount!"

          } ]
