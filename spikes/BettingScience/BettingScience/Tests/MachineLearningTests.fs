module MachineLearningTests
open NUnit.Framework
open Domain
open Analytics
open System.IO
open Microsoft.FSharpLu.Json
open System
open System.Data
open Utils
open Accord.Statistics
open Accord.Statistics.Filters
open Accord.Statistics.Models.Regression.Fitting
open Accord.Statistics.Models.Regression
open Accord.Neuro.Networks
open Accord.Neuro.ActivationFunctions
open Accord.Neuro
open Accord.Neuro.Learning
open Accord.Math
open System.Diagnostics
open System.Collections.Generic

type BookType = Opening | Closing
type DistributionType = Train | Validation
type Model = Plain | Ideal | Book of BookType | Distribution of DistributionType

let [<Literal>] plain = "Plain"
let [<Literal>] ideal = "Ideal"
let [<Literal>] twoFromThree = "TwoFromThree"
let [<Literal>] bookOpening = "Book Opening"
let [<Literal>] bookClosing = "Book Closing"
let [<Literal>] bookFallingCoefficients = "Book Falling Coefficients"
let [<Literal>] distributionTrain = "Distribution Train"
let [<Literal>] distributionValidation = "Distribution Validation"

let getMatches directory trainSeasons validationSeasons =
    let baseDirectory = AppDomain.CurrentDomain.BaseDirectory
    let files = Directory.GetFiles(Path.Combine(baseDirectory, directory), "*.json", SearchOption.AllDirectories)
    let leagues = files |> Array.map (fun file -> Compact.deserializeFile<LeagueData> file)
    let train, valid =
        leagues |> Array.fold(fun (train, valid) l ->
            let contains seasons value = seasons |> List.contains value
            if contains trainSeasons l.Season then l::train, valid
            else if contains validationSeasons l.Season then train, l::valid
            else train, valid
        ) ([], [])
    let collectMatches leagues =
        leagues |> List.map (fun l -> l.Matches) |> Array.concat |> Array.sortBy (fun m -> m.Time)
    collectMatches train, collectMatches valid
let getBookCoefficients getCoef out matchData =
    matchData.Odds |> Array.tryFind (fun o -> o.Outcome = out)
    ||> (fun out ->
        out.Values |> Array.tryFind (fun v -> v.Value = None)
        ||> (fun value ->
            value.BookOdds |> Array.tryFind (fun book -> book.Book = Pin)
            |>> (fun book -> getCoef book.Odds)
        )
    )
let getClosing = getBookCoefficients (fun odds -> odds.Closing)
let getOpening = getBookCoefficients (fun odds -> odds.Opening)
let plainModel out _ =
    match out with
    | O1X2 ->
        let of3 = 1.f / 3.f;
        Some(PX3 { PO1 = of3; PO0 = of3; PO2 = of3 }), None
    | _ ->
        Some(PX2 { PO1 = 0.5f; PO2 = 0.5f }), None
let idealModel out matchData =
    match out with
    | O1X2 ->
        let pred =
            match getMatchResult matchData.Score with
            | O1 -> PX3 { PO1 = 1.f; PO0 = 0.f; PO2 = 0.f }
            | O0 -> PX3 { PO1 = 0.f; PO0 = 1.f; PO2 = 0.f }
            | O2 -> PX3 { PO1 = 0.f; PO0 = 0.f; PO2 = 1.f }
        Some(pred), None
    | HA ->
        match getMatchResult matchData.Score with
        | O1 -> Some(PX2 { PO1 = 1.f; PO2 = 0.f }), None
        | O2 -> Some(PX2 { PO1 = 0.f; PO2 = 1.f }), None
        | _ -> None, None
    | _ -> failwith "TODO"
