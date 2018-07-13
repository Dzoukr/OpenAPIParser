module OpenAPIParser.Version3.Parser.Response

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse Response from mapping node
let rec parse findByRef (node:YamlMappingNode) = 
    
    let parseDirect node =
        {
            Description = node |> findScalarValue "description"
            Headers = 
                node |> tryFindByName "headers" 
                |> Option.map (toMappingNode >> toNamedMapM (Header.parse findByRef))
                |> someOrEmptyMap
            Content = 
                node |> tryFindByName "content"
                |> Option.map (toMappingNode >> toNamedMapM (MediaType.parse findByRef))
                |> someOrEmptyMap
        } : Response
    
    match node with
    | Ref r -> r |> findByRef |> parse findByRef
    | _ -> node |> parseDirect