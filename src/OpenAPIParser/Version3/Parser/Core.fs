module OpenAPIParser.Version3.Parser.Core

open System
open YamlDotNet.RepresentationModel

/// Find node by name
let findByName name (node:YamlMappingNode) =
    try
        node.Children 
        |> Seq.find (fun x -> x.Key.ToString() = name) 
        |> (fun x -> x.Value)
    with _ -> failwith <| sprintf "Cannot find '%s' in %A" name node

/// Find node by name with map function
let findByNameM name mapFn = findByName name >> mapFn

/// Try find node by name
let tryFindByName name (node:YamlMappingNode) =
    node.Children 
    |> Seq.tryFind (fun x -> x.Key.ToString() = name)
    |> Option.map (fun x -> x.Value)

/// Try find node by name with map function
let tryFindByNameM name mapFn = tryFindByName name >> Option.map mapFn

/// Get node scalar value
let value (node:YamlNode) = node :?> YamlScalarNode |> (fun x -> x.Value)

/// Get node as list of nodes
let seqValue (node:YamlNode) = node :?> YamlSequenceNode |> (fun x -> x.Children) |> Seq.toList

/// Find scalar value by name
let findScalarValue name = findByName name >> value

/// Find scalar value by name with map function
let findScalarValueM name mapFn = findByName name >> value >> mapFn

/// Try find scalar value by name
let tryFindScalarValue name = tryFindByName name >> Option.map value

/// Try find scalar value by name with map function
let tryFindScalarValueM name mapFn = tryFindScalarValue name >> Option.map mapFn

/// Convert node to mapping node
let toMappingNode (node:YamlNode) = node :?> YamlMappingNode

/// Convert node to map
let toNamedMap (node:YamlMappingNode) = 
    node.Children
    |> Seq.map ((|KeyValue|) >> (fun (k,v) -> k.ToString(), v))
    |> Map.ofSeq

/// Convert node to map with map function
let toNamedMapM mapFn = toNamedMap >> Map.map (fun _ v -> v |> toMappingNode |> mapFn)

/// Some or default (bool) value
let someBoolOr value = function
    | Some v -> v |> System.Boolean.Parse
    | None -> value

/// Some or empty map
let someOrEmptyMap = function
    | Some v -> v
    | None -> Map.empty

/// Some or empty list
let someOrEmptyList = function
    | Some x -> x
    | None -> List.Empty

let (|Ref|_|) (node:YamlMappingNode) = node |> tryFindScalarValue "$ref"

let toFileAndPathRef (refString:string) =
    let parts = refString.Split([|'#'|])
    let emptyToNone value = 
        match String.IsNullOrWhiteSpace value with
        | true -> None
        | false -> Some value
    if refString.Contains("#") then
        (emptyToNone parts.[0]), (emptyToNone parts.[1])
    else (Some refString, None)

/// Find node by ref string
let findByRef (fileLoader:string -> YamlMappingNode) (rootNode:YamlMappingNode) (refString:string) =
    try
        let findPathInNode (path:string) (node:YamlMappingNode)  =
            let parts = path.Split([|'/'|], StringSplitOptions.RemoveEmptyEntries)
            let foldFn (n:YamlMappingNode) (name:string) =
                n.Children 
                |> Seq.filter (fun x -> x.Key.ToString() = name)
                |> Seq.head
                |> (fun x -> x.Value |> toMappingNode)
            parts |> Array.fold foldFn node
        
        match toFileAndPathRef refString with
        | Some file, Some path -> file |> fileLoader |> findPathInNode path
        | None, Some path -> rootNode |> findPathInNode path
        | Some file, None -> file |> fileLoader |> findPathInNode ""
        | None, None -> failwithf "Invalid $ref string '%s'" refString
    with ex -> failwith <| sprintf "Cannot find '%s' in node %A, exception thrown: %A" refString rootNode ex

/// Find schema
let findSchema mapFn node =
    match node |> tryFindByName "schema" with
    | Some x -> x |> mapFn
    | None -> OpenAPIParser.Version3.Specification.SchemaDefinition.Empty |> OpenAPIParser.Version3.Specification.Schema.Inline