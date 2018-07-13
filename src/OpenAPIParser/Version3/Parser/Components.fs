module OpenAPIParser.Version3.Parser.Components

open System
open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse Components from mapping node
let parse findByRef (node:YamlMappingNode) = 
    {
        Schemas = 
            node 
            |> tryFindByName "schemas"
            |> Option.map (toMappingNode >> toNamedMapM (Schema.parse findByRef))
            |> someOrEmptyMap
        Responses = 
            node 
            |> tryFindByName "responses"
            |> Option.map (toMappingNode >> toNamedMapM (Response.parse findByRef))
            |> someOrEmptyMap
        Parameters =
            node 
            |> tryFindByName "parameters"
            |> Option.map (toMappingNode >> toNamedMapM (Parameter.parse findByRef))
            |> someOrEmptyMap
        RequestBodies = 
            node 
            |> tryFindByName "requestBodies"
            |> Option.map (toMappingNode >> toNamedMapM (RequestBody.parse findByRef))
            |> someOrEmptyMap
        Headers = 
            node 
            |> tryFindByName "headers"
            |> Option.map (toMappingNode >> toNamedMapM (Header.parse findByRef))
            |> someOrEmptyMap
    } : Components