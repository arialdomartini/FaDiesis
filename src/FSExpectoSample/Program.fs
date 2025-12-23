module FSExpecto.Program

open Expecto
open ExpectoSample
open ExpectoSample.Tests


let integration =
    testList "integration" [ MyTests.``some tests``; ManualTest.tests ]

let originallyIgnored =
    testList
        "originally ignored"
        [ testList "originally ignored"
          <| (MyTests.``this is not discovered`` |> Seq.toList) ]


[<Tests>]
let allTests =
        testList "all"
            [
              // MyTests.numerology
              // integration
              //originallyIgnored
              State.WalkTests.tests ]

[<EntryPoint>]
let main argv =
    //runTestsInAssemblyWithCLIArgs [] argv
    runTestsWithCLIArgs [] argv allTests
