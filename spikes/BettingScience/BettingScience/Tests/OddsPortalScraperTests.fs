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
        let actual = extractMatchOdds (baseballID, [outHomeAwayID; outOverUnderID; outAsianHandicapID]) (matchID, matchUrl)
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


