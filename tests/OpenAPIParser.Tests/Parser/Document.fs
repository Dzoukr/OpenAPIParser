module OpenAPIParser.Tests.Parser.Document

open NUnit.Framework
open OpenAPIParser.Version3.Parser
open OpenAPIParser.Tests

[<Test>]
let ``Parses document (Petstore)``() = 
    "Document-Petstore.yaml" |> SampleLoader.parse Document.parse |> ignore
    Assert.Pass()