module Kore.AutoTest.Common.Tests
open FsUnit
open NUnit.Framework
open System.Collections.Generic
open Kore.AutoTest.Common


[<Test>]
let ``should parameter templates be replaced with parameter list values``() = 
    let expected = "http://172.30.100.30:20000/welcome.aspx?companyID=123456"
    let parameters = new Dictionary<string,string>()
    parameters.Add("CompanyID","123456")

    let actual = assignParam "http://172.30.100.30:20000/welcome.aspx?companyID=#{CompanyID}" parameters

    actual |> should equal expected

[<Test>]
let ``should text without parameter template be not changed``() = 
    let expected = "http://172.30.100.30:20000/welcome.aspx?companyID=123456"
    let parameters = new Dictionary<string,string>()
    parameters.Add("CompanyID","123456")

    let actual = assignParam "http://172.30.100.30:20000/welcome.aspx?companyID=123456" parameters

    actual |> should equal expected

[<Test>]
let ``should test case parameters have values in parameter list to pass validation``() = 
    let expected = true
    let testParams = ["CompanyID";"SimNumber"]
    let parameters = new Dictionary<string,string>()
    parameters.Add("CompanyID","123456")
    parameters.Add("SimNumber","8700000000000")

    let actual = validateParameters testParams parameters

    actual |> should equal expected


[<Test>]
let ``should test case parameters have no values in parameter list fail validation``() = 
    let expected = false
    let testParams = ["CompanyID";"SimNumber"]
    let parameters = new Dictionary<string,string>()
    parameters.Add("CompanyID","123456")

    let actual = validateParameters testParams parameters

    actual |> should equal expected