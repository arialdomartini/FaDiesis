module ExpectoSample.ManualTest

open Expecto

module U = Swensen.Unquote.Assertions

[<Tests>]
let tests =
    test "A simple test" {
        let subject = "Hello World"
        U.test <@ subject = "Hello World" @>
    }
