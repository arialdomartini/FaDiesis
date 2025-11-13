module FSTest.Session06.Patterns.ApplicativeBuilder
//
// open System
// open Xunit
// open Swensen.Unquote
// open Content
// open Result
// open Repositories
//
// let buildNote (number: NoteNumber) (account: Account) (content: string) (createdAt: DateTime) (noteType: NoteType) =
//     { number = number
//       account = account
//       content = content
//       createdAt = createdAt
//       noteType = noteType }
//
//
// let getNote (noteNumber: NoteNumber) =
//
//     let build content =
//         let number = parseNumber content.id
//         let account = failwith "Not yet implemented"
//         let contentText = failwith "Not yet implemented"
//         let createdAt = failwith "Not yet implemented"
//         let noteType = failwith "Not yet implemented"
//
//         buildNote <!> number <*> account <*> contentText <*> createdAt <*> noteType
//
//     let content = getContent noteNumber
//
//     build content
//
// [<Fact>]
// let ``successfully builds an Account`` () =
//
//     let account = parseAccount "joe"
//
//     let expected =
//         Ok
//             { name = "Joe"
//               email = Email "joe.doe@example.com"
//               role = User }
//
//     test <@ account = expected @>
//
//     let account = parseAccount "unknown"
//
//     test <@ account = Failure [ "Account for 'unknown' could not be found" ] @>
//
// [<Fact>]
// let ``Successfully found Note`` () =
//
//     let note = getNote (NoteNumber 42)
//
//     let expected =
//         Ok
//             { number = NoteNumber 42
//               account =
//                 { name = "Joe"
//                   email = Email "joe.doe@example.com"
//                   role = User }
//               content = "This is note 42"
//               createdAt = DateTime(2025, 12, 11)
//               noteType = Secrets }
//
//     test <@ note = expected @>
//
// [<Fact>]
// let ``Not found Note`` () =
//
//     let note = getNote (NoteNumber 12345)
//
//     test <@ note = Failure [ "Note 12345 does not exist" ] @>
//
// [<Fact>]
// let ``Accumulation of errors`` () =
//
//     let note = getNote (NoteNumber 99)
//
//     test
//         <@
//             note = Failure
//                 [ "'s�cret' is not a valid note type"
//                   "25-12- is not a valid date"
//                   "Account for 'jan' could not be found" ]
//         @>
