module OpenAPIParser.Version3.Parser.Parameter

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse Parameter from mapping node
let rec parse findByRef (node:YamlMappingNode) = 
    
    let parseDirect node =
        {
            Name = node |> findScalarValue "name"
            In = node |> findScalarValue "in"
            Description = node |> tryFindScalarValue "description"
            Required = node |> tryFindScalarValue "required" |> someBoolOr false
            Deprecated = node |> tryFindScalarValue "deprecated" |> someBoolOr false
            AllowEmptyValue = node |> tryFindScalarValue "allowEmptyValue" |> someBoolOr false
            Schema = node |> findSchema (toMappingNode >> Schema.parse findByRef)
            Content = 
                node |> tryFindByName "content" 
                |> Option.map (toMappingNode >> toNamedMapM (MediaType.parse findByRef))
                |> someOrEmptyMap
        } : Parameter
    
    match node with
    | Ref r -> r |> findByRef |> parse findByRef
    | _ -> node |> parseDirect
    