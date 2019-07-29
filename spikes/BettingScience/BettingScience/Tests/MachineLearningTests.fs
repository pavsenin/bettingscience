module MachineLearningTests
open NUnit.Framework
open Domain
open Analytics
open System.IO
open Microsoft.FSharpLu.Json
open System
open Utils

[<TestFixture>]
type MachineLearningTests() =
    [<Test>]
    member this.PredictMLB18BasedOnMLB1217() =
        ()