let twoFromThreeModel out (matchData:MatchData) =
    let shouldValid = matchData.ID.GetHashCode() % 3 <> 0
    match out with
    | O1X2 ->
        let pred =
            match getMatchResult matchData.Score, shouldValid with
            | O1, true | O0, false -> PX3 { PO1 = 1.f; PO0 = 0.f; PO2 = 0.f }
            | O0, true | O2, false -> PX3 { PO1 = 0.f; PO0 = 1.f; PO2 = 0.f }
            | O2, true | O1, false -> PX3 { PO1 = 0.f; PO0 = 0.f; PO2 = 1.f }
        Some(pred), None
    | HA ->
        match getMatchResult matchData.Score, shouldValid with
        | O1, true | O2, false -> Some(PX2 { PO1 = 1.f; PO2 = 0.f }), None
        | O2, true | O1, false -> Some(PX2 { PO1 = 0.f; PO2 = 1.f }), None
        | _ -> None, None
    | _ -> failwith "TODO"
let bookClosingModel out matchData =
    let closing = getClosing out matchData
    closing |>> getProbabilities, None
let bookOpeningModel out matchData =
    let opening = getOpening out matchData
    opening |>> getProbabilities, None
let bookFallingCoefficientsModel out matchData =
    let opening = getOpening out matchData
    let closing = getClosing out matchData
    let openingProb = opening |>> getProbabilities
    let closingProb = closing |>> getProbabilities
    match openingProb, closingProb, opening with
    | Some(PX2 { PO1 = poo1; PO2 = poo2 }), Some(PX2 { PO1 = pco1; PO2 = pco2 }), Some(X2 { O1 = (oo1, _); O2 = (oo2, _) }) ->
        let o1Diff, o2Diff = (pco1 - poo1) / poo1, (pco2 - poo2) / poo2
        if o1Diff > o2Diff then Some(PX2{ PO1 = 1.f; PO2 = 0.f }), Some(O1, oo1)
        else Some(PX2{ PO1 = 0.f; PO2 = 1.f }), Some(O2, oo2)
    | Some(PX3 { PO1 = poo1; PO0 = poo0; PO2 = poo2 }), Some(PX3 { PO1 = pco1; PO0 = pco0; PO2 = pco2 }), Some(X3 { O1 = (oo1, _); O0 = (oo0, _); O2 = (oo2, _) }) ->
        let o1Diff, o0Diff, o2Diff = (pco1 - poo1) / poo1, (pco0 - poo0) / poo0, (pco2 - poo2) / poo2
        if o1Diff > o2Diff && o1Diff > o0Diff then Some(PX3{ PO1 = 1.f; PO0 = 0.f; PO2 = 0.f }), Some(O1, oo1)
        else if o2Diff > o1Diff && o2Diff > o0Diff then Some(PX3{ PO1 = 0.f; PO0 = 0.f; PO2 = 1.f }), Some(O2, oo2)
        else Some(PX3{ PO1 = 0.f; PO0 = 1.f; PO2 = 0.f }), Some(O0, oo0)
    | _ -> None, None
let trainDistibutionModel out (set:MatchData array) =
    let getResultCounts() =
        set
        |> Array.fold (fun (o1, o0, o2) m ->
            let closing = getClosing out m
            match closing with
            | Some _ ->
                match getMatchResult m.Score with
                | O1 -> o1 + 1.f, o0, o2
                | O0 -> o1, o0 + 1.f, o2
                | O2 -> o1, o0, o2 + 1.f
            | _ -> o1, o0, o2
        ) (0.f, 0.f, 0.f)
    match out with
    | O1X2 ->
        let o1, o0, o2 = getResultCounts()
        let total = o1 + o0 + o2
        (fun _ -> Some(PX3 { PO1 = o1 / total; PO0 = o0 / total; PO2 = o2 / total }), None)
    | HA ->
        let o1, _, o2 = getResultCounts()
        let total = o1 + o2
        (fun _ -> Some(PX2 { PO1 = o1 / total; PO2 = o2 / total }), None)
    | _ -> failwith "TODO"
