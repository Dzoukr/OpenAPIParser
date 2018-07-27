module OpenAPIParser.Tests.Parser.Document

open NUnit.Framework
open OpenAPIParser.Version3.Parser
open OpenAPIParser.Tests
open OpenAPIParser.Version3.Specification

[<Test>]
let ``Parses document (Petstore)``() = 
    "Document-Petstore.yaml" |> SampleLoader.getSamplePath |> Document.loadFromYamlFile |> ignore
    Assert.Pass()

[<Test>]
let ``Parses document with remote refs (Petstore External)``() = 
    let doc = "Document-Petstore-External.yaml" |> SampleLoader.getSamplePath |> Document.loadFromYamlFile
    let found = doc.Components.Value.Schemas |> Map.find "Error"
    let props =
        [
            "code", Schema.Integer (IntFormat.Default)
            "message", Schema.String (StringFormat.Default)
        ] |> Map
    let expected = Schema.Object(props, ["code"; "message"])
    let path = doc.Paths.["/pets"]
    Assert.AreEqual("Blah", path.Get.Value.Description.Value)
    Assert.AreEqual(expected, found)