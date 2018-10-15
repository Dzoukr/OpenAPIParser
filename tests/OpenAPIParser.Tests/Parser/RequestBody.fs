module OpenAPIParser.Tests.Parser.RequestBody

open NUnit.Framework
open OpenAPIParser.Version3.Parser
open OpenAPIParser.Version3.Specification
open OpenAPIParser.Tests

let rbSample = {
    Description = Some "Hello"
    Required = false
    Content =
        ["application/json", { Schema = SchemaDefinition.Integer(IntFormat.Default) |> Schema.Inline }] |> Map
}

[<Test>]
let ``Parses request body (direct)``() = 
    let actual = "RequestBody.yaml" |> SampleLoader.parseMapWithRoot RequestBody.parse
    Assert.AreEqual(rbSample, actual.["direct"])

[<Test>]
let ``Parses request body (ref)``() = 
    let actual = "RequestBody.yaml" |> SampleLoader.parseMapWithRoot RequestBody.parse
    Assert.AreEqual(rbSample, actual.["referenced"])