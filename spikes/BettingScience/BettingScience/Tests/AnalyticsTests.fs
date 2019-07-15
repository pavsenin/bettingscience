module AnalyticsTests
open NUnit.Framework
open Analytics
open Domain
open OddsPortalScraper

[<TestFixture>]
type AccuracyTests() =
    [<Test>]
    member this.ComputeAccuracy_01() =
        let state = {
            BookID = "";
            Opening = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
            Closing = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
        }
        let expected = {
            BookID = "";
            Opening = AX2 { O1 = { Expected = 0.f; Variance = 1.08f }; O2 = { Expected = 0.f; Variance = 1.08f } };
            Closing = AX2 { O1 = { Expected = 0.f; Variance = 2.f }; O2 = { Expected = 0.f; Variance = 2.f } }
        }
        let actual = 
            [(0.2f, 0.5f, 0); (0.3f, 0.5f, 0); (0.4f, 0.5f, 0); (0.5f, 0.5f, 0); (0.5f, 0.5f, 1); (0.6f, 0.5f, 1); (0.7f, 0.5f, 1); (0.8f, 0.5f, 1)]
            |> List.fold (fun state (op, cl, h) ->
                let score = { Home = h; Away = 1 - h }
                let opening = { O1 = (1.f / op, 0); O2 = (1.f / (1.f - op), 0) }
                let closing = { O1 = (1.f / cl, 0); O2 = (1.f / (1.f - cl), 0) }
                let odds = { OutcomeID = ""; Values = [|{ Value = None; BookOdds = [| { BookID = ""; Odds = { Opening = X2 opening; Closing = X2 closing } } |] }|] }
                let newState = Analytics.compute state score odds
                newState
            ) state
        Assert.That(expected, Is.EqualTo(actual))
    [<Test>]
    member this.ComputeAccuracy_02() =
        let state = {
            BookID = "";
            Opening = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
            Closing = AX2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
        }
        let expected = {
            BookID = "";
            Opening = AX2 { O1 = { Expected = 0.f; Variance = 3.48f }; O2 = { Expected = 0.f; Variance = 3.48f } };
            Closing = AX2 { O1 = { Expected = 0.8f; Variance = 2.08f }; O2 = { Expected = 0.8f; Variance = 2.08f } }
        }
        let actual = 
            [(0.8f, 0.6f, 0); (0.7f, 0.6f, 0); (0.6f, 0.6f, 0); (0.5f, 0.6f, 0); (0.5f, 0.6f, 1); (0.4f, 0.6f, 1); (0.3f, 0.6f, 1); (0.2f, 0.6f, 1)]
            |> List.fold (fun state (op, cl, h) ->
                let score = { Home = h; Away = 1 - h }
                let opening = { O1 = (1.f / op, 0); O2 = (1.f / (1.f - op), 0) }
                let closing = { O1 = (1.f / cl, 0); O2 = (1.f / (1.f - cl), 0) }
                let odds = { OutcomeID = ""; Values = [|{ Value = None; BookOdds = [| { BookID = ""; Odds = { Opening = X2 opening; Closing = X2 closing } } |] }|] }
                let newState = Analytics.compute state score odds
                newState
            ) state
        Assert.That(expected, Is.EqualTo(actual))