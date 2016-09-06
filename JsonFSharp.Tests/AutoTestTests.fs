module AutoTestTests
open FsUnit
open NUnit.Framework

[<Test>]
let ``should work``() = 
    true |> should equal true