open FSharp.Data
open System
open canopy
open runner
open jsonSchema


let proximusTests = Simple.Parse(""" {
"projects" : [
    {
        "name": "Proximus",
        "environment" : {
            "name": "QA"
        },
        "testCases" : [
            {
                "name": "SID Activation",
                "steps": [
                    {
                        "action": "url",
                        "target":"http://172.30.100.33:20000/Secure/Company/Home/CompanyHome.aspx?companyID=9"
                    },
                    {
                        "action": "click",
                        "target": "#ctl00_LeftNavTree > ul > li:nth-child(5) a:first"
                    },
                    {
                        "action": "click",
                        "target": "#ctl00_LeftNavTree > ul > li:nth-child(5) ul li:nth-child(2) a:first"
                    },
                    {
                        "action": "write",
                        "target": "#ctl00_RightPlaceHolder_ddlActivateTo",
                        "value": "Active"
                    }
                ]
            }
        ]
    }
]
} """)


start chrome

"test 1" &&&& fun _->
    let testSteps = proximusTests.Projects.[0].TestCases.[0].Steps
    testSteps |> Seq.iter (fun(s) -> 
                            match s.Action with
                            |"url" -> url s.Target
                            |"click" -> click s.Target
                            |"write" -> s.Target << s.Value
                            | _ -> ()
                            )
run()
    
printfn "press [enter] to exit"
Console.Read() |> ignore

quit()



