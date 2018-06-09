#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.Dotnet.Cli
nuget Fake.DotNet.Testing.NUnit
nuget Fake.Core.ReleaseNotes
nuget Fake.Core.Target //"

#load "./.fake/build.fsx/intellisense.fsx"

open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.Core

let description = "Open API Parser"
let projectSrc = "src/OpenAPIParser"
let testsSrc = "tests/OpenAPIParser.Tests"

Target.create "Build" (fun _ ->
    projectSrc |> DotNet.build id
)

Target.create "RunTests" (fun _ ->
    testsSrc |> DotNet.test id
)

// Read release notes & version info from RELEASE_NOTES.md
let release = ReleaseNotes.load "RELEASE_NOTES.md"

Target.create "Nuget" (fun _ ->
    let toNotes = List.map (fun x -> x + System.Environment.NewLine) >> List.fold (+) ""
    let args = 
        [
            "PackageId=\"OpenAPIParser\""
            "Title=\"OpenAPIParser\""
            sprintf "Description=\"%s\"" description
            sprintf "Summary=\"%s\"" description
            sprintf "PackageVersion=\"%s\"" release.NugetVersion
            sprintf "PackageReleaseNotes=\"%s\"" (release.Notes |> toNotes)
            "PackageLicenseUrl=\"http://github.com/dzoukr/OpenAPIParser/blob/master/LICENSE.md\""
            "PackageProjectUrl=\"http://github.com/dzoukr/OpenAPIParser\""
            "PackageIconUrl=\"https://avatars2.githubusercontent.com/u/851307?v=3&amp;s=64\""
            "PackageTags=\"FSharp OpenAPI Swagger Parser\""
            "Copyright=\"Roman Provazník - 2018\""
            "Authors=\"Roman Provazník\""
        ] 
        |> List.map (fun x -> "/p:" + x)
        |> String.concat " "

    
    projectSrc |> DotNet.pack (fun p -> { p with Configuration = DotNet.Custom "Release"; OutputPath = Some "../../nuget"; Common = { p.Common with CustomParams = Some args } })
)

// start build
Target.runOrDefault "Build"