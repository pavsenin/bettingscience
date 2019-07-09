module OddsPortalScraperTests
open NUnit.Framework
open OddsPortalScraper
open Domain

[<TestFixture>]
type InternetTests() = 
    [<Test>]
    member this.ScrapBaseballMLB18Match() =
        let matchID = "Of9rIjv8"
        let matchUrl = "baseball/usa/mlb-2018/cincinnati-reds-pittsburgh-pirates-" + matchID + "/"
        let actual = extractMatchOdds baseballID [outHomeAwayID; outOverUnderID; outAsianHandicapID] (matchID, matchUrl)
        let expected =
            Some {
                ID = "Of9rIjv8";
                Url = "http://www.oddsportal.com/baseball/usa/mlb-2018/cincinnati-reds-pittsburgh-pirates-Of9rIjv8/";
                Time = 1538334600;
                Score = { Home = 5; Away = 6 };
                Odds = [|
                    { OutcomeID = "3";
                    Values = [|{ Value = None; Odds = { Starting = X2 {O1 = 1.78f; O2 = 2.18f}; Closing = X2 {O1 = 1.81f; O2 = 2.14f}} } |]};
                    { OutcomeID = "2";
                    Values =
                    [|
                        { Value = Some 8.5f; Odds = {Starting = X2 {O1 = 1.61f; O2 = 2.45f}; Closing = X2 {O1 = 1.61f; O2 = 2.48f } } };
                        { Value = Some 9.0f; Odds = {Starting = X2 {O1 = 1.73f; O2 = 2.20f;}; Closing = X2 {O1 = 1.75f; O2 = 2.21f } } };
                        { Value = Some 9.5f; Odds = {Starting = X2 {O1 = 1.95f; O2 = 1.95f;}; Closing = X2 {O1 = 1.94f; O2 = 1.96f } } };
                        { Value = Some 10.0f; Odds = {Starting = X2 {O1 = 2.11f; O2 = 1.79f;}; Closing = X2 {O1 = 2.12f; O2 = 1.81f } } };
                        { Value = Some 10.5f; Odds = {Starting = X2 {O1 = 2.27f; O2 = 1.69f;}; Closing = X2 {O1 = 2.27f; O2 = 1.71f } } }
                    |]};
                    { OutcomeID = "5";
                    Values =
                    [|
                        { Value = Some -2.5f; Odds = {Starting = X2 {O1 = 3.43f; O2 = 1.34f;}; Closing = X2 {O1 = 3.48f; O2 = 1.33f } } };
                        { Value = Some -2.0f; Odds = {Starting = X2 {O1 = 3.12f; O2 = 1.40f;}; Closing = X2 {O1 = 3.17f; O2 = 1.39f } } };
                        { Value = Some -1.5f; Odds = {Starting = X2 {O1 = 2.58f; O2 = 1.57f;}; Closing = X2 {O1 = 2.62f; O2 = 1.56f } } };
                        { Value = Some -1.0f; Odds = {Starting = X2 {O1 = 2.12f; O2 = 1.79f;}; Closing = X2 {O1 = 2.17f; O2 = 1.76f } } };
                        { Value = Some 1.5f; Odds = {Starting = X2 {O1 = 1.47f; O2 = 2.86f;}; Closing = X2 {O1 = 1.48f; O2 = 2.83f } } }
                    |]}
                |]}
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapSoccerRPL1198Match() =
        let matchID = "6mrwJVoQ"
        let matchUrl = "soccer/russia/premier-league-2018-2019/dynamo-moscow-arsenal-tula-" + matchID + "/"
        let actual = extractMatchOdds soccerID [out1x2ID; outOverUnderID; outAsianHandicapID] (matchID, matchUrl)
        let text = sprintf "%A" actual
        let expected =
            Some {
                ID = "6mrwJVoQ";
                Url = "http://www.oddsportal.com/soccer/russia/premier-league-2018-2019/dynamo-moscow-arsenal-tula-6mrwJVoQ/";
                Time = 1558868400;
                Score = { Home = 3; Away = 3 };
                Odds = [|
                    { OutcomeID = "1";
                    Values = [| { Value = None; Odds = {Starting = X3 {O1 = 2.19f; O0 = 3.32f; O2 = 3.41f }; Closing = X3 {O1 = 1.74f; O0 = 3.73f; O2 = 5.17f } } }|] };
                    { OutcomeID = "2";
                    Values =
                    [|
                        { Value = Some 1.75f; Odds = {Starting = X2 {O1 = 1.53f; O2 = 2.41f;}; Closing = X2 {O1 = 1.43f; O2 = 2.82f } } };
                        { Value = Some 2.0f; Odds = {Starting = X2 {O1 = 1.75f; O2 = 2.07f;}; Closing = X2 {O1 = 1.58f; O2 = 2.42f } } };
                        { Value = Some 2.25f; Odds = {Starting = X2 {O1 = 2.07f; O2 = 1.76f;}; Closing = X2 {O1 = 1.88f; O2 = 2.0f } } };
                        { Value = Some 2.5f; Odds = {Starting = X2 {O1 = 2.35f; O2 = 1.56f;}; Closing = X2 {O1 = 2.15f; O2 = 1.75f } } };
                        { Value = Some 2.75f; Odds = {Starting = X2 {O1 = 2.81f; O2 = 1.37f;}; Closing = X2 {O1 = 2.51f; O2 = 1.54f } } }
                    |]};
                    { OutcomeID = "5";
                    Values =
                    [|
                        { Value = Some -1.25f; Odds = {Starting = X2 {O1 = 3.06f; O2 = 1.38f;}; Closing = X2 {O1 = 2.94f; O2 = 1.41f } } };
                        { Value = Some -1.0f; Odds = {Starting = X2 {O1 = 3.29f; O2 = 1.28f;}; Closing = X2 {O1 = 2.48f; O2 = 1.57f } } };
                        { Value = Some -0.75f; Odds = {Starting = X2 {O1 = 2.62f; O2 = 1.46f;}; Closing = X2 {O1 = 1.99f; O2 = 1.91f } } };
                        { Value = Some -0.5f; Odds = {Starting = X2 {O1 = 2.17f; O2 = 1.69f;}; Closing = X2 {O1 = 1.73f; O2 = 2.21f } } };
                        { Value = Some -0.25f; Odds = {Starting = X2 {O1 = 1.88f; O2 = 1.96f;}; Closing = X2 {O1 = 1.49f; O2 = 2.69f } } };
                        { Value = Some 0.0f; Odds = {Starting = X2 {O1 = 1.54f; O2 = 2.44f;}; Closing = X2 {O1 = 1.30f; O2 = 3.48f } } };
                        { Value = Some 0.25f; Odds = {Starting = X2 {O1 = 1.37f; O2 = 2.88f;}; Closing = X2 {O1 = 1.34f; O2 = 2.99f } } }
                    |]}
                |]}
        Assert.That(actual, Is.EqualTo(expected))


