module FSharpBits.ForFunAndProfit.Catamorphism.Memoization

open System.Collections.Generic

// (a -> b) -> (a -> b)
let memoize (f: 'a ->'b) =
    let cache = Dictionary<'a, 'b>()

    fun a ->
        if cache.ContainsKey(a)
        then cache[a]
        else
            let b = f a
            cache.Add(a, b)
            b


let twice n = n * 2

let twiceCached = memoize twice
