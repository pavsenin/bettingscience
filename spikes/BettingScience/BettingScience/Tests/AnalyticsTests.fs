module AnalyticsTests
open NUnit.Framework
open Analytics
open Domain
open OddsPortalScraper

[<TestFixture>]
type AccuracyTests() =
    let getAccuracy2 out value data =
        let state = {
            Book = Pin; Count = 0;
            Opening = X2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
            Closing = X2 { O1 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
        }
        data
        |> List.fold (fun state (op, cl, (h, a)) ->
            let score = { Home = h; Away = a }
            let opening = { O1 = (1.f / op, 0); O2 = (1.f / (1.f - op), 0) }
            let closing = { O1 = (1.f / cl, 0); O2 = (1.f / (1.f - cl), 0) }
            let odds = { Outcome = out; Values = [|{ Value = value; BookOdds = [| { Book = Pin; Odds = { Opening = X2 opening; Closing = X2 closing } } |] }|] }
            let newState = Analytics.compute state (score, value) odds
            newState
        ) state
    let getAccuracy3 out value data =
        let state = {
            Book = Pin; Count = 0;
            Opening = X3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } };
            Closing = X3 { O1 = { Expected = 0.f; Variance = 0.f }; O0 = { Expected = 0.f; Variance = 0.f }; O2 = { Expected = 0.f; Variance = 0.f } }
        }
        data
        |> List.fold (fun state ((oO1, oO0, oO2), (cO1, cO0, cO2), (h, a)) ->
            let score = { Home = h; Away = a }
            let opening = { O1 = (1.f / oO1, 0); O0 = (1.f / oO0, 0); O2 = (1.f / oO2, 0) }
            let closing = { O1 = (1.f / cO1, 0); O0 = (1.f / cO0, 0); O2 = (1.f / cO2, 0) }
            let odds = { Outcome = out; Values = [|{ Value = value; BookOdds = [| { Book = Pin; Odds = { Opening = X3 opening; Closing = X3 closing } } |] }|] }
            let newState = Analytics.compute state (score, value) odds
            newState
        ) state
    [<Test>]
    member this.ComputeAccuracyDifferentVariance() =
        let expected = {
            Book = Pin; Count = 8;
            Opening = X2 {O1 = {Expected = 0.0f; Variance = 1.07999992f }; O2 = {Expected = 0.0f; Variance = 1.07999992f } };
            Closing = X2 {O1 = {Expected = 0.0f; Variance = 2.0f }; O2 = {Expected = 0.0f; Variance = 2.0f } }
        }
        let actual = 
            [(0.2f, 0.5f, (0, 1)); (0.3f, 0.5f, (0, 1)); (0.4f, 0.5f, (0, 1)); (0.5f, 0.5f, (0, 1));
             (0.5f, 0.5f, (1, 0)); (0.6f, 0.5f, (1, 0)); (0.7f, 0.5f, (1, 0)); (0.8f, 0.5f, (1, 0))]
            |> getAccuracy2 HA None
        Assert.That(expected, Is.EqualTo(actual))
    [<Test>]
    member this.ComputeAccuracyDifferentExpected() =
        let expected = {
            Book = Pin; Count = 8;
            Opening = X2 {O1 = {Expected = 1.1920929e-07f; Variance = 3.48000026f }; O2 = {Expected = -1.1920929e-07f; Variance = 3.48000026f } };
            Closing = X2 {O1 = {Expected = -0.800000072f; Variance = 2.07999992f }; O2 = {Expected = 0.800000072f; Variance = 2.07999992f } }
        }
        let actual = 
            [(0.8f, 0.6f, (0, 1)); (0.7f, 0.6f, (0, 1)); (0.6f, 0.6f, (0, 1)); (0.5f, 0.6f, (0, 1));
             (0.5f, 0.6f, (1, 0)); (0.4f, 0.6f, (1, 0)); (0.3f, 0.6f, (1, 0)); (0.2f, 0.6f, (1, 0))]
            |> getAccuracy2 HA None
        Assert.That(expected, Is.EqualTo(actual))

    [<TestCase(5, 2.5f, 1.35000002f, -1.60000002f, 0.540000021f, -0.900000036f, 0.190000013f, 3.5f, 2.44999981f, -2.0f, 0.800000072f, -1.5f, 0.450000018f, 1, 0)>]
    [<TestCase(5, -2.5f, 1.35000002f, 3.40000001f, 2.34000015f, -0.900000036f, 0.190000013f, -1.5f, 0.450000018f, 3.0f, 1.80000007f, -1.5f, 0.450000018f, 1, 1)>]
    [<TestCase(5, -2.5f, 1.35000002f, -1.60000002f, 0.540000021f, 4.0999999f, 3.3900001f, -1.5f, 0.450000018f, -2.0f, 0.800000072f, 3.5f, 2.44999981f, 0, 1)>]
    member this.ComputeAccuracy1X2(count, oO1e, oO1v, oO0e, oO0v, oO2e, oO2v, cO1e, cO1v, cO0e, cO0v, cO2e, cO2v, h, a) =
        let expected = {
            Book = Pin; Count = count;
            Opening = X3 { O1 = { Expected = oO1e; Variance = oO1v }; O0 = { Expected = oO0e; Variance = oO0v }; O2 = { Expected = oO2e; Variance = oO2v } };
            Closing = X3 { O1 = { Expected = cO1e; Variance = cO1v }; O0 = { Expected = cO0e; Variance = cO0v }; O2 = { Expected = cO2e; Variance = cO2v } } 
        }
        let actual = 
            [((0.7f, 0.2f, 0.1f), (0.3f, 0.4f, 0.3f), (h, a)); ((0.6f, 0.3f, 0.1f), (0.3f, 0.4f, 0.3f), (h, a));
            ((0.5f, 0.3f, 0.2f), (0.3f, 0.4f, 0.3f), (h, a)); ((0.4f, 0.4f, 0.2f), (0.3f, 0.4f, 0.3f), (h, a));
            ((0.3f, 0.4f, 0.3f), (0.3f, 0.4f, 0.3f), (h, a))]
            |> getAccuracy3 O1X2 None
        Assert.That(expected, Is.EqualTo(actual))
    
    [<TestCase(1.5f, 8, 4.f, 2.27999997f, 3.19999981f, 1.27999985f)>]
    [<TestCase(2.5f, 8, 1.1920929e-07f, 3.48000026f, -0.800000072f, 2.07999992f)>]
    [<TestCase(3.5f, 8, -4.f, 2.27999997f, -4.79999971f, 2.88000035f)>]
    member this.ComputeAccuracyOverUnder(handicap, count, openingExp, openingVar, closingExp, closingVar) =
        let expected = {
            Book = Pin; Count = count;
            Opening = X2 {O1 = {Expected = openingExp; Variance = openingVar }; O2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = X2 {O1 = {Expected = closingExp; Variance = closingVar }; O2 = {Expected = -closingExp; Variance = closingVar } }
        }
        let actual = 
            [(0.8f, 0.6f, (2, 0)); (0.7f, 0.6f, (2, 0)); (0.6f, 0.6f, (2, 0)); (0.5f, 0.6f, (2, 0));
             (0.5f, 0.6f, (3, 0)); (0.4f, 0.6f, (3, 0)); (0.3f, 0.6f, (3, 0)); (0.2f, 0.6f, (3, 0))]
            |> getAccuracy2 OU (Some handicap)
        Assert.That(expected, Is.EqualTo(actual))

    [<TestCase(1.f, 2, 1.f, 0.680000007f, 0.799999952f, 0.319999963f)>]
    [<TestCase(2.f, 1, 0.800000012f, 0.640000045f, 0.399999976f, 0.159999982f)>]
    [<TestCase(3.f, 1, -0.8f, 0.640000045f, -0.600000024f, 0.360000014f)>]
    [<TestCase(4.f, 2, -1.f, 0.680000067f, -1.20000005f, 0.720000029f)>]
    member this.ComputeAccuracyOverUnderWithReturn(handicap, count, openingExp, openingVar, closingExp, closingVar) =
        let expected = {
            Book = Pin; Count = count;
            Opening = X2 {O1 = {Expected = openingExp; Variance = openingVar }; O2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = X2 {O1 = {Expected = closingExp; Variance = closingVar }; O2 = {Expected = -closingExp; Variance = closingVar } }
        }
        let actual = [(0.8f, 0.6f, (2, 0)); (0.2f, 0.6f, (3, 0))] |> getAccuracy2 OU (Some handicap)
        Assert.That(expected, Is.EqualTo(actual))
    
    [<TestCase(-2.5f, 8, -4.f, 2.27999997f, -4.79999971f, 2.88000035f)>]
    [<TestCase(-1.5f, 8, 1.1920929e-07f, 3.48000026f, -0.800000072f, 2.07999992f)>]
    [<TestCase(-0.5f, 8, 4.f, 2.27999997f, 3.19999981f, 1.27999985f)>]
    [<TestCase(0.5f, 8, 4.f, 2.27999997f, 3.19999981f, 1.27999985f)>]
    member this.ComputeAccuracyHandicap(handicap, count, openingExp, openingVar, closingExp, closingVar) =
        let expected = {
            Book = Pin; Count = count;
            Opening = X2 {O1 = {Expected = openingExp; Variance = openingVar }; O2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = X2 {O1 = {Expected = closingExp; Variance = closingVar }; O2 = {Expected = -closingExp; Variance = closingVar } }
        }
        let actual = 
            [(0.8f, 0.6f, (1, 0)); (0.7f, 0.6f, (1, 0)); (0.6f, 0.6f, (1, 0)); (0.5f, 0.6f, (1, 0));
             (0.5f, 0.6f, (2, 0)); (0.4f, 0.6f, (2, 0)); (0.3f, 0.6f, (2, 0)); (0.2f, 0.6f, (2, 0))]
            |> getAccuracy2 AH (Some handicap)
        Assert.That(expected, Is.EqualTo(actual))

    [<TestCase(-3.0f, 2, -1.f, 0.680000067f, -1.20000005f, 0.720000029f)>]
    [<TestCase(-2.0f, 1, -0.800000012f, 0.640000045f, -0.600000024f, 0.360000014f)>]
    [<TestCase(-1.f, 1, 0.800000012f, 0.640000045f, 0.399999976f, 0.159999982f)>]
    [<TestCase(0.f, 2, 1.f, 0.680000007f, 0.799999952f, 0.319999963f)>]
    member this.ComputeAccuracyHandicapWithReturn(handicap, count, openingExp, openingVar, closingExp, closingVar) =
        let expected = {
            Book = Pin; Count = count;
            Opening = X2 {O1 = {Expected = openingExp; Variance = openingVar }; O2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = X2 {O1 = {Expected = closingExp; Variance = closingVar }; O2 = {Expected = -closingExp; Variance = closingVar } }
        }
        let actual = [(0.8f, 0.6f, (1, 0)); (0.2f, 0.6f, (2, 0))] |> getAccuracy2 AH (Some handicap)
        Assert.That(expected, Is.EqualTo(actual))

    [<TestCase(-1.75f)>]
    [<TestCase(-0.25f)>]
    [<TestCase(0.75f)>]
    [<TestCase(1.25f)>]
    member this.CannotComputeAccuracySomeHandicaps(handicap) =
        let expected = {
            Book = Pin; Count = 0;
            Opening = X2 {O1 = {Expected = 0.f; Variance = 0.f }; O2 = {Expected = 0.f; Variance = 0.f } };
            Closing = X2 {O1 = {Expected = 0.f; Variance = 0.f }; O2 = {Expected = 0.f; Variance = 0.f } }
        }
        [| AH; OU |]
        |> Array.iter (fun out ->
            let actual = [(0.8f, 0.6f, (1, 0)); (0.2f, 0.6f, (2, 0))] |> getAccuracy2 out (Some handicap)
            Assert.That(expected, Is.EqualTo(actual))
        )