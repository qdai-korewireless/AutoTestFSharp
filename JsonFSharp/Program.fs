open FSharp.Data
open System
open canopy
open runner
open jsonSchema


start chrome


let simple = Simple.Parse(""" {
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
                        "action": "click",
                        "target":"#button1"
                    },
                    {
                        "action": "findByTitle",
                        "target": "Home page title"
                    }
                ]
            }
        ]
    }
]
} """)

let myTest1 = "SID Activation"
let url1 = "http://172.30.100.33:20000/Secure/Company/Home/CompanyHome.aspx?companyID=9"
let newReqSelector = "#ctl00_LeftNavTree > ul > li:nth-child(5) a:first"
let activationSelector = "#ctl00_LeftNavTree > ul > li:nth-child(5) ul li:nth-child(2) a:first"
let activateToDDL = "#ctl00_RightPlaceHolder_ddlActivateTo"
let searchBtn = "#ctl00_RightPlaceHolder_btnSearch"
let firstSIM = "#ctl00_RightPlaceHolder_grdSIMList_ctl00__0 input[type='checkbox']"
let proceedToFeatureBtn = "#ctl00_RightPlaceHolder_btnSubmit"
let dataService = "#ctl00_RightPlaceHolder_featureSelectionPanel_dataServiceCheckbox"
let fourGFeature = "#ctl00_RightPlaceHolder_featureSelectionPanel_dataSelector_featureChecklist_7"
let customApnDD = "#ctl00_RightPlaceHolder_featureSelectionPanel_customApnSelector_featureSelection"
let dataPlanDD = "#ctl00_RightPlaceHolder_planSelector_dataPlanSelection"
let smsPlanDD = "#ctl00_RightPlaceHolder_planSelector_smsPlanSelection"
let proceedToConfirmBtn = "#ctl00_RightPlaceHolder_btnSave"
let confirmEmailDDL = "#ctl00_RightPlaceHolder_uxConfirmationMessages_RadToEmail_Input"
let confirmEmail = "a@a.com"
let dataPlan = "M2M Data Plan - National Start 1MB"
let smsPlan = "M2M SMS pay per use"
let customApn = "140000"
let activateTo = "Active"
let selectRadDD() =
     click "#ctl00_RightPlaceHolder_uxConfirmationMessages_RadToEmail_Arrow"
     hover "#ctl00_RightPlaceHolder_uxConfirmationMessages_RadToEmail_DropDown"
     hover "#ctl00_RightPlaceHolder_uxConfirmationMessages_RadToEmail_DropDown li:nth-child(2)"
     click "#ctl00_RightPlaceHolder_uxConfirmationMessages_RadToEmail_DropDown li:nth-child(2)"
let confirmBtn = "#ctl00_RightPlaceHolder_btnSubmit"
let dataServicePanelEnabled = "#ctl00_RightPlaceHolder_featureSelectionPanel_customApnSelector_featureSelection:enabled"

let waitForDataServiceEnabled () = 
    not ((element dataServicePanelEnabled) = null)

myTest1 &&&& fun _->
    url url1
    click newReqSelector
    click activationSelector
    activateToDDL << activateTo
    click searchBtn
    click firstSIM
    click proceedToFeatureBtn
    click dataService
    waitFor2 "waiting on enabling data service" waitForDataServiceEnabled 
    click fourGFeature
    customApnDD << customApn
    dataPlanDD << dataPlan
    smsPlanDD << smsPlan
    click proceedToConfirmBtn
    selectRadDD()
    //click confirmBtn
run()
    
printfn "press [enter] to exit"
Console.Read() |> ignore

quit()



