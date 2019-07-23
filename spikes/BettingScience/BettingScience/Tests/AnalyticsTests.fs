module AnalyticsTests
open NUnit.Framework
open Analytics
open Domain
open OddsPortalScraper
open Utils

[<TestFixture>]
type AccuracyTests() =
    let getAccuracy2 out value data =
        let state = {
            Book = Pin; Count = 0;
            Opening = AX2 { AO1 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } };
            Closing = AX2 { AO1 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
        }
        data
        |> List.fold (fun state (op, cl, (h, a)) ->
            let score = { Home = h; Away = a }
            let opening = { O1 = (1.f / op, 0); O2 = (1.f / (1.f - op), 0) }
            let closing = { O1 = (1.f / cl, 0); O2 = (1.f / (1.f - cl), 0) }
            let book = { Book = Pin; Odds = { Opening = X2 opening; Closing = X2 closing } }
            Analytics.compute state score out value book
        ) state
    let getAccuracy3 out value data =
        let state = {
            Book = Pin; Count = 0;
            Opening = AX3 { AO1 = { Expected = 0.f; Variance = 0.f }; AO0 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } };
            Closing = AX3 { AO1 = { Expected = 0.f; Variance = 0.f }; AO0 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
        }
        data
        |> List.fold (fun state ((oO1, oO0, oO2), (cO1, cO0, cO2), (h, a)) ->
            let score = { Home = h; Away = a }
            let opening = { O1 = (1.f / oO1, 0); O0 = (1.f / oO0, 0); O2 = (1.f / oO2, 0) }
            let closing = { O1 = (1.f / cO1, 0); O0 = (1.f / cO0, 0); O2 = (1.f / cO2, 0) }
            let book = { Book = Pin; Odds = { Opening = X3 opening; Closing = X3 closing } }
            Analytics.compute state score out value book
        ) state
    [<Test>]
    member this.ComputeAccuracyDifferentVariance() =
        let expected = {
            Book = Pin; Count = 8;
            Opening = AX2 { AO1 = {Expected = 0.0f; Variance = 1.08f }; AO2 = {Expected = 0.0f; Variance = 1.08f } };
            Closing = AX2 { AO1 = {Expected = 0.0f; Variance = 2.0f }; AO2 = {Expected = 0.0f; Variance = 2.0f } }
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
            Opening = AX2 { AO1 = {Expected = 0.f; Variance = 3.48f }; AO2 = {Expected = 0.f; Variance = 3.48f } };
            Closing = AX2 { AO1 = {Expected = -0.8f; Variance = 2.08f }; AO2 = {Expected = 0.8f; Variance = 2.08f } }
        }
        let actual = 
            [(0.8f, 0.6f, (0, 1)); (0.7f, 0.6f, (0, 1)); (0.6f, 0.6f, (0, 1)); (0.5f, 0.6f, (0, 1));
             (0.5f, 0.6f, (1, 0)); (0.4f, 0.6f, (1, 0)); (0.3f, 0.6f, (1, 0)); (0.2f, 0.6f, (1, 0))]
            |> getAccuracy2 HA None
        Assert.That(expected, Is.EqualTo(actual))

    [<TestCase(5, 2.5f, 1.35f, -1.6f, 0.54f, -0.9f, 0.19f, 3.5f, 2.45f, -2.f, 0.8f, -1.5f, 0.45f, 1, 0)>]
    [<TestCase(5, -2.5f, 1.35f, 3.4f, 2.34f, -0.9f, 0.19f, -1.5f, 0.45f, 3.0f, 1.8f, -1.5f, 0.45f, 1, 1)>]
    [<TestCase(5, -2.5f, 1.35f, -1.6f, 0.54f, 4.1f, 3.39f, -1.5f, 0.45f, -2.0f, 0.8f, 3.5f, 2.45f, 0, 1)>]
    member this.ComputeAccuracy1X2(count, oO1e, oO1v, oO0e, oO0v, oO2e, oO2v, cO1e, cO1v, cO0e, cO0v, cO2e, cO2v, h, a) =
        let expected = {
            Book = Pin; Count = count;
            Opening = AX3 { AO1 = { Expected = oO1e; Variance = oO1v }; AO0 = { Expected = oO0e; Variance = oO0v }; AO2 = { Expected = oO2e; Variance = oO2v } };
            Closing = AX3 { AO1 = { Expected = cO1e; Variance = cO1v }; AO0 = { Expected = cO0e; Variance = cO0v }; AO2 = { Expected = cO2e; Variance = cO2v } } 
        }
        let actual = 
            [((0.7f, 0.2f, 0.1f), (0.3f, 0.4f, 0.3f), (h, a)); ((0.6f, 0.3f, 0.1f), (0.3f, 0.4f, 0.3f), (h, a));
            ((0.5f, 0.3f, 0.2f), (0.3f, 0.4f, 0.3f), (h, a)); ((0.4f, 0.4f, 0.2f), (0.3f, 0.4f, 0.3f), (h, a));
            ((0.3f, 0.4f, 0.3f), (0.3f, 0.4f, 0.3f), (h, a))]
            |> getAccuracy3 O1X2 None
        Assert.That(expected, Is.EqualTo(actual))
    
    [<TestCase(1.5f, 8, 4.f, 2.28f, 3.2f, 1.28f)>]
    [<TestCase(2.5f, 8, 0.f, 3.48f, -0.8f, 2.08f)>]
    [<TestCase(3.5f, 8, -4.f, 2.28f, -4.8f, 2.88f)>]
    member this.ComputeAccuracyOverUnder(handicap, count, openingExp, openingVar, closingExp, closingVar) =
        let expected = {
            Book = Pin; Count = count;
            Opening = AX2 {AO1 = {Expected = openingExp; Variance = openingVar }; AO2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = AX2 {AO1 = {Expected = closingExp; Variance = closingVar }; AO2 = {Expected = -closingExp; Variance = closingVar } }
        }
        let actual = 
            [(0.8f, 0.6f, (2, 0)); (0.7f, 0.6f, (2, 0)); (0.6f, 0.6f, (2, 0)); (0.5f, 0.6f, (2, 0));
             (0.5f, 0.6f, (3, 0)); (0.4f, 0.6f, (3, 0)); (0.3f, 0.6f, (3, 0)); (0.2f, 0.6f, (3, 0))]
            |> getAccuracy2 OU (Some handicap)
        Assert.That(expected, Is.EqualTo(actual))

    [<TestCase(1.f, 2, 1.f, 0.68f, 0.8f, 0.32f)>]
    [<TestCase(2.f, 1, 0.8f, 0.64f, 0.4f, 0.16f)>]
    [<TestCase(3.f, 1, -0.8f, 0.64f, -0.6f, 0.36f)>]
    [<TestCase(4.f, 2, -1.f, 0.68f, -1.2f, 0.72f)>]
    member this.ComputeAccuracyOverUnderWithReturn(handicap, count, openingExp, openingVar, closingExp, closingVar) =
        let expected = {
            Book = Pin; Count = count;
            Opening = AX2 {AO1 = {Expected = openingExp; Variance = openingVar }; AO2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = AX2 {AO1 = {Expected = closingExp; Variance = closingVar }; AO2 = {Expected = -closingExp; Variance = closingVar } }
        }
        let actual = [(0.8f, 0.6f, (2, 0)); (0.2f, 0.6f, (3, 0))] |> getAccuracy2 OU (Some handicap)
        Assert.That(expected, Is.EqualTo(actual))
    
    [<TestCase(-2.5f, 8, -4.f, 2.28f, -4.8f, 2.88f)>]
    [<TestCase(-1.5f, 8, 0.f, 3.48f, -0.8f, 2.08f)>]
    [<TestCase(-0.5f, 8, 4.f, 2.28f, 3.2f, 1.29f)>]
    [<TestCase(0.5f, 8, 4.f, 2.28f, 3.2f, 1.28f)>]
    member this.ComputeAccuracyHandicap(handicap, count, openingExp, openingVar, closingExp, closingVar) =
        let expected = {
            Book = Pin; Count = count;
            Opening = AX2 {AO1 = {Expected = openingExp; Variance = openingVar }; AO2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = AX2 {AO1 = {Expected = closingExp; Variance = closingVar }; AO2 = {Expected = -closingExp; Variance = closingVar } }
        }
        let actual = 
            [(0.8f, 0.6f, (1, 0)); (0.7f, 0.6f, (1, 0)); (0.6f, 0.6f, (1, 0)); (0.5f, 0.6f, (1, 0));
             (0.5f, 0.6f, (2, 0)); (0.4f, 0.6f, (2, 0)); (0.3f, 0.6f, (2, 0)); (0.2f, 0.6f, (2, 0))]
            |> getAccuracy2 AH (Some handicap)
        Assert.That(expected, Is.EqualTo(actual))

    [<TestCase(-3.0f, 2, -1.f, 0.68f, -1.2f, 0.72f)>]
    [<TestCase(-2.0f, 1, -0.8f, 0.64f, -0.6f, 0.36f)>]
    [<TestCase(-1.f, 1, 0.8f, 0.64f, 0.4f, 0.16f)>]
    [<TestCase(0.f, 2, 1.f, 0.68f, 0.8f, 0.32f)>]
    member this.ComputeAccuracyHandicapWithReturn(handicap, count, openingExp, openingVar, closingExp, closingVar) =
        let expected = {
            Book = Pin; Count = count;
            Opening = AX2 {AO1 = {Expected = openingExp; Variance = openingVar }; AO2 = {Expected = -openingExp; Variance = openingVar } };
            Closing = AX2 {AO1 = {Expected = closingExp; Variance = closingVar }; AO2 = {Expected = -closingExp; Variance = closingVar } }
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
            Opening = AX2 {AO1 = {Expected = 0.f; Variance = 0.f }; AO2 = {Expected = 0.f; Variance = 0.f } };
            Closing = AX2 {AO1 = {Expected = 0.f; Variance = 0.f }; AO2 = {Expected = 0.f; Variance = 0.f } }
        }
        [| AH; OU |]
        |> Array.iter (fun out ->
            let actual = [(0.8f, 0.6f, (1, 0)); (0.2f, 0.6f, (2, 0))] |> getAccuracy2 out (Some handicap)
            Assert.That(expected, Is.EqualTo(actual))
        )