module FSTest.Session05.Patterns.Content

open System

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

type Content =
    { id: string
      account: string
      text: string
      created: string
      ofType: string }
