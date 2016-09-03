module jsonSchema

open FSharp.Data

type Simple = JsonProvider<"""{
    "projects" : [
        {
            "name": "Proximus",
            "environment" : {
                "name": "QA"
            },
            "testCases" : [
                {
                    "name": "Test Case Name",
                    "steps": [
                        {
                            "action": "click",
                            "target":"#button1",
                            "value":"text"
                        }
                    ]
                }
            ]
        }
    ]
}""">