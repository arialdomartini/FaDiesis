module ExpectoSample.Tests

open System
open Expecto
module U = Swensen.Unquote.Assertions

module MyTests =

    let ``this is not discovered``: Test seq =
        testParam 42 [ "not discovered", fun value () -> Expect.isTrue (value = 42) "OK!" ]

    let s = "should have been true!"

    let parametric i =
        testParam
            i
            [ ($"parametric test 1 case {i}", fun a () -> Expect.equal a i s)
              ($"parametric test 2 case {i}", fun a () -> Expect.equal a i s)
              ($"parametric test 3 case {i}", fun a () -> Expect.equal a i s)
              ($"parametric test 4 case {i}", fun a () -> Expect.equal a i s) ]


    let hardOrSimple n =
        if n > 10
        then "Super heavy test"
        else "Easy peacy"

    let case (x: int) : Test =
        testCase $"Let's test with number: {x}" (fun () -> Expect.isTrue (x > 0) "should have been positive!")


    let localTest = testCase "locally" (fun () -> failwith "whatever")
    let remoteTest cpu = testCase "remotely" (fun () -> failwith "whatever")

    let makeTest: Test =
        if Environment.GetEnvironmentVariable("config") = "dev"
        then localTest
        else remoteTest Environment.MachineName


    let case2: Test = testCase "an ordinary test case 2" (fun () -> Expect.isTrue true "should have been true!")

    let case3 =
        testCase "an ordinary test case 3"
        <| fun () -> Expect.isTrue true "should have been true!"

    let case4 =
        testCase "an ordinary test case 4"
        <| fun () -> Expect.isTrue true "should have been true!"

    let case5 =
        testCase "an ordinary test case 5"
        <| fun () -> Expect.isTrue true "should have been true!"

    let parametricTests =
        testList "some parametric tests"
        <| (([ 1..10 ] |> Seq.collect parametric) |> Seq.toList)


    type MyDisposable() =
        member this.Value = 42
        interface IDisposable with
            member _.Dispose() = ()

    let factory test =
        // setup
        use ms = new MyDisposable()

        test ms

        // here your tear down
        //(ms :> IDisposable).Dispose()
        //result

    let withFixture: Test list =
        [ yield!
            testFixture factory [ "can read", fun ms -> (fun () -> Expect.isTrue (ms.Value = 42) s) ] ]


    let ``some tests``: Test =
        testList "test name"
        <| [ testList "some cases" [ case 42; case2; case3; case4; ]

             parametricTests

             test "A simple test" {
                 let subject = "Hello World"
                 U.test <@ subject = "Hello World" @>
             }

             testList "with fixture" withFixture ]


    let numerology =
        testList "numberology 101" (
          testParam 1333 [
            "First sample",
              fun value () ->
                Expect.equal value 1333 "Should be expected value"
            "Second sample",
              fun value () ->
                Expect.isLessThan value 1444 "Should be less than"
        ] |> List.ofSeq)

    let test1 value = testCase "First sample - manual" <| fun ()-> Expect.equal value 1333 "Should be expected value"
    let test2 value = testCase "Second sample - manual" <| fun ()-> Expect.equal value 1333 "Should be expected value"

    let test1a value () = Expect.equal value 1333 "Should be expected value"
    let test2a value () = Expect.equal value 1333 "Should be expected value"

    let numerology2 =
        testList "numberology 101 - manual"
          ([ test1 ; test2 ] |> List.map (fun f -> f 1333))

    let numerology3 =
        testList "numberology 101 - compact"
            (( testParam 1333 [ "first", test1a; "second", test2a ]) |> List.ofSeq)
