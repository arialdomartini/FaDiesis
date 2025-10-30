module FSTest.Session05.Patterns.ApplicativeBuilder

open System
open Xunit
open Swensen.Unquote

type Result<'success> =
    | Ok of 'success
    | Failure of string list

type Email = Email of string

type Role =
    | User
    | Administrator

type Account =
    { name: string
      email: Email
      role: Role }

type NoteType =
    | ShopList
    | Reminder
    | Secrets

type NoteNumber = NoteNumber of int

type Note =
    { number: NoteNumber
      account: Account
      content: string
      createdAt: DateTime
      noteType: NoteType }

let buildNote (number: NoteNumber) (account: Account) (content: string) (createdAt: DateTime) (noteType: NoteType) =
    { number = number
      account = account
      content = content
      createdAt = createdAt
      noteType = noteType }

let pure' a = Ok a

let (<!>) (f: 'a -> 'b) (a: Result<'a>) =
    match a with
    | Ok resultValue -> Ok(f resultValue)
    | Failure errorValue -> Failure errorValue

let (<*>) (f: Result<'a -> 'b>) (a: Result<'a>) =
    match f with
    | Ok fOk ->
        match a with
        | Ok aOK -> Ok(fOk aOK)
        | Failure aFailure -> Failure aFailure
    | Failure fFailure ->
        match a with
        | Ok aOK -> Failure fFailure
        | Failure aFailure -> Failure(aFailure @ fFailure)

let (>>=) (a: Result<'a>) (f: 'a -> Result<'b>) =
    match a with
    | Ok aOK -> f aOK
    | Failure aFailure -> Failure aFailure

let getAccount nickName =
    match nickName with
    | n when n = "joe" ->
        Ok
            { name = "Joe"
              email = Email "joe.doe@example.com"
              role = User }
    | n when n = "jane" ->
        Ok
            { name = "Jane"
              email = Email "jane.doe@example.com"
              role = Administrator }
    | n -> Failure [ $"Account for '{n}' could not be found" ]

type Content =
    { id: string
      account: string
      text: string
      created: string
      ofType: string }

let getContent number : Content Result =
    match number with
    | NoteNumber n when n = 42 ->
        Ok
            { id = "42"
              account = "joe"
              text = "This is note 42"
              created = "2025-12-11"
              ofType = "secret" }
    | NoteNumber n when n = 99 ->
        Ok
            { id = "99"
              account = "jan"
              text = "This is note 99"
              created = "25-12-"
              ofType = "s�cret" }
    | NoteNumber n -> Failure [ $"Note {n} does not exist" ]

let parseDate (s: string) : DateTime Result =
    let success, dateTime = DateTime.TryParse(s)

    match success with
    | true -> Ok dateTime
    | false -> Failure [ $"{s} is not a valid date" ]

let parseNoteType (s: string) : NoteType Result =
    match s with
    | s when s = "secret" -> Ok Secrets
    | s when s = "shopping" -> Ok ShopList
    | s when s = "todo" -> Ok Reminder
    | _ -> Failure [ $"'{s}' is not a valid note type" ]

let parseNumber (s: string) =
    let success, i = Int32.TryParse s

    match success with
    | true -> Ok(NoteNumber i)
    | false -> Failure [ $"{i} is not a number" ]


let getNote (noteNumber: NoteNumber) =
    let content = getContent noteNumber

    let build content =
        let number = parseNumber content.id
        let account = getAccount content.account
        let contentText = pure' content.text
        let createdAt = parseDate content.created
        let noteType = parseNoteType content.ofType

        buildNote <!> number <*> account <*> contentText <*> createdAt <*> noteType

    content >>= build

[<Fact>]
let ``successfully builds an Account`` () =

    let account = getAccount "joe"

    test
        <@
            account = Ok
                { name = "Joe"
                  email = Email "joe.doe@example.com"
                  role = User }
        @>

    let account = getAccount "unknown"

    test <@ account = Failure [ "Account for 'unknown' could not be found" ] @>

[<Fact>]
let ``Successfully found`` () =

    let note = getNote (NoteNumber 42)

    let expected =
        Ok
            { number = NoteNumber 42
              account =
                { name = "Joe"
                  email = Email "joe.doe@example.com"
                  role = User }
              content = "This is note 42"
              createdAt = DateTime(2025, 12, 11)
              noteType = Secrets }

    test <@ note = expected @>

[<Fact>]
let ``Not found note`` () =

    let note = getNote (NoteNumber 12345)

    test <@ note = Failure [ "Note 12345 does not exist" ] @>

[<Fact>]
let ``Accumulation of errors`` () =

    let note = getNote (NoteNumber 99)

    test <@
            note = Failure
                    [ "'s�cret' is not a valid note type"
                      "25-12- is not a valid date"
                      "Account for 'jan' could not be found" ] @>
