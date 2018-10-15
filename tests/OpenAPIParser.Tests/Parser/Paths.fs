module OpenAPIParser.Tests.Parser.Paths

open NUnit.Framework
open OpenAPIParser.Version3.Parser
open OpenAPIParser.Version3.Specification
open OpenAPIParser.Tests

let sample = { 
    Post = Some {
        Description = None
        Tags = ["pet"]
        Summary = Some "Updates a pet in the store with form data"
        OperationId = Some "updatePetWithForm"
        Parameters = 
            [
                {
                    Name = "petId"
                    In = "path"
                    Description = Some "ID of pet that needs to be updated"
                    Required = true
                    Deprecated = false
                    AllowEmptyValue = false
                    Schema = SchemaDefinition.String(StringFormat.Default) |> Schema.Inline
                    Content = Map.empty
                }
            ]
        RequestBody = 
            Some ({
                    Description = None
                    Content = 
                        [
                            "application/x-www-form-urlencoded",
                            { 
                                Schema = 
                                    SchemaDefinition.Object (
                                        ["name", SchemaDefinition.String (StringFormat.Default) |> Schema.Inline] |> Map
                                        ,["name"]
                                    ) |> Schema.Inline 
                            }
                        ] |> Map
                    Required = false
        })
        Responses = 
            [
                "200", { 
                    Description = "Pet updated"
                    Headers = Map.empty
                    Content = 
                        [
                            "application/json", { Schema = Schema.Empty }
                            "application/xml", { Schema = Schema.Empty }
                        ] |> Map}
            ] |> Map
        Deprecated = false
    }
    Get = None
    Put = None
    Delete = None
    Options = None
    Head = None
    Patch = None
    Trace = None
    Summary = None
    Description = None
    Parameters = []
}

[<Test>]
let ``Parses path``() = 
    let actual = "Paths.yaml" |> SampleLoader.parseWithRoot Path.parse
    Assert.AreEqual(sample, actual)