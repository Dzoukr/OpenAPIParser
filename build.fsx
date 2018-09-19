#r "paket: groupref Build //"

#load ".fake/build.fsx/intellisense.fsx"
open System.IO
open Fake
open Fake.Core
open Fake.IO
open Fake.IO.Globbing.Operators
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.DotNet.NuGet

let description = "Simple F# parser for Open API specification"
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

Target.create "CleanBinObj" (fun _ -> 
    !! "src/*/bin"
    ++ "src/*/obj"
    ++ "tests/*/bin"
    ++ "tests/*/obj"
    |> Shell.deleteDirs
)

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
            "PackageIconUrl=\"https://raw.githubusercontent.com/Dzoukr/OpenAPITypeProvider/master/logo.jpg\""
            "PackageTags=\"FSharp OpenAPI Swagger Parser\""
            "Copyright=\"Roman Provazník - 2018\""
            "Authors=\"Roman Provazník\""
        ] 
        |> List.map (fun x -> "/p:" + x)
        |> String.concat " "

    
    projectSrc |> DotNet.pack (fun p -> { p with Configuration = DotNet.Custom "Release"; OutputPath = Some "../../nuget"; Common = { p.Common with CustomParams = Some args } })
)

open Fake.Core.TargetOperators
"RunTests" ==> "CleanBinObj" ==> "Nuget"

// start build
Target.runOrDefault "Build"