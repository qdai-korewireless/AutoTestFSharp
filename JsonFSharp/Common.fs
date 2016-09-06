module Kore.AutoTest.Common
open FSharp.Data
open System.Collections.Generic
open System.Text.RegularExpressions

let executingDir () = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\drivers"
let getBaseUrl (project:JsonProvider<"myJson.json">.Project) = 
    project.Environment.BaseUrl

let assignParam text (parameters:Dictionary<string,string>)= 
    let pattern = @"\#\{\w+\}"
    let mutable result = text
    let matches = Regex.Matches(text,pattern)
    for m in matches do
        let key = m.Value.Replace("#{","").Replace("}","")
        if parameters.ContainsKey(key) then
            let value = parameters.[key]
            result <- result.Replace("#{"+key+"}",value)

    result

let validateParameters testParams (parameters:Dictionary<string,string>) = 
    let results = [
        for p in testParams do

            if not (parameters.ContainsKey(p)) then
                parameters.Add(p,"")
                yield false
            else
                if not (parameters.[p] = "") then
                    yield true
                else
                    yield false
        ]

    results |> Seq.forall (fun(r) -> r)