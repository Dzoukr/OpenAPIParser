module OpenAPIParser.Tests.Parser.Response

open NUnit.Framework
open OpenAPIParser.Version3.Parser
open OpenAPIParser.Version3.Specification
open OpenAPIParser.Tests


let sample = {
    Description = "Hello"
    Headers = ["myHeader", Header.sample ] |> Map
    Content = 
        [
            "application/json", { Schema = SchemaDefinition.Integer(IntFormat.Default) |> Schema.Inline }
            "application/xml", { Schema = Schema.Empty }
        ] |> Map
}

[<Test>]
let ``Parses response (direct)``() = 
    let responses = "Response.yaml" |> SampleLoader.parseMapWithRoot Response.parse
    Assert.AreEqual(sample, responses.["direct"])

[<Test>]
let ``Parses response (ref)``() = 
    let responses = "Response.yaml" |> SampleLoader.parseMapWithRoot Response.parse
    Assert.AreEqual(sample, responses.["referenced"])