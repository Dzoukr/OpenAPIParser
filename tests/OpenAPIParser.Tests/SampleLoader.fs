module OpenAPIParser.Tests.SampleLoader

open System
open System.IO
open YamlDotNet.RepresentationModel
open OpenAPIParser.Version3.Parser.Core
open OpenAPIParser.Version3.Parser

let samplesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples")

let getSamplePath p = Path.Combine(samplesPath, p)

let loader = Document.refFileLoader samplesPath

let parseWithRoot parseFn name =
    let root = name |> loader
    parseFn (findByRef loader root) root

let parse parseFn name = name |> loader |> parseFn

let parseMapWithRoot parseFn name =
    let root = name |> loader
    root |> toNamedMapM (parseFn (findByRef loader root))