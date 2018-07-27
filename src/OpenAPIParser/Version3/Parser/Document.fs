module OpenAPIParser.Version3.Parser.Document

open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel
open System.IO
open Newtonsoft.Json
open Newtonsoft.Json.Converters
open System.Dynamic

let private readFile p = p |> File.ReadAllText

let private toYaml content =
    let reader = new StringReader(content)
    let yaml = YamlStream()
    yaml.Load(reader)
    yaml.Documents.[0].RootNode |> toMappingNode

let private getExtension = Path.GetExtension >> (fun x -> x.ToLower())

/// Convert JSON string to YAML
let convertJsonToYaml json =
    let expConverter = new ExpandoObjectConverter();
    let deserializedObject = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);
    let serializer = new YamlDotNet.Serialization.Serializer();
    serializer.Serialize(deserializedObject)

/// File loader for $ref references
let refFileLoader (rootLocation:string) (file:string) =
    let filePath = Path.Combine(rootLocation,file)
    match getExtension file with
    | ".yaml" -> filePath |> readFile |> toYaml
    | ".json" -> filePath |> readFile |> convertJsonToYaml |> toYaml
    | _ -> failwith "Only .yaml & .json files are supported."


/// Parse specification from mapping node
let parse (rootLocation:string) (rootNode:YamlMappingNode) = 
    let findByRef = Core.findByRef (refFileLoader rootLocation) rootNode
    {
        SpecificationVersion = rootNode |> findScalarValue "openapi"
        Info = rootNode |> findByNameM "info" (toMappingNode >> Info.parse)
        Paths = rootNode |> findByNameM "paths" (toMappingNode >> toNamedMapM (Path.parse findByRef))
        Components = rootNode |> tryFindByName "components" |> Option.map (toMappingNode >> Components.parse findByRef)
    }

/// Parse specification from YAML string
let parseFromYaml rootLocation content = content |> toYaml |> parse rootLocation

/// Load specification from YAML file
let loadFromYamlFile file = 
    let fileLocation = file |> Path.GetDirectoryName
    file |> readFile |> parseFromYaml fileLocation

/// Parse specification from JSON string
let parseFromJson rootLocation content = content |> convertJsonToYaml |> parseFromYaml rootLocation

/// Load specification from JSON file
let loadFromJsonFile file = 
    let fileLocation = file |> Path.GetDirectoryName
    file |> readFile |> parseFromJson fileLocation