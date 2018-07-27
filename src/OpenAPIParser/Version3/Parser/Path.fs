module OpenAPIParser.Version3.Parser.Path

open System
open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse Path from mapping node
let rec parse findByRef (node:YamlMappingNode) = 
    
    let parseDirect node = 
        {
            Summary = node |> tryFindScalarValue "summary"
            Description = node |> tryFindScalarValue "description"
            Get = node |> tryFindByNameM "get" (toMappingNode >> Operation.parse findByRef)
            Put = node |> tryFindByNameM "put" (toMappingNode >> Operation.parse findByRef)
            Post = node |> tryFindByNameM "post" (toMappingNode >> Operation.parse findByRef)
            Delete = node |> tryFindByNameM "delete" (toMappingNode >> Operation.parse findByRef)
            Options = node |> tryFindByNameM "options" (toMappingNode >> Operation.parse findByRef)
            Head = node |> tryFindByNameM "head" (toMappingNode >> Operation.parse findByRef)
            Patch = node |> tryFindByNameM "patch" (toMappingNode >> Operation.parse findByRef)
            Trace = node |> tryFindByNameM "trace" (toMappingNode >> Operation.parse findByRef)
            Parameters =
                    node 
                    |> tryFindByName "parameters"
                    |> Option.map (seqValue)
                    |> Option.map (List.map (toMappingNode >> Parameter.parse findByRef))
                    |> someOrEmptyList
        }

    match node with
    | Ref r -> r |> findByRef |> parse findByRef
    | _ -> node |> parseDirect