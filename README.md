<img src="https://github.com/Dzoukr/OpenAPITypeProvider/raw/master/logo.jpg" alt="drawing" width="100px"/>

# Open API F# Parser

Simple library for parsing YAML/JSON Open API (previously called as Swagger) specification (version 3.0.1). Originally part of my type provider, but I will probably never finish it, so at least community can use part of it for own projects. :)

## Installation
First install NuGet package

    Install-Package OpenAPIParser

or using [Paket](http://fsprojects.github.io/Paket/getting-started.html)

    nuget OpenAPIParser

## How to use

Typically, you would use some existing specification (see [official examples for version 3](https://github.com/OAI/OpenAPI-Specification/tree/master/examples/v3.0)):

```fsharp
open OpenAPIParser.Version3.Parser
let openAPI = Document.loadFromYamlFile "mySpec.yaml" 
```

Then you have basic F# record filled

```fsharp
let version = openAPI.Info.Version
let title = openAPI.Info.Title
let paths = openAPI.Paths
...
```

Please check [test project](https://github.com/Dzoukr/OpenAPIParser/tree/master/tests/OpenAPIParser.Tests) for more examples.

## Limitations

Not all properties from [3.0.1 specification](https://github.com/OAI/OpenAPI-Specification/blob/master/versions/3.0.1.md) are implemented. Especially `anyOf` and `oneOf` Schema objects, which will be probably implemented in future versions. Please check [Specification record definition](https://github.com/Dzoukr/OpenAPIParser/blob/master/src/OpenAPIParser/Version3/Specification.fs) for already implemented properties.

## Contribution

You know the drill. Code + Tests = Good PR. Any contribution more than welcome!