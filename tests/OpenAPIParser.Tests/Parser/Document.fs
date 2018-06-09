module OpenAPIParser.Tests.Parser.Document

open NUnit.Framework
open OpenAPIParser.Version3.Parser
open OpenAPIParser.Tests

[<Test>]
let ``Parses document (Petstore)``() = 
    let x = "Document-Petstore.yaml" |> SampleLoader.parse Document.parse |> ignore
    let y = x
    Assert.Pass()