# FaDiesis - Learning F#, hands on

## Day 1

### a + b
- A solution with a C# and a F# project.
  - Interoperability of code.
  - Interoperability of libraries.

- Write a (tested) C# program that calculates the sum of 2
  integers. Translate it 1:1 to F#

  - Types are indicated after parameters.
  - Attributes syntax: `[< >]`.
  - Classes are declared with `type`.
  - F# uses Primary Constructors.
    -  Why? Falling Into The Pit of Success.

- Type Inference and Syntax
  - Remove type hints.
  - Are we using any mutable state?
  - OOP with Instances, FP with Static classes

- Remove classes, use top-level functions

- Function Signatures
  - What is a function?
  - Different function implementations: maps between sets,
    dictionaries, cartesian products.
  - Memoization and closures.
  - The `a -> b -> c` syntax.

- Remove parenthesis and commas from function declarations and
  invocations.
- Tuples and `unit`.


### Concatenating string

- Write a C# function that concatenates `n` repetitions of the input
  string `s`.
- Translate it to F#.

Possible implementations:

```fsharp
let repeatBuilder n s =
    let sb = System.Text.StringBuilder()
    for _ in 1 .. n do
        sb.Append s |> ignore
    sb.ToString()

let rec repeat n s =
    if n <= 0 then ""
    else s + repeat (n - 1) s

let repeatTR n s =
    let rec loop acc i =
        if i = 0 then acc
        else loop (acc + s) (i - 1)
    loop "" n


let repeatSeq n s =
    Seq.init n (fun _ -> s)
    |> String.concat ""

let repeat (n:int) (s:string) =
    String.replicate n s
```

- Mutability
- Recursion


- Extending module `String`:

```fsharp
module String =
    let replicate (count:int) (s:string) =
        ...
```

- Extending type `string`:


```fsharp
type System.String with
    member this.replicate (count:int) =
        ...
```


### Currying

- Add logging to a `StringBuilder`
- Dependency injection via parameters.
- Currying and partial application

- F# Type aliases
- Do the same in C# with delegates


### Building the Building Blocks

- Swap parameters
- Unless
- run `n` times an action
- `also`
- Try/Catch
- Combine
- And
- Curry / Uncurry
- Function composition
- Function application
- Repeat application
- Pipe
- Inverse pipe
- Memoize



## Resources

- [Scott Wlaschin - F# for Fun and Profit][fun-and-profit]
- [Jeff Atwood - Falling Into The Pit of Success][pit-of-success]

[fun-and-profit]: https://book.huihoo.com/dotnet/fsharp-for-fun-and-profit.pdf
[pit-of-success]: https://blog.codinghorror.com/falling-into-the-pit-of-success/
