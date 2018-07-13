module OpenAPIParser.Version3.Parser.Schema

open System
open OpenAPIParser.Version3.Specification
open Core
open YamlDotNet.RepresentationModel

let private intFormatFromString = function
    | "int32" -> IntFormat.Int32
    | "int64" -> IntFormat.Int64
    | _ -> IntFormat.Default

let private numberFormatFromString = function
    | "float" -> NumberFormat.Float
    | "double" -> NumberFormat.Double
    | _ -> NumberFormat.Default

let private stringFormatFromString = function
    | "binary" -> StringFormat.Binary
    | "byte" -> StringFormat.Byte
    | "date" -> StringFormat.Date
    | "date-time" -> StringFormat.DateTime
    | "password" -> StringFormat.Password
    | _ -> StringFormat.Default

let private tryParseFormat fn node =
    node 
    |> tryFindScalarValue "format"
    |> (fun x -> defaultArg x String.Empty)
    |> fn

let private optionToList = function
    | Some v -> v |> Seq.toList
    | None -> []

let private isNodeType typ node = 
    node |> tryFindScalarValue "type" 
    |> Option.map (fun x -> x = typ)
    |> Option.bind (fun x -> if x then Some () else None)

let private (|Array|_|) (node:YamlMappingNode) = node |> isNodeType "array"
let private (|Integer|_|) (node:YamlMappingNode) = node |> isNodeType "integer"
let private (|String|_|) (node:YamlMappingNode) = node |> isNodeType "string"
let private (|Boolean|_|) (node:YamlMappingNode) = node |> isNodeType "boolean"
let private (|Number|_|) (node:YamlMappingNode) = node |> isNodeType "number"
let private (|Object|_|) (node:YamlMappingNode) = node |> isNodeType "object"
let private (|AllOf|_|) (node:YamlMappingNode) = node |> tryFindByName "allOf" |> Option.map seqValue

let private mergeSchemaPair (schema1:Schema) (schema2:Schema) = 
    match schema1, schema2 with
    | Schema.Object (p1, r1), Schema.Object (p2, r2) ->
        let required = (r1 @ r2) |> List.distinct
        let props = Map(Seq.concat [ (Map.toSeq p1) ; (Map.toSeq p2) ])
        Schema.Object (props, required)
    | _ -> failwith "Both schemas must be Object type"

let private mergeSchemas (schemas:Schema list) = 
    match schemas with
    | [] -> failwith "Schema list should not be empty"
    | list -> list |> List.reduce mergeSchemaPair

/// Parse Schema from mapping node
let rec parse findByRef (node:YamlMappingNode) =
    
    let parseDirect node =
        match node with
        | AllOf n -> n |> List.map (toMappingNode >> parse findByRef) |> mergeSchemas
        | Array ->
            let items = node |> findByName "items" |> toMappingNode
            items |> parse findByRef |> Schema.Array
        | Integer -> node |> tryParseFormat intFormatFromString |> Schema.Integer
        | String -> node |> tryParseFormat stringFormatFromString |> Schema.String
        | Boolean -> Schema.Boolean
        | Number -> node |> tryParseFormat numberFormatFromString |> Schema.Number
        | _ ->
            match node |> tryFindByNameM "properties" toMappingNode with
            | Some p ->
                let props = p |> toMappingNode |> toNamedMapM (parse findByRef)
                let required = 
                    node |> tryFindByName "required" 
                    |> Option.map seqValue
                    |> Option.map (List.map (fun x -> x.ToString()))
                    |> optionToList
                Schema.Object(props, required)
            | None -> Schema.Empty
    
    match node with
    | Ref r -> r |> findByRef |> parse findByRef
    | _ -> node |> parseDirect