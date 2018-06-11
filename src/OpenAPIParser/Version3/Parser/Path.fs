module OpenAPIParser.Version3.Parser.Path

open System
open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse Path from mapping node
let parse (rootNode:YamlMappingNode) (node:YamlMappingNode) = {
    Summary = node |> tryFindScalarValue "summary"
    Description = node |> tryFindScalarValue "description"
    Get = node |> tryFindByNameM "get" (toMappingNode >> Operation.parse rootNode)
    Put = node |> tryFindByNameM "put" (toMappingNode >> Operation.parse rootNode)
    Post = node |> tryFindByNameM "post" (toMappingNode >> Operation.parse rootNode)
    Delete = node |> tryFindByNameM "delete" (toMappingNode >> Operation.parse rootNode)
    Options = node |> tryFindByNameM "options" (toMappingNode >> Operation.parse rootNode)
    Head = node |> tryFindByNameM "head" (toMappingNode >> Operation.parse rootNode)
    Patch = node |> tryFindByNameM "patch" (toMappingNode >> Operation.parse rootNode)
    Trace = node |> tryFindByNameM "trace" (toMappingNode >> Operation.parse rootNode)
    Parameters =
            node 
            |> tryFindByName "parameters"
            |> Option.map (seqValue)
            |> Option.map (List.map (toMappingNode >> Parameter.parse rootNode))
            |> someOrEmptyList
}