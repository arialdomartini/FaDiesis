## Day 3

### Giraffe!

#### Create solution

```bash
mkdir GiraffeDemo
cd GiraffeDemo
dotnet new sln --name GiraffeDemo
mkdir src

dotnet new install "giraffe-template::*"
dotnet new giraffe -o src/GiraffeDemo
dotnet sln add src/GiraffeDemo/GiraffeDemo.fsproj

dotnet new xunit -o src/GiraffeDemo.Test -lang F#
dotnet sln add src/GiraffeDemo.Test/GiraffeDemo.Test.fsproj

rider .\GiraffeDemo.sln
```

#### Run

```
dotnet run --project .\src\GiraffeDemo\GiraffeDemo.fsproj
```

## Functional Refactoring

* Primitive Obsession
* Option
  * Functors
  * Monads

## Resources

- [Matteo Baglini - Functional Refactoring][baglini]


[baglini]: https://www.youtube.com/watch?v=qqS5CKXXvdg
