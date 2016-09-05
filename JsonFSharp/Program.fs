open FSharp.Data
open System
open canopy
open runner
open jsonSchema
open System.Collections.Generic

let data = System.IO.File.ReadAllText("data.json")

let proximusTests = Simple.Parse(data)

start chrome

"test 1" &&&& fun _->
    let baseUrl = proximusTests.Projects.[0].Environment.BaseUrl
    let runTests (tests:JsonProvider<"myJson.json">.Test[]) (parameters:Dictionary<string,string>) = 
        let rec runTest (testId:string) (tests:JsonProvider<"myJson.json">.Test[]) (parameters:Dictionary<string,string>) = 
            let test = tests |> Seq.filter (fun(t) -> t.TestId = testId) |> Seq.head
            let n = test.TestId
            if test.Parameters.Length > 0 && parameters.Count = 0 && not (test.DependsOn =  [||]) then
                parameters.Add(test.Parameters.[0],"")
                runTest test.DependsOn.[0] tests parameters
            test.Steps |> Seq.iter (fun(s) -> 
                match s.Action with
                |"url" -> url (baseUrl + s.Target)
                |"click" -> click s.Target
                |"write" -> s.Target << s.Value
                | _ -> ()
                )
        tests |> Seq.iter (fun(t) -> runTest t.TestId tests parameters) 
    
    let parameters = new Dictionary<string,string>()
    let tcs = proximusTests.Projects.[0].Tests

    runTests tcs parameters

run()
    
printfn "press [enter] to exit"
Console.Read() |> ignore

quit()



