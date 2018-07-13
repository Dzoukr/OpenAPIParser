module OpenAPIParser.Version3.Parser.RequestBody

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse RequestBody from mapping node
let rec parse findByRef (node:YamlMappingNode) = 
    
    let parseDirect node = 
        {
            Description = node |> tryFindScalarValue "description"
            Required = node |> tryFindScalarValue "required" |> someBoolOr false
            Content = 
                node |> findByNameM "content" toMappingNode 
                |> toMappingNode |> toNamedMapM (MediaType.parse findByRef)
        } : RequestBody

    match node with
    | Ref r -> r |> findByRef |> parse findByRef
    | _ -> node |> parseDirect