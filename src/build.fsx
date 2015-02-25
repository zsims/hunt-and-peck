#r "Tools/FAKE/tools/FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile

let version = getBuildParamOrDefault "version" "1.0.0.0"

Target "BuildApp" (fun _ ->
    CreateCSharpAssemblyInfo "./SolutionInfo.cs"
            [Attribute.Version version
             Attribute.FileVersion version
             Attribute.Product "HuntAndPeck"
             Attribute.Copyright "Copyright Zachary Sims"]
    
    MSBuildRelease "./Build" "Build" ["./HuntAndPeck.sln"]
        |> Log "AppBuild-Output: "
)

Target "Package" (fun _ ->
    let path = sprintf "./Release/HuntAndPeck-%s.zip" version
    ZipHelper.CreateZip "Build" path "" ZipHelper.DefaultZipLevel false !!("./Build/**")
)

Target "Clean" (fun _ -> 
    CleanDirs ["./Release/"; "./Build/"]
)

Target "Help" <| fun () ->
    printfn ""
    printfn "  Please specify the target by calling 'build <Target>'"
    printfn ""
    printfn "  * BuildApp   - Build everything"
    printfn "  * Package    - Build and package a new release"

Target "Root" DoNothing

"BuildApp"
    ==> "Package"
    ==> "Package"

RunTargetOrDefault "Help"