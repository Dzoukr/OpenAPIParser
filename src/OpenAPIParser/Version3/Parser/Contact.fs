module OpenAPIParser.Version3.Parser.Contact

open System
open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

let parse (node:YamlMappingNode) = 
    {
        Name = node |> tryFindScalarValue "name"
        Url = node |> tryFindScalarValueM "url" Uri
        Email = node |> tryFindScalarValue "email"
    }