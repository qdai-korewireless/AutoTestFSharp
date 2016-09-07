module Kore.AutoTest.Program
open FSharp.Data
open System
open canopy
open runner
open jsonSchema
open Common
open System.Collections.Generic
open System.Text.RegularExpressions

configuration.chromeDir <- executingDir()
configuration.ieDir <- executingDir()

let raw = System.IO.File.ReadAllText(@"Data\data.json")
let obj = AutoTestSchema.Parse(raw)
let parameters = new Dictionary<string,string>()
let firstProject = obj.Projects.[0]
let tests = firstProject.Tests
let baseUrl = getBaseUrl firstProject

let rec runTest (testId:string) (tests:JsonProvider<"myJson.json">.Test[]) (parameters:Dictionary<string,string>) = 
    let test = tests |> Seq.filter (fun(t) -> t.TestId = testId) |> Seq.head
            
    if not (validateParameters test.Parameters parameters) then
        test.DependsOn |> Seq.iter (fun(d) -> runTest d tests parameters)

    test.Steps |> Seq.iter (fun(s) -> 
        match s.Action with
        |"url" -> url (baseUrl + (assignParam s.Target parameters))
        |"click" -> click s.Target
        |"write" -> s.Target << s.Value
        |"read" -> waitFor (fun _-> (elements s.Target).Length = 1)
        |"record" -> 
            let readValue = read s.Value
            if not (parameters.ContainsKey(s.Target)) then
                parameters.Add(s.Target, readValue)
            else
                parameters.[s.Target] <- readValue
        | _ -> ()
        )

let initTests (tests:JsonProvider<"myJson.json">.Test[]) (parameters:Dictionary<string,string>) = 
    tests |> Seq.iter (fun(t) ->
        t.Name &&&& fun _->
            runTest t.TestId tests parameters) 

initTests tests parameters


start chrome

run()
    
printfn "press [enter] to exit"
Console.Read() |> ignore

quit()



