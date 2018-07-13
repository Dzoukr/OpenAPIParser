module OpenAPIParser.Version3.Parser.Header

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse Header from mapping node
let rec parse findByRef (node:YamlMappingNode) = 
    
    let parseDirect node = 
        {
            Description = node |> tryFindScalarValue "description"
            Required = node |> tryFindScalarValue "required" |> someBoolOr false
            Deprecated = node |> tryFindScalarValue "deprecated" |> someBoolOr false
            AllowEmptyValue = node |> tryFindScalarValue "allowEmptyValue" |> someBoolOr false
            Schema = node |> findSchema (toMappingNode >> Schema.parse findByRef)
        } : Header
    
    match node with
    | Ref r -> r |> findByRef |> parse findByRef
    | _ -> node |> parseDirect