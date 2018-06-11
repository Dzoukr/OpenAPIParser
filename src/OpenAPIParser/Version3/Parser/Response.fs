module OpenAPIParser.Version3.Parser.Response

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse Response from mapping node
let rec parse (rootNode:YamlMappingNode) (node:YamlMappingNode) = 
    
    let parseDirect node =
        {
            Description = node |> findScalarValue "description"
            Headers = 
                node |> tryFindByName "headers" 
                |> Option.map (toMappingNode >> toNamedMapM (Header.parse rootNode))
                |> someOrEmptyMap
            Content = 
                node |> tryFindByName "content"
                |> Option.map (toMappingNode >> toNamedMapM (MediaType.parse rootNode))
                |> someOrEmptyMap
        } : Response

    let parseRef refString =
        refString 
        |> findByRef rootNode
        |> parse rootNode
    
    match node with
    | Ref r -> r |> parseRef
    | _ -> node |> parseDirect