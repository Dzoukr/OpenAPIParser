module OpenAPIParser.Version3.Parser.MediaType

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

let parse (rootNode:YamlMappingNode) (node:YamlMappingNode) = 
    {
        Schema = node |> findSchema (toMappingNode >> Schema.parse rootNode)
    } : MediaType