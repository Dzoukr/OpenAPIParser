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
            "code", Schema.Inline <| SchemaDefinition.Integer (IntFormat.Default)
            "message", Schema.Inline <| SchemaDefinition.String (StringFormat.Default)
        ] |> Map
    let expected = SchemaDefinition.Object(props, ["code"; "message"]) |> (fun x -> Schema.Reference("Document-Petstore.yaml#/components/schemas/Error", x))
    let path = doc.Paths.["/pets"]
    Assert.AreEqual("Blah", path.Get.Value.Description.Value)
    Assert.AreEqual(expected, found)