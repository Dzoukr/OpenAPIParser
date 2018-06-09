module OpenAPIParser.Tests.Parser.Parameter

open NUnit.Framework
open OpenAPIParser.Version3.Parser
open OpenAPIParser.Version3.Specification
open OpenAPIParser.Tests

let sample = {
    Name = "Hello"
    In = "In value"
    Description = Some "Some desc"
    Required = true
    Deprecated = true
    AllowEmptyValue = true
    Schema = Schema.String(StringFormat.Default)
    Content =
        ["application/json", { Schema = Schema.Integer(IntFormat.Default) }] |> Map
}

[<Test>]
let ``Parses parameter (direct)``() = 
    let actual = "Parameter.yaml" |> SampleLoader.parseMapWithRoot Parameter.parse
    Assert.AreEqual(sample, actual.["direct"])

[<Test>]
let ``Parses parameter (ref)``() = 
    let actual = "Parameter.yaml" |> SampleLoader.parseMapWithRoot Parameter.parse
    Assert.AreEqual(sample, actual.["referenced"])