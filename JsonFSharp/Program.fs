open FSharp.Data
open System
open canopy
open runner
open jsonSchema
open System.Collections.Generic

let data = System.IO.File.ReadAllText("data.json")

let proximusTests = AutoTestSchema.Parse(data)

start chrome

let assignParam text parameters= 
    "Secure/Company/Home/CompanyHome.aspx?companyID=137883"

let validateParameters testParams (parameters:Dictionary<string,string>) = 
    let results = [
        for p in testParams do
            if parameters.[p] = null then
                parameters.Add(p,"")
                yield false
            else
                if not (parameters.[p] = "") then
                    yield true
                else
                    yield false
        ]

    results |> Seq.forall (fun(r) -> r)

"test 1" &&&& fun _->
    let baseUrl = proximusTests.Projects.[0].Environment.BaseUrl
    let runTests (tests:JsonProvider<"myJson.json">.Test[]) (parameters:Dictionary<string,string>) = 

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
                    if not (parameters.ContainsKey(s.Target)) then
                        parameters.Add(s.Target, (read s.Value))
                    else
                        parameters.[s.Target] <- s.Value
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



