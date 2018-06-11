module OpenAPIParser.Version3.Parser.Document

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel
open System.IO
open Newtonsoft.Json
open Newtonsoft.Json.Converters
open System.Dynamic

/// Parse specification from mapping node
let parse (rootNode:YamlMappingNode) = {
    SpecificationVersion = rootNode |> findScalarValue "openapi"
    Info = rootNode |> findByNameM "info" (toMappingNode >> Info.parse)
    Paths = rootNode |> findByNameM "paths" (toMappingNode >> toNamedMapM (Path.parse rootNode))
    Components = rootNode |> tryFindByName "components" |> Option.map (toMappingNode >> Components.parse rootNode)
}

/// Parse specification from YAML string
let parseFromYaml content =
    let reader = new StringReader(content)
    let yaml = YamlStream()
    yaml.Load(reader)
    yaml.Documents.[0].RootNode |> toMappingNode |> parse

let private readFile p = p |> File.ReadAllText

/// Load specification from YAML file
let loadFromYamlFile file = file |> readFile |> parseFromYaml

/// Convert JSON string to YAML
let convertJsonToYaml json =
    let expConverter = new ExpandoObjectConverter();
    let deserializedObject = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);
    let serializer = new YamlDotNet.Serialization.Serializer();
    serializer.Serialize(deserializedObject)

/// Parse specification from JSON string
let parseFromJson content = content |> convertJsonToYaml |> parseFromYaml

/// Load specification from JSON file
let loadFromJsonFile file = file |> readFile |> parseFromJson