module OpenAPIParser.Version3.Parser.Operation

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse Operation from mapping node
let parse findByRef (node:YamlMappingNode) = 
    {
        Tags =
            node 
            |> tryFindByName "tags"
            |> Option.map (seqValue)
            |> Option.map (List.map value)
            |> someOrEmptyList
        Summary = node |> tryFindScalarValue "summary"
        Description = node |> tryFindScalarValue "description"
        OperationId = node |> tryFindScalarValue "operationId"
        Parameters =
            node 
            |> tryFindByName "parameters"
            |> Option.map (seqValue)
            |> Option.map (List.map (toMappingNode >> Parameter.parse findByRef))
            |> someOrEmptyList
        RequestBody =
            node
            |> tryFindByNameM "requestBody" (toMappingNode >> RequestBody.parse findByRef)
        Responses = 
            node 
            |> tryFindByName "responses"
            |> Option.map (toMappingNode >> toNamedMapM (Response.parse findByRef))
            |> someOrEmptyMap
        Deprecated = node |> tryFindScalarValue "deprecated" |> someBoolOr false
    }