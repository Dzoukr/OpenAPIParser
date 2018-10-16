module OpenAPIParser.Tests.Parser.Schema

open NUnit.Framework
open OpenAPIParser.Version3.Parser
open OpenAPIParser.Version3.Specification
open OpenAPIParser.Tests


[<Test>]
let ``Parses array schema``() = 
    let schemas = "Schema-Array.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = IntFormat.Int32 |> SchemaDefinition.Integer |> Schema.Inline |> SchemaDefinition.Array |> Schema.Inline
    let actual = schemas.["ArrayInt"]
    Assert.AreEqual(expected, actual)


[<Test>]
let ``Parses int schema``() = 
    let schemas = "Schema-Int.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = IntFormat.Int32 |> SchemaDefinition.Integer |> Schema.Inline
    let actual = schemas.["Int"]
    Assert.AreEqual(expected, actual)

let ``Parses int schema (Int64)``() = 
    let schemas = "Schema-Int.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = IntFormat.Int64 |> SchemaDefinition.Integer |> Schema.Inline
    let actual = schemas.["Int64"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses string schema``() = 
    let schemas = "Schema-String.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = StringFormat.String |> SchemaDefinition.String |> Schema.Inline
    let actual = schemas.["String"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses string schema (Binary)``() = 
    let schemas = "Schema-String.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = StringFormat.Binary |> SchemaDefinition.String |> Schema.Inline
    let actual = schemas.["Binary"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses string schema (Date)``() = 
    let schemas = "Schema-String.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = StringFormat.Date |> SchemaDefinition.String |> Schema.Inline
    let actual = schemas.["Date"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses string schema (DateTime)``() = 
    let schemas = "Schema-String.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = StringFormat.DateTime |> SchemaDefinition.String |> Schema.Inline
    let actual = schemas.["DateTime"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses string schema (Password)``() = 
    let schemas = "Schema-String.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = StringFormat.Password |> SchemaDefinition.String |> Schema.Inline
    let actual = schemas.["Password"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses string schema (UUID)``() = 
    let schemas = "Schema-String.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = StringFormat.UUID |> SchemaDefinition.String |> Schema.Inline
    let actual = schemas.["UUID"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses string schema (Enum)``() = 
    let schemas = "Schema-String.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = StringFormat.Enum ["a";"b"] |> SchemaDefinition.String |> Schema.Inline
    let actual = schemas.["Enum"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses boolean schema``() = 
    let schemas = "Schema-Boolean.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = SchemaDefinition.Boolean |> Schema.Inline
    let actual = schemas.["Bool"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses number schema``() = 
    let schemas = "Schema-Number.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = NumberFormat.Default |> SchemaDefinition.Number |> Schema.Inline
    let actual = schemas.["Num"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses number schema (Float)``() = 
    let schemas = "Schema-Number.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = NumberFormat.Float |> SchemaDefinition.Number |> Schema.Inline
    let actual = schemas.["NumFloat"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses number schema (Double)``() = 
    let schemas = "Schema-Number.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let expected = NumberFormat.Double |> SchemaDefinition.Number |> Schema.Inline
    let actual = schemas.["NumDouble"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses object schema``() = 
    let schemas = "Schema-Object.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let props =
        [
            "name", SchemaDefinition.String (StringFormat.Default) |> Schema.Inline
            "age", SchemaDefinition.Integer (IntFormat.Default) |> Schema.Inline
        ] |> Map
    let expected = SchemaDefinition.Object(props, ["name"; "age"]) |> Schema.Inline
    let actual = schemas.["Basic"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses object schema (Nested object)``() = 
    let schemas = "Schema-Object.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let subObj = 
        SchemaDefinition.Object 
            (
                (["age", SchemaDefinition.Integer (IntFormat.Default) |> Schema.Inline] |> Map),
                []) |> Schema.Inline
    let props =
        [
            "name", SchemaDefinition.String (StringFormat.Default) |> Schema.Inline
            "subObj", subObj
        ] |> Map
    let expected = SchemaDefinition.Object(props, ["subObj"]) |> Schema.Inline
    let actual = schemas.["ObjectInObject"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses allOf schema (Mixed)``() = 
    let schemas = "Schema-AllOf.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let props =
        [
            "name", SchemaDefinition.String (StringFormat.Default) |> Schema.Inline
            "age", SchemaDefinition.Integer (IntFormat.Default) |> Schema.Inline
        ] |> Map
    let expected = SchemaDefinition.Object(props, []) |> Schema.Inline
    let actual = schemas.["Mixed"]
    Assert.AreEqual(expected, actual)

[<Test>]
let ``Parses allOf schema (With ref)``() = 
    let schemas = "Schema-AllOf.yaml" |> SampleLoader.parseMapWithRoot Schema.parse
    let props =
        [
            "name", SchemaDefinition.String (StringFormat.Default) |> Schema.Inline
            "age", SchemaDefinition.Integer (IntFormat.Default) |> Schema.Inline
        ] |> Map
    let expected = SchemaDefinition.Object(props, []) |> Schema.Inline
    let actual = schemas.["Extended"]
    Assert.AreEqual(expected, actual)