let getModel name out (train, validation) =
    if name = plain then plainModel out
    else if name = ideal then idealModel out
    else if name = twoFromThree then twoFromThreeModel out
    else if name = bookClosing then bookClosingModel out
    else if name = bookOpening then bookOpeningModel out
    else if name = bookFallingCoefficients then bookFallingCoefficientsModel out
    else if name = distributionTrain then trainDistibutionModel out train
    else if name = distributionValidation then trainDistibutionModel out validation
    else failwith "TODO"

let getBet closing prediction =
    match closing, prediction with
    | Some(X3 { O1 = (c1, _); O0 = (c0, _); O2 = (c2, _) }), Some(PX3 { PO1 = p1; PO0 = p0; PO2 = p2 }) ->
        let m1, m0, m2 = c1 * p1, c0 * p0, c2 * p2
        if m1 > m0 && m1 > m2 then Some(O1, c1)
        else if m0 > m1 && m0 > m2 then Some(O0, c0)
        else Some(O2, c2)
    | Some(X2 { O1 = (c1, _); O2 = (c2, _) }), Some(PX2 { PO1 = p1; PO2 = p2 }) ->
        let m1, m2 = c1 * p1, c2 * p2
        if m1 > m2 then Some(O1, c1) else Some(O2, c2)
    | _ -> None
let getWinFlat bet result =
    match bet, result with
    | (O1, c), O1
    | (O0, c), O0
    | (O2, c), O2 -> c - 1.f
    | _ -> -1.f
let getResultsCounts bet (co1, co0, co2) =
    match bet with
    | (O1, _) -> co1 + 1, co0, co2
    | (O0, _) -> co1, co0 + 1, co2
    | (O2, _) -> co1, co0, co2 + 1

