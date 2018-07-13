module OpenAPIParser.Version3.Parser.MediaType

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse MediaType from mapping node
let parse findByRef (node:YamlMappingNode) = 
    {
        Schema = node |> findSchema (toMappingNode >> Schema.parse findByRef)
    } : MediaType