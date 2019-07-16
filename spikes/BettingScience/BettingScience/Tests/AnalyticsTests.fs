module AnalyticsTests
open NUnit.Framework
open Analytics
open Domain

[<TestFixture>]
type AccuracyTests() =
    [<Test>]
    member this.ComputeAccuracyDifferentVariance() =
        let state = {
            BookID = "";
            Opening = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
            Closing = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
        }
        let expected = {
            BookID = "";
            Opening = AX2 {O1 = {Expected = 0.0f; Variance = 1.07999992f }; O2 = {Expected = 0.0f; Variance = 1.07999992f } };
            Closing = AX2 {O1 = {Expected = 0.0f; Variance = 2.0f }; O2 = {Expected = 0.0f; Variance = 2.0f } }
        }
        let actual = 
            [(0.2f, 0.5f, 0); (0.3f, 0.5f, 0); (0.4f, 0.5f, 0); (0.5f, 0.5f, 0); (0.5f, 0.5f, 1); (0.6f, 0.5f, 1); (0.7f, 0.5f, 1); (0.8f, 0.5f, 1)]
            |> List.fold (fun state (op, cl, h) ->
                let score = { Home = h; Away = 1 - h }
                let opening = { O1 = (1.f / op, 0); O2 = (1.f / (1.f - op), 0) }
                let closing = { O1 = (1.f / cl, 0); O2 = (1.f / (1.f - cl), 0) }
                let odds = { OutcomeID = ""; Values = [|{ Value = None; BookOdds = [| { BookID = ""; Odds = { Opening = X2 opening; Closing = X2 closing } } |] }|] }
                let newState = Analytics.compute state (score, None) odds
                newState
            ) state
        Assert.That(expected, Is.EqualTo(actual))
    [<Test>]
    member this.ComputeAccuracyDifferentExpected() =
        let state = {
            BookID = "";
            Opening = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
            Closing = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
        }
        let expected = {
            BookID = "";
            Opening = AX2 {O1 = {Expected = -1.1920929e-07f; Variance = 3.48000026f }; O2 = {Expected = 1.1920929e-07f; Variance = 3.48000026f } };
            Closing = AX2 {O1 = {Expected = 0.800000072f; Variance = 2.07999992f }; O2 = {Expected = -0.800000072f; Variance = 2.07999992f } }
        }
        let actual = 
            [(0.8f, 0.6f, 0); (0.7f, 0.6f, 0); (0.6f, 0.6f, 0); (0.5f, 0.6f, 0); (0.5f, 0.6f, 1); (0.4f, 0.6f, 1); (0.3f, 0.6f, 1); (0.2f, 0.6f, 1)]
            |> List.fold (fun state (op, cl, h) ->
                let score = { Home = h; Away = 1 - h }
                let opening = { O1 = (1.f / op, 0); O2 = (1.f / (1.f - op), 0) }
                let closing = { O1 = (1.f / cl, 0); O2 = (1.f / (1.f - cl), 0) }
                let odds = { OutcomeID = ""; Values = [|{ Value = None; BookOdds = [| { BookID = ""; Odds = { Opening = X2 opening; Closing = X2 closing } } |] }|] }
                let newState = Analytics.compute state (score, None) odds
                newState
            ) state
        Assert.That(expected, Is.EqualTo(actual))
    
    [<TestCase(1.5f, -4.f, 2.27999997f, -3.19999981f, 1.27999985f)>]
    [<TestCase(2.5f, -1.1920929e-07f, 3.48000026f, 0.800000072f, 2.07999992f)>]
    [<TestCase(3.5f, 4.f, 2.27999997f, 4.79999971f, 2.88000035f)>]
    member this.ComputeAccuracyOverUnder(handicap, openingExp, openingVar, closingExp, closingVar) =
        let state = {
            BookID = "";
            Opening = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
            Closing = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
        }
        let expected = {
            BookID = "";
            Opening = AX2 {O1 = {Expected = openingExp; Variance = openingVar }; O2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = AX2 {O1 = {Expected = closingExp; Variance = closingVar }; O2 = {Expected = -closingExp; Variance = closingVar } }
        }
        let actual = 
            [(0.8f, 0.6f, 2); (0.7f, 0.6f, 2); (0.6f, 0.6f, 2); (0.5f, 0.6f, 2); (0.5f, 0.6f, 3); (0.4f, 0.6f, 3); (0.3f, 0.6f, 3); (0.2f, 0.6f, 3)]
            |> List.fold (fun state (op, cl, h) ->
                let score = { Home = h; Away = 0 }
                let opening = { O1 = (1.f / op, 0); O2 = (1.f / (1.f - op), 0) }
                let closing = { O1 = (1.f / cl, 0); O2 = (1.f / (1.f - cl), 0) }
                let odds = { OutcomeID = ""; Values = [|{ Value = Some handicap; BookOdds = [| { BookID = ""; Odds = { Opening = X2 opening; Closing = X2 closing } } |] }|] }
                let newState = Analytics.compute state (score, Some handicap) odds
                newState
            ) state
        Assert.That(expected, Is.EqualTo(actual))