[<TestFixture>]
type MachineLearningTests() =
    let mlbTrain, mlbValidation = getMatches "../../../data/baseball/mlb/" [Y12;Y13;Y14;Y15;Y16;Y17] [Y18]
    let russiaRplTrain, russiaRplValidation= getMatches "../../../data/soccer/russia/rpl/" [Y12;Y13;Y14;Y15;Y16;Y17] [Y18]
    let englandAplTrain, englandAplValidation= getMatches "../../../data/soccer/england/apl/" [Y12;Y13;Y14;Y15;Y16;Y17] [Y18]
    let englandNlnTrain, englandNlnValidation= getMatches "../../../data/soccer/england/nln/" [Y12;Y13;Y14;Y15;Y16;Y17] [Y18]

    let checkPredictedModels (exO1, exO0, exO2, expectedAccuracy, exEarn) model out handicap initState validResults data =
        let earn, accuracy, count, (cO1, cO0, cO2) =
            data
            |> Array.fold (fun ((earn, accuracy, count, counts) as state) matchData ->
                let result = getMatchResult matchData.Score
                let isValidResult = validResults |> List.exists (fun r -> result = r)
                if isValidResult then
                    let closing = getClosing out matchData
                    let prediction, predictedBet = model matchData
                    let bet =
                        match predictedBet with 
                        | None -> getBet closing prediction
                        | _ -> predictedBet
                    let real = getReal matchData.Score out handicap
                    match real, prediction, bet with
                    | Some real, Some pred, Some bet ->
                        Debug.WriteLine(sprintf "Money %2f"earn)
                        earn + (getWinFlat bet result),
                        getAccuracy accuracy pred real,
                        count + 1,
                        getResultsCounts bet counts
                    | _ -> state
                else
                    state
            ) (0.f, initState, 0, (0, 0, 0))
        let normAccuracy = normalizeScore accuracy count
        Assert.AreEqual(expectedAccuracy, normAccuracy)
        Assert.AreEqual(count, cO1 + cO0 + cO2)
        Assert.AreEqual(exO1, cO1)
        Assert.AreEqual(exO0, cO0)
        Assert.AreEqual(exO2, cO2)
        Assert.AreEqual(exEarn, earn)

    [<TestCase(plain, 72, 17, 153, 23.4139996f, 25.4107437f, -11.5860004f, 20.5946293f, -11.5860004f, 20.5946293f, 21.5900059f)>]
    [<TestCase(bookOpening, 101, 23, 118, 1.56900001f, 20.9008274f, 1.84000003f, 19.7190094f, -3.4230001f, 18.6012402f, -4.109997f)>]
    [<TestCase(bookClosing, 40, 33, 169, 0.658999979f, 20.7206612f, 0.460999995f, 19.5528927f, -1.11199999f, 18.4731407f, -27.32f)>]
    [<TestCase(bookFallingCoefficients, 96, 73, 73, 8.f, 43.801651f, -4.f, 36.363636f, -4.f, 38.0165291f, 62.36f)>]
    [<TestCase(distributionTrain, 116, 0, 126, -1.75399995f, 24.5148754f, 6.07999992f, 20.4574375f, -4.32600021f, 20.4338837f, 7.550004f)>]
    [<TestCase(distributionValidation, 124, 0, 118, -0.0599999987f, 24.5165291f, 0.0299999993f, 20.3603306f, 0.0299999993f, 20.3603306f, 12.7000055f)>]
    [<TestCase(twoFromThree, 99, 69, 74, 5.f, 20.2479343f, 0.f, 18.181818f, -5.f, 20.2479343f, 247.920029f)>]
    [<TestCase(ideal, 104, 69, 69, 0.f, 0.f, 0.f, 0.f, 0.f, 0.f, 458.86f)>]
    member this.PredictRPL18BasedOnRPL1217_O1X2(name, exO1, exO0, exO2, exeO1, exvO1, exeO0, exvO0, exeO2, exvO2, exEarn) =
        let out, handicap = O1X2, None
        let initState = AX3 { AO1 = { Expected = 0.f; Variance = 0.f }; AO0 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
        let model = getModel name out (russiaRplTrain, russiaRplValidation)
        let expectedState = AX3 { AO1 = { Expected = exeO1; Variance = exvO1 }; AO0 = { Expected = exeO0; Variance = exvO0 }; AO2 = { Expected = exeO2; Variance = exvO2 } }
        checkPredictedModels (exO1, exO0, exO2, expectedState, exEarn) model out handicap initState [O1; O0; O2] russiaRplValidation

    //[<TestCase(plain, 72, 17, 153, 23.4139996f, 25.4107437f, -11.5860004f, 20.5946293f, -11.5860004f, 20.5946293f, 21.5900059f)>]
    //[<TestCase(bookOpening, 101, 23, 118, 1.56900001f, 20.9008274f, 1.84000003f, 19.7190094f, -3.4230001f, 18.6012402f, -4.109997f)>]
    [<TestCase(bookClosing, 61, 61, 257, 12.0819998f, 19.9274406f, -19.7310009f, 15.1269131f, 7.63100004f, 17.1343021f, 16.2700024f)>]
    [<TestCase(bookFallingCoefficients, 123, 99, 157, 58.f, 46.9656982f, -28.f, 32.1899719f, -30.0f, 41.6886559f, 49.3700027f)>]
    //[<TestCase(distributionTrain, 116, 0, 126, -1.75399995f, 24.5148754f, 6.07999992f, 20.4574375f, -4.32600021f, 20.4338837f, 7.550004f)>]
    //[<TestCase(distributionValidation, 124, 0, 118, -0.0599999987f, 24.5165291f, 0.0299999993f, 20.3603306f, 0.0299999993f, 20.3603306f, 12.7000055f)>]
    //[<TestCase(twoFromThree, 99, 69, 74, 5.f, 20.2479343f, 0.f, 18.181818f, -5.f, 20.2479343f, 247.920029f)>]
    //[<TestCase(ideal, 104, 69, 69, 0.f, 0.f, 0.f, 0.f, 0.f, 0.f, 458.86f)>]
    member this.PredictAPL18BasedOnAPL1217_O1X2(name, exO1, exO0, exO2, exeO1, exvO1, exeO0, exvO0, exeO2, exvO2, exEarn) =
        let out, handicap = O1X2, None
        let initState = AX3 { AO1 = { Expected = 0.f; Variance = 0.f }; AO0 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
        let model = getModel name out (englandAplTrain, englandAplValidation)
        let expectedState = AX3 { AO1 = { Expected = exeO1; Variance = exvO1 }; AO0 = { Expected = exeO0; Variance = exvO0 }; AO2 = { Expected = exeO2; Variance = exvO2 } }
        checkPredictedModels (exO1, exO0, exO2, expectedState, exEarn) model out handicap initState [O1; O0; O2] englandAplValidation

    //[<TestCase(plain, 72, 17, 153, 23.4139996f, 25.4107437f, -11.5860004f, 20.5946293f, -11.5860004f, 20.5946293f, 21.5900059f)>]
    //[<TestCase(bookOpening, 101, 23, 118, 1.56900001f, 20.9008274f, 1.84000003f, 19.7190094f, -3.4230001f, 18.6012402f, -4.109997f)>]
    //[<TestCase(bookClosing, 61, 61, 257, 12.0819998f, 19.9274406f, -19.7310009f, 15.1269131f, 7.63100004f, 17.1343021f, 16.2700024f)>]
    [<TestCase(bookFallingCoefficients, 186, 57, 154, -29.f, 43.576828f, 46.f, 33.7531471f, -17.0f, 40.5541573f, -6.7200017f)>]
    //[<TestCase(distributionTrain, 116, 0, 126, -1.75399995f, 24.5148754f, 6.07999992f, 20.4574375f, -4.32600021f, 20.4338837f, 7.550004f)>]
    //[<TestCase(distributionValidation, 124, 0, 118, -0.0599999987f, 24.5165291f, 0.0299999993f, 20.3603306f, 0.0299999993f, 20.3603306f, 12.7000055f)>]
    //[<TestCase(twoFromThree, 99, 69, 74, 5.f, 20.2479343f, 0.f, 18.181818f, -5.f, 20.2479343f, 247.920029f)>]
    //[<TestCase(ideal, 104, 69, 69, 0.f, 0.f, 0.f, 0.f, 0.f, 0.f, 458.86f)>]
    member this.PredictEnglandNLN18BasedOnEnglandNLN1217_O1X2(name, exO1, exO0, exO2, exeO1, exvO1, exeO0, exvO0, exeO2, exvO2, exEarn) =
        let out, handicap = O1X2, None
        let initState = AX3 { AO1 = { Expected = 0.f; Variance = 0.f }; AO0 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
        let model = getModel name out (englandNlnTrain, englandNlnValidation)
        let expectedState = AX3 { AO1 = { Expected = exeO1; Variance = exvO1 }; AO0 = { Expected = exeO0; Variance = exvO0 }; AO2 = { Expected = exeO2; Variance = exvO2 } }
        checkPredictedModels (exO1, exO0, exO2, expectedState, exEarn) model out handicap initState [O1; O0; O2] englandNlnValidation


    [<TestCase(plain, 911, 0, 1794, 72.5f, 25.f, -22.9300251f)>]
    [<TestCase(bookOpening, 1264, 0, 1441, -22.6429996f, 24.0048065f, 35.2800407f)>]
    [<TestCase(bookClosing, 745, 0, 1960, -25.4829998f, 23.9693909f, -83.1499786f)>]
    [<TestCase(bookFallingCoefficients, 1354, 0, 1351, 71.f, 48.9833641f, -13.00994f)>]
    [<TestCase(distributionTrain, 1312, 0, 1393, -19.47f, 24.9177456f, -57.439888f)>]
    [<TestCase(distributionValidation, 1236, 0, 1469, -0.535f, 24.955267f, -71.849884f)>]
    [<TestCase(twoFromThree, 1413, 0, 1292, 12.f, 32.9020348f, 851.0092f)>]
    [<TestCase(ideal, 1425, 0, 1280, 0.f, 0.f, 2591.15454f)>]
    member this.PredictMLB18BasedOnMLB1217_HA(name, exO1, exO0, exO2, exeO1, exv, exEarn) =
        let out, handicap = HA, None
        let initState = AX2 { AO1 = { Expected = 0.f; Variance = 0.f }; AO2 = { Expected = 0.f; Variance = 0.f } }
        let model = getModel name out (mlbTrain, mlbValidation)
        let expectedState = AX2 { AO1 = { Expected = exeO1; Variance = exv }; AO2 = { Expected = -exeO1; Variance = exv } }
        checkPredictedModels (exO1, exO0, exO2, expectedState, exEarn) model out handicap initState [O1; O2] mlbValidation

    [<TestCase(1156, 0.42735675f, 0.169989109f)>]
    member this.PredictMLB18BasedOnMLB1217Coefficients(exCorrect, exAccuracy, exMoney) = // WeightedLogisticRegression
        let out = HA
        let transform data =
            let values, closingOdds, weights =
                data |> Array.choose (fun m ->
                    let opening = getOpening out m
                    let openingProbs = opening |>> getProbabilities
                    let closing = getClosing out m
                    let closingProbs = closing |>> getProbabilities
                    let result = getMatchResult m.Score
                    match opening, openingProbs, closing, closingProbs, result with
                    | _, Some(PX2 { PO1 = oo1 }), Some(X2 { O1 = (o1, _); O2 = (o2, _)} as c), Some (PX2 { PO1 = co1 }), res when res = O1 || res = O2 ->
                        let o1Diff = (co1 - oo1) / oo1
                        let o1Real, w = if result = O1 then 1, o1 - 1.f else 0, o2 - 1.f
                        Some(([|float(oo1); float(o1Diff)|], o1Real), c, float(w))
                    | _ -> None
                ) |> Array.unzip3
            let inputs, outputs = Array.unzip values
            inputs, outputs, closingOdds, weights
        let xTrain, yTrain, _, wTrain = transform mlbTrain
        let xValid, yValid, oddsValid, _ = transform mlbValidation
        let teacher = new IterativeReweightedLeastSquares<LogisticRegression>(MaxIterations = 100, Regularization = 1e-6)
        let regression = teacher.Learn(xTrain, yTrain, wTrain)
        let predicted = regression.Decide(xValid).ToZeroOne()
        let data = Array.zip3 yValid predicted oddsValid
        let correct, money =
            data
            |> Array.fold (fun (correct, money) (result, predict, closing) ->
                if result = predict then
                    let win = 
                        match result, closing with
                        | 0, X2 { O2 = (o2, _) } -> o2
                        | 1, X2 { O1 = (o1, _) } -> o1
                        | _ -> 0.f
                    (correct + 1, money + win - 1.f)
                else
                    (correct, money - 1.f)
            ) (0, 0.f)
        let accuracy = float32(correct) / float32(yValid.Length)
        Assert.AreEqual(exCorrect, correct)
        Assert.AreEqual(exAccuracy, accuracy)
        Assert.AreEqual(exMoney, money)

    [<TestCase(1373, 0.42735675f, 0.169989109f)>]
    member this.PredictMLB18BasedOnMLB1217Coefficients_2(exCorrect, exAccuracy, exMoney) = // DeepBeliefNetwork
        let out = HA
        let transform data =
            let values, closingOdds, weights =
                data |> Array.choose (fun m ->
                    let opening = getOpening out m
                    let openingProbs = opening |>> getProbabilities
                    let closing = getClosing out m
                    let closingProbs = closing |>> getProbabilities
                    let result = getMatchResult m.Score
                    match opening, openingProbs, closing, closingProbs, result with
                    | _, Some(PX2 { PO1 = oo1 }), Some(X2 { O1 = (o1, _); O2 = (o2, _)} as c), Some (PX2 { PO1 = co1 }), res when res = O1 || res = O2 ->
                        let o1Diff = (co1 - oo1) / oo1
                        let o1Real, w = if result = O1 then 1, o1 - 1.f else 0, o2 - 1.f
                        Some(([|float(oo1); float(o1Diff)|], o1Real), c, float(w))
                    | _ -> None
                ) |> Array.unzip3
            let inputs, outputs = Array.unzip values
            inputs, outputs, closingOdds, weights
        let xTrain, yTrain, _, wTrain = transform mlbTrain
        let xValid, yValid, oddsValid, _ = transform mlbValidation
        

        let reformat (y:int []) = 
            let reformatted = Array.create y.Length [||]
            [0..y.Length-1] |> List.iter (fun i ->
                let t = Array.create 2 0.
                t.[y.[i]] <- 1.
                reformatted.[i] <- t
            )
            reformatted
        
        [[|10;2|]]
        //[[|2|];[|5; 2|];[|10; 2|];[|50; 2|]]
        |> List.iter(fun layers ->
            [0.2]
            //[0.2;0.1;0.05;0.01]
            |> List.iter (fun rate ->
                [0.99]
                //[0.8;0.9;0.95;0.99]
                |> List.iter (fun mom ->
                    let network = new DeepBeliefNetwork(new BernoulliFunction(), 2, layers)
                    let weights = new GaussianWeights(network)
                    weights.Randomize()
                    network.UpdateVisibleWeights()

                    let reTrain = reformat yTrain
                    let teacher = new BackPropagationLearning(network, LearningRate = rate, Momentum = mom)
                    [0..99] |> List.iter(fun i ->
                        let error = teacher.RunEpoch(xTrain, reTrain)
                        //Debug.WriteLine(sprintf "Epoch %d Error %3f" (i + 1) error)
                        ()
                    )
                    
                    let predicted = xValid |> Array.map (fun x ->
                        let predicted = network.GenerateOutput(x)
                        let _, predict = predicted.Max()
                        predict
                    )
                    let data = Array.zip3 yValid predicted oddsValid
                    let correct, money =
                        data
                        |> Array.fold (fun (correct, money) (result, predict, closing) ->
                            if result = predict then
                                let win = 
                                    match result, closing with
                                    | 0, X2 { O2 = (o2, _) } -> o2
                                    | 1, X2 { O1 = (o1, _) } -> o1
                                    | _ -> 0.f
                                (correct + 1, money + win - 1.f)
                            else
                                (correct, money - 1.f)
                        ) (0, 0.f)
                    let accuracy = float32(correct) / float32(yValid.Length)
                    Debug.WriteLine(sprintf "%A %2f %2f %d %2f" layers rate mom correct money)
                    //Assert.AreEqual(exCorrect, correct)
                    //Assert.AreEqual(exAccuracy, accuracy)
                    //Assert.AreEqual(exMoney, money)
                )
            )
        )

    [<Test>]
    member this.Transform() =
        let out = HA
        let ignoredTeam = ["American League"; "National League"; "Australia"; "Sacramento River Cats"; "El Paso Chihuahuas"]
        let isIgnoredTeam team = ignoredTeam |> List.contains team
        let filterIgnoredTeams = Array.filter (fun m -> not(isIgnoredTeam m.TeamAway || isIgnoredTeam m.TeamHome))
        let filterDraws = Array.filter (fun m -> getMatchResult m.Score <> O0)
        let filterHasClosingOdds = Array.filter (fun m -> getClosing out m |> Option.isSome)
        let teamHomeColumn, teamAwayColumn, winOddsColumn, resultColumn = "TeamHome", "TeamAway", "WinOdds", "Result"
        let getTable data =
            let table = new DataTable()
            table.Columns.Add(teamHomeColumn) |> ignore
            table.Columns.Add(teamAwayColumn) |> ignore
            table.Columns.Add(winOddsColumn) |> ignore
            table.Columns.Add(resultColumn) |> ignore
            data |> Array.iter (fun m ->
                let result = getMatchResult m.Score
                let resultValue = if result = O1 then 0 else if result = O2 then 1 else failwith "Invalid result"
                let closing = getClosing out m
                let closingOdds =
                    match closing with
                    | Some(X2 { O1 = (o1, _); O2 = (o2, _)}) ->
                        if result = O1 then o1 else if result = O2 then o2 else failwith "Invalid result"
                    | _ -> failwith "Invalid odds"
                table.Rows.Add(m.TeamHome, m.TeamAway, closingOdds, resultValue) |> ignore
            )
            table
        let prepareData =
            filterIgnoredTeams
            >> filterDraws
            >> filterHasClosingOdds
            >> getTable
        let getCodification (table:DataTable) =
            let code = new Codification()
            code.Add(teamHomeColumn, CodificationVariable.Categorical)
            code.Add(teamAwayColumn, CodificationVariable.Categorical)
            code.Add(resultColumn, CodificationVariable.Ordinal)
            code.Learn(table) |> ignore
            code

        let tableTrain = prepareData mlbTrain
        let tableValidation = prepareData mlbValidation
        let code = getCodification tableTrain

        let numberOfInputs = code.NumberOfInputs
        let numberOfOutputs = code.NumberOfOutputs

        let xTrain, names = code.Apply(tableTrain, teamHomeColumn, teamAwayColumn).ToJagged()
        let yTrain = tableTrain.Columns.[resultColumn].ToArray<int>()
        let xValidation = code.Apply(tableValidation, teamHomeColumn, teamAwayColumn).ToJagged()
        let yValidation = tableValidation.Columns.[resultColumn].ToArray<int>()
        let oddsValidation = tableValidation.Columns.[winOddsColumn].ToArray<float32>()

        let reformat (y:int []) = 
            let reformatted = Array.create y.Length [||]
            [0..y.Length-1] |> List.iter (fun i ->
                let t = Array.create 2 0.
                t.[y.[i]] <- 1.
                reformatted.[i] <- t
            )
            reformatted

        [[|2|];[|10; 2|];[|120; 2|];[|300; 2|]]
        |> List.iter(fun layers ->
            [0.2;0.1;0.05;0.01]
            |> List.iter (fun rate ->
                [0.8;0.9;0.95;0.99]
                |> List.iter (fun mom ->
                    let network = new DeepBeliefNetwork(new BernoulliFunction(), 60, layers)
                    let weights = new GaussianWeights(network)
                    weights.Randomize()
                    network.UpdateVisibleWeights()

                    let reTrain = reformat yTrain
                    let teacher = new BackPropagationLearning(network, LearningRate = rate, Momentum = mom)
                    [0..49] |> List.iter(fun i ->
                        let error = teacher.RunEpoch(xTrain, reTrain)
                        //Debug.WriteLine(sprintf "Epoch %d Error %3f" (i + 1) error)
                        ()
                    )
                    
                    let predicted = xValidation |> Array.map (fun x ->
                        let predicted = network.GenerateOutput(x)
                        let _, predict = predicted.Max()
                        predict
                    )
                    let data = Array.zip3 yValidation predicted oddsValidation
                    let correct, money =
                        data
                        |> Array.fold (fun (correct, money) (result, predict, winOdds) ->
                            if result = predict then
                                (correct + 1, money + winOdds - 1.f)
                            else
                                (correct, money - 1.f)
                        ) (0, 0.f)
                    let accuracy = float32(correct) / float32(yValidation.Length)
                    Debug.WriteLine(sprintf "%A %2f %2f %d %2f" layers rate mom correct money)
                    //Assert.AreEqual(exCorrect, correct)
                    //Assert.AreEqual(exAccuracy, accuracy)
                    //Assert.AreEqual(exMoney, money)
                )
            )
        )
