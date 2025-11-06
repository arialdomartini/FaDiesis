module FSTest.Session05.Patterns.Repositories
//
// open System
// open Content
// open Result
//
// let getAccount nickName =
//     match nickName with
//     | n when n = "joe" ->
//         Ok
//             { name = "Joe"
//               email = Email "joe.doe@example.com"
//               role = User }
//     | n when n = "jane" ->
//         Ok
//             { name = "Jane"
//               email = Email "jane.doe@example.com"
//               role = Administrator }
//     | n -> Failure [ $"Account for '{n}' could not be found" ]
//
//
// let getContent number : Content Result =
//     match number with
//     | NoteNumber n when n = 42 ->
//         Ok
//             { id = "42"
//               account = "joe"
//               text = "This is note 42"
//               created = "2025-12-11"
//               ofType = "secret" }
//     | NoteNumber n when n = 99 ->
//         Ok
//             { id = "99"
//               account = "jan"
//               text = "This is note 99"
//               created = "25-12-"
//               ofType = "s�cret" }
//     | NoteNumber n -> Failure [ $"Note {n} does not exist" ]
//
// let parseDate (s: string) : DateTime Result =
//     let success, dateTime = DateTime.TryParse(s)
//
//     match success with
//     | true -> Ok dateTime
//     | false -> Failure [ $"{s} is not a valid date" ]
//
// let parseNoteType (s: string) : NoteType Result =
//     match s with
//     | s when s = "secret" -> Ok Secrets
//     | s when s = "shopping" -> Ok ShopList
//     | s when s = "todo" -> Ok Reminder
//     | _ -> Failure [ $"'{s}' is not a valid note type" ]
//
// let parseNumber (s: string) =
//     let success, i = Int32.TryParse s
//
//     match success with
//     | true -> Ok(NoteNumber i)
//     | false -> Failure [ $"{i} is not a number" ]
