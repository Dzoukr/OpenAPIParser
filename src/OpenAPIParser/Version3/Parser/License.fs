module OpenAPIParser.Version3.Parser.License

open System
open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

/// Parse License from mapping node
let parse (node:YamlMappingNode) = {
    Name = node |> findScalarValue "name"
    Url = node |> tryFindScalarValueM "url" Uri
}