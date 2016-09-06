module jsonSchema

open FSharp.Data

type AutoTestSchema = JsonProvider<"myJson.json">