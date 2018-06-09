module OpenAPIParser.Version3.Parser.Info

open System
open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

let parse (node:YamlMappingNode) = 
    {
        Title = node |> findScalarValue "title"
        Description = node |> tryFindScalarValue "description"
        TermsOfService = node |> tryFindScalarValueM "termsOfService" Uri
        Contact = node |> tryFindByNameM "contact" (toMappingNode >> Contact.parse)
        License = node |> tryFindByNameM "license" (toMappingNode >> License.parse)
        Version = node |> findScalarValue "version"
    } : Info