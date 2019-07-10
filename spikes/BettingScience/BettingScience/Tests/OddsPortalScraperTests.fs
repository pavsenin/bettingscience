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
    member this.ScrapSoccerRPL1819Match() =
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
    [<Test>]
    member this.ScrapBaseballMLB18League() =
        let leagueID, pageCount = ("r3414Mwe", 2)
        let (sportID, _) = baseballID
        let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + sportID + "/" + leagueID + "/X0/1/0/"
        let actual =
            [1..pageCount]
            |> List.map (fun pageNum -> fetchLeagueMatches leagueRelativeUrl pageNum)
            |> List.concat
        let expected = [
            ("0tOlebLn", "/baseball/usa/mlb-2018/los-angeles-dodgers-boston-red-sox-0tOlebLn/");
            ("xWfY3iRJ", "/baseball/usa/mlb-2018/los-angeles-dodgers-boston-red-sox-xWfY3iRJ/");
            ("OS6ju8lJ", "/baseball/usa/mlb-2018/los-angeles-dodgers-boston-red-sox-OS6ju8lJ/");
            ("px8ntlZC", "/baseball/usa/mlb-2018/boston-red-sox-los-angeles-dodgers-px8ntlZC/");
            ("S0DssUJ6", "/baseball/usa/mlb-2018/boston-red-sox-los-angeles-dodgers-S0DssUJ6/");
            ("YVyF77TP", "/baseball/usa/mlb-2018/milwaukee-brewers-los-angeles-dodgers-YVyF77TP/");
            ("lCVJmzs7", "/baseball/usa/mlb-2018/milwaukee-brewers-los-angeles-dodgers-lCVJmzs7/");
            ("lpqZQyTn", "/baseball/usa/mlb-2018/houston-astros-boston-red-sox-lpqZQyTn/");
            ("8foWf3t2", "/baseball/usa/mlb-2018/houston-astros-boston-red-sox-8foWf3t2/");
            ("v7XQ8vds", "/baseball/usa/mlb-2018/los-angeles-dodgers-milwaukee-brewers-v7XQ8vds/");
            ("tUCVyB6C", "/baseball/usa/mlb-2018/los-angeles-dodgers-milwaukee-brewers-tUCVyB6C/");
            ("U3nSeqRe", "/baseball/usa/mlb-2018/houston-astros-boston-red-sox-U3nSeqRe/");
            ("2wERxii6", "/baseball/usa/mlb-2018/los-angeles-dodgers-milwaukee-brewers-2wERxii6/");
            ("nucNdPBk", "/baseball/usa/mlb-2018/boston-red-sox-houston-astros-nucNdPBk/");
            ("lfvKc5dq", "/baseball/usa/mlb-2018/boston-red-sox-houston-astros-lfvKc5dq/");
            ("8GENwXy0", "/baseball/usa/mlb-2018/milwaukee-brewers-los-angeles-dodgers-8GENwXy0/");
            ("fDAJvDMg", "/baseball/usa/mlb-2018/milwaukee-brewers-los-angeles-dodgers-fDAJvDMg/");
            ("ruvW3mCA", "/baseball/usa/mlb-2018/new-york-yankees-boston-red-sox-ruvW3mCA/");
            ("QTIE66Hg", "/baseball/usa/mlb-2018/new-york-yankees-boston-red-sox-QTIE66Hg/");
            ("EmSZghg8", "/baseball/usa/mlb-2018/atlanta-braves-los-angeles-dodgers-EmSZghg8/");
            ("YVaLSy1C", "/baseball/usa/mlb-2018/cleveland-indians-houston-astros-YVaLSy1C/");
            ("IeVtSIkR", "/baseball/usa/mlb-2018/atlanta-braves-los-angeles-dodgers-IeVtSIkR/");
            ("GAwqJwZJ", "/baseball/usa/mlb-2018/colorado-rockies-milwaukee-brewers-GAwqJwZJ/");
            ("r5T97n2m", "/baseball/usa/mlb-2018/boston-red-sox-new-york-yankees-r5T97n2m/");
            ("fu1HTHo6", "/baseball/usa/mlb-2018/houston-astros-cleveland-indians-fu1HTHo6/");
            ("v5WxTbzL", "/baseball/usa/mlb-2018/los-angeles-dodgers-atlanta-braves-v5WxTbzL/");
            ("U1P58Sns", "/baseball/usa/mlb-2018/boston-red-sox-new-york-yankees-U1P58Sns/");
            ("l4k1ffdJ", "/baseball/usa/mlb-2018/milwaukee-brewers-colorado-rockies-l4k1ffdJ/");
            ("0I1DUcWa", "/baseball/usa/mlb-2018/houston-astros-cleveland-indians-0I1DUcWa/");
            ("dtsYTvLE", "/baseball/usa/mlb-2018/los-angeles-dodgers-atlanta-braves-dtsYTvLE/");
            ("ADjcezsD", "/baseball/usa/mlb-2018/milwaukee-brewers-colorado-rockies-ADjcezsD/");
            ("Q7oL3URq", "/baseball/usa/mlb-2018/new-york-yankees-oakland-athletics-Q7oL3URq/");
            ("rwwUUK58", "/baseball/usa/mlb-2018/chicago-cubs-colorado-rockies-rwwUUK58/");
            ("OGdlm3rd", "/baseball/usa/mlb-2018/los-angeles-dodgers-colorado-rockies-OGdlm3rd/");
            ("OSfxF2jP", "/baseball/usa/mlb-2018/pittsburgh-pirates-miami-marlins-OSfxF2jP/");
            ("27ehnNc2", "/baseball/usa/mlb-2018/chicago-cubs-milwaukee-brewers-27ehnNc2/");
            ("d4u0IKkl", "/baseball/usa/mlb-2018/chicago-cubs-st-louis-cardinals-d4u0IKkl/");
            ("OWQeRfHL", "/baseball/usa/mlb-2018/kansas-city-royals-cleveland-indians-OWQeRfHL/");
            ("Of9rIjv8", "/baseball/usa/mlb-2018/cincinnati-reds-pittsburgh-pirates-Of9rIjv8/");
            ("IDtdJ0zr", "/baseball/usa/mlb-2018/colorado-rockies-washington-nationals-IDtdJ0zr/");
            ("bTaPOMCK", "/baseball/usa/mlb-2018/milwaukee-brewers-detroit-tigers-bTaPOMCK/");
            ("Qwp4Hv5f", "/baseball/usa/mlb-2018/minnesota-twins-chicago-white-sox-Qwp4Hv5f/");
            ("xv7WKhfk", "/baseball/usa/mlb-2018/new-york-mets-miami-marlins-xv7WKhfk/");
            ("460LP2cE", "/baseball/usa/mlb-2018/san-diego-padres-arizona-diamondbacks-460LP2cE/");
            ("CW3TNtSQ", "/baseball/usa/mlb-2018/seattle-mariners-texas-rangers-CW3TNtSQ/");
            ("tUYKrH3g", "/baseball/usa/mlb-2018/tampa-bay-rays-toronto-blue-jays-tUYKrH3g/");
            ("p4AvJWO1", "/baseball/usa/mlb-2018/los-angeles-angels-oakland-athletics-p4AvJWO1/");
            ("jsBzKC9e", "/baseball/usa/mlb-2018/boston-red-sox-new-york-yankees-jsBzKC9e/");
            ("QF1HQrr8", "/baseball/usa/mlb-2018/philadelphia-phillies-atlanta-braves-QF1HQrr8/");
            ("fFHRLYvq", "/baseball/usa/mlb-2018/san-francisco-giants-los-angeles-dodgers-fFHRLYvq/");
            ("h6VaQEWR", "/baseball/usa/mlb-2018/baltimore-orioles-houston-astros-h6VaQEWR/");
            ("ptSiSz2F", "/baseball/usa/mlb-2018/seattle-mariners-texas-rangers-ptSiSz2F/");
            ("ABBHQNQA", "/baseball/usa/mlb-2018/los-angeles-angels-oakland-athletics-ABBHQNQA/");
            ("2HxmTGn9", "/baseball/usa/mlb-2018/san-diego-padres-arizona-diamondbacks-2HxmTGn9/");
            ("4WJqUdX2", "/baseball/usa/mlb-2018/colorado-rockies-washington-nationals-4WJqUdX2/");
            ("UHwwp6SO", "/baseball/usa/mlb-2018/baltimore-orioles-houston-astros-UHwwp6SO/");
            ("0zoI5Mlo", "/baseball/usa/mlb-2018/kansas-city-royals-cleveland-indians-0zoI5Mlo/");
            ("jTV15Df2", "/baseball/usa/mlb-2018/milwaukee-brewers-detroit-tigers-jTV15Df2/");
            ("CxXc6gud", "/baseball/usa/mlb-2018/minnesota-twins-chicago-white-sox-CxXc6gud/");
            ("ldN8Sqdb", "/baseball/usa/mlb-2018/new-york-mets-miami-marlins-ldN8Sqdb/");
            ("f1xg7ZPk", "/baseball/usa/mlb-2018/philadelphia-phillies-atlanta-braves-f1xg7ZPk/");
            ("OhvGqclm", "/baseball/usa/mlb-2018/tampa-bay-rays-toronto-blue-jays-OhvGqclm/");
            ("OEFLPstH", "/baseball/usa/mlb-2018/cincinnati-reds-pittsburgh-pirates-OEFLPstH/");
            ("2uP0U5Ro", "/baseball/usa/mlb-2018/baltimore-orioles-houston-astros-2uP0U5Ro/");
            ("tWN4TPth", "/baseball/usa/mlb-2018/san-francisco-giants-los-angeles-dodgers-tWN4TPth/");
            ("SnMCR3B4", "/baseball/usa/mlb-2018/boston-red-sox-new-york-yankees-SnMCR3B4/");
            ("GAwk8FAq", "/baseball/usa/mlb-2018/chicago-cubs-st-louis-cardinals-GAwk8FAq/");
            ("fZu4b5sI", "/baseball/usa/mlb-2018/san-francisco-giants-los-angeles-dodgers-fZu4b5sI/");
            ("UifnBrRN", "/baseball/usa/mlb-2018/san-diego-padres-arizona-diamondbacks-UifnBrRN/");
            ("OEgjA2tU", "/baseball/usa/mlb-2018/seattle-mariners-texas-rangers-OEgjA2tU/");
            ("SMAIHSst", "/baseball/usa/mlb-2018/los-angeles-angels-oakland-athletics-SMAIHSst/");
            ("n9xd0RC5", "/baseball/usa/mlb-2018/kansas-city-royals-cleveland-indians-n9xd0RC5/");
            ("j1erCOBH", "/baseball/usa/mlb-2018/colorado-rockies-washington-nationals-j1erCOBH/");
            ("YP0wD4dB", "/baseball/usa/mlb-2018/milwaukee-brewers-detroit-tigers-YP0wD4dB/");
            ("fZ1ZDps5", "/baseball/usa/mlb-2018/minnesota-twins-chicago-white-sox-fZ1ZDps5/");
            ("GlUCdqCU", "/baseball/usa/mlb-2018/boston-red-sox-new-york-yankees-GlUCdqCU/");
            ("xbV8cPdO", "/baseball/usa/mlb-2018/new-york-mets-miami-marlins-xbV8cPdO/");
            ("p2uCpwZt", "/baseball/usa/mlb-2018/tampa-bay-rays-toronto-blue-jays-p2uCpwZt/");
            ("tWy0aoSB", "/baseball/usa/mlb-2018/baltimore-orioles-houston-astros-tWy0aoSB/");
            ("0C2VEQRb", "/baseball/usa/mlb-2018/philadelphia-phillies-atlanta-braves-0C2VEQRb/");
            ("6D9MGncn", "/baseball/usa/mlb-2018/cincinnati-reds-pittsburgh-pirates-6D9MGncn/");
            ("lxaRF6Ch", "/baseball/usa/mlb-2018/chicago-cubs-st-louis-cardinals-lxaRF6Ch/");
            ("Wvol6KF0", "/baseball/usa/mlb-2018/minnesota-twins-chicago-white-sox-Wvol6KF0/");
            ("8Iwh17ca", "/baseball/usa/mlb-2018/seattle-mariners-texas-rangers-8Iwh17ca/");
            ("jRvZpnDI", "/baseball/usa/mlb-2018/kansas-city-royals-cleveland-indians-jRvZpnDI/");
            ("td7e2mrg", "/baseball/usa/mlb-2018/minnesota-twins-detroit-tigers-td7e2mrg/");
            ("258i3TSn", "/baseball/usa/mlb-2018/chicago-cubs-pittsburgh-pirates-258i3TSn/");
            ("MizVoScC", "/baseball/usa/mlb-2018/new-york-mets-atlanta-braves-MizVoScC/");
            ("8r9m49Dt", "/baseball/usa/mlb-2018/colorado-rockies-philadelphia-phillies-8r9m49Dt/");
            ("WMHL8k5P", "/baseball/usa/mlb-2018/tampa-bay-rays-new-york-yankees-WMHL8k5P/");
            ("r1FeXSrJ", "/baseball/usa/mlb-2018/san-francisco-giants-san-diego-padres-r1FeXSrJ/");
            ("r1yRn8r6", "/baseball/usa/mlb-2018/seattle-mariners-oakland-athletics-r1yRn8r6/");
            ("d43wdCLJ", "/baseball/usa/mlb-2018/los-angeles-angels-texas-rangers-d43wdCLJ/");
            ("0xnMmlTa", "/baseball/usa/mlb-2018/arizona-diamondbacks-los-angeles-dodgers-0xnMmlTa/");
            ("l2rIlUDg", "/baseball/usa/mlb-2018/colorado-rockies-philadelphia-phillies-l2rIlUDg/");
            ("0INvyBqf", "/baseball/usa/mlb-2018/chicago-white-sox-cleveland-indians-0INvyBqf/");
            ("fuqEkAbm", "/baseball/usa/mlb-2018/minnesota-twins-detroit-tigers-fuqEkAbm/");
            ("GlpAjjqs", "/baseball/usa/mlb-2018/chicago-cubs-pittsburgh-pirates-GlpAjjqs/");
            ("ze2seWzQ", "/baseball/usa/mlb-2018/st-louis-cardinals-milwaukee-brewers-ze2seWzQ/");
            ("UcBiY8TC", "/baseball/usa/mlb-2018/tampa-bay-rays-new-york-yankees-UcBiY8TC/")
        ]
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapSoccerRPL1819League() =
        let leagueID, pageCount = ("jytwvQhq", 2)
        let (sportID, _) = soccerID
        let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + sportID + "/" + leagueID + "/X0/1/0/"
        let actual =
            [1..pageCount]
            |> List.map (fun pageNum -> fetchLeagueMatches leagueRelativeUrl pageNum)
            |> List.concat
        let expected =
            [
                ("OrTpxLnj", "/soccer/russia/premier-league-2018-2019/krylya-sovetov-samara-fc-nizhny-novgorod-est-2015-OrTpxLnj/");
                ("ULTtw1Xq", "/soccer/russia/premier-league-2018-2019/tomsk-ufa-ULTtw1Xq/");
                ("SCsFssPS", "/soccer/russia/premier-league-2018-2019/fc-nizhny-novgorod-est-2015-krylya-sovetov-samara-SCsFssPS/");
                ("AymArN9M", "/soccer/russia/premier-league-2018-2019/ufa-tomsk-AymArN9M/");
                ("EgeieSgJ", "/soccer/russia/premier-league-2018-2019/akhmat-grozny-fk-rostov-EgeieSgJ/");
                ("QXO9D99m", "/soccer/russia/premier-league-2018-2019/anzhi-ural-QXO9D99m/");
                ("0jimd8vD", "/soccer/russia/premier-league-2018-2019/cska-moscow-krylya-sovetov-samara-0jimd8vD/");
                ("6mrwJVoQ", "/soccer/russia/premier-league-2018-2019/dynamo-moscow-arsenal-tula-6mrwJVoQ/");
                ("nsQ5Ekfs", "/soccer/russia/premier-league-2018-2019/krasnodar-rubin-kazan-nsQ5Ekfs/");
                ("KfODCTOg", "/soccer/russia/premier-league-2018-2019/lokomotiv-moscow-ufa-KfODCTOg/");
                ("IusHBmv0", "/soccer/russia/premier-league-2018-2019/orenburg-spartak-moscow-IusHBmv0/");
                ("SdqZJBWJ", "/soccer/russia/premier-league-2018-2019/zenit-petersburg-yenisey-SdqZJBWJ/");
                ("hK4Wajul", "/soccer/russia/premier-league-2018-2019/rubin-kazan-anzhi-hK4Wajul/");
                ("4dmfJvt9", "/soccer/russia/premier-league-2018-2019/fk-rostov-zenit-petersburg-4dmfJvt9/");
                ("AeFR0WPs", "/soccer/russia/premier-league-2018-2019/arsenal-tula-krasnodar-AeFR0WPs/");
                ("bJnbIbeF", "/soccer/russia/premier-league-2018-2019/yenisey-dynamo-moscow-bJnbIbeF/");
                ("WA3zaAff", "/soccer/russia/premier-league-2018-2019/ural-lokomotiv-moscow-WA3zaAff/");
                ("Q5ljKKQ2", "/soccer/russia/premier-league-2018-2019/krylya-sovetov-samara-spartak-moscow-Q5ljKKQ2/");
                ("IR1rclP6", "/soccer/russia/premier-league-2018-2019/cska-moscow-akhmat-grozny-IR1rclP6/");
                ("vy3vbU90", "/soccer/russia/premier-league-2018-2019/ufa-orenburg-vy3vbU90/");
                ("ll57eyl2", "/soccer/russia/premier-league-2018-2019/zenit-petersburg-cska-moscow-ll57eyl2/");
                ("Ac63dHYe", "/soccer/russia/premier-league-2018-2019/spartak-moscow-ufa-Ac63dHYe/");
                ("C2OG5LB9", "/soccer/russia/premier-league-2018-2019/akhmat-grozny-krylya-sovetov-samara-C2OG5LB9/");
                ("KSRO3atM", "/soccer/russia/premier-league-2018-2019/yenisey-krasnodar-KSRO3atM/");
                ("403ebw4q", "/soccer/russia/premier-league-2018-2019/lokomotiv-moscow-rubin-kazan-403ebw4q/");
                ("bJQS2JeS", "/soccer/russia/premier-league-2018-2019/anzhi-arsenal-tula-bJQS2JeS/");
                ("jgNK4uRF", "/soccer/russia/premier-league-2018-2019/dynamo-moscow-fk-rostov-jgNK4uRF/");
                ("Gz7accJk", "/soccer/russia/premier-league-2018-2019/orenburg-ural-Gz7accJk/");
                ("0lQgjtCM", "/soccer/russia/premier-league-2018-2019/fk-rostov-krasnodar-0lQgjtCM/");
                ("GzV77ssc", "/soccer/russia/premier-league-2018-2019/cska-moscow-dynamo-moscow-GzV77ssc/");
                ("noMkiMdG", "/soccer/russia/premier-league-2018-2019/krylya-sovetov-samara-ufa-noMkiMdG/");
                ("dKBPpKZq", "/soccer/russia/premier-league-2018-2019/arsenal-tula-lokomotiv-moscow-dKBPpKZq/");
                ("UwZB61d3", "/soccer/russia/premier-league-2018-2019/akhmat-grozny-zenit-petersburg-UwZB61d3/");
                ("2XtQrb4d", "/soccer/russia/premier-league-2018-2019/ural-spartak-moscow-2XtQrb4d/");
                ("OtrMqvkj", "/soccer/russia/premier-league-2018-2019/rubin-kazan-orenburg-OtrMqvkj/");
                ("fBPck0RS", "/soccer/russia/premier-league-2018-2019/yenisey-anzhi-fBPck0RS/");
                ("I5UYf4ci", "/soccer/russia/premier-league-2018-2019/spartak-moscow-rubin-kazan-I5UYf4ci/");
                ("G0jjWKKj", "/soccer/russia/premier-league-2018-2019/orenburg-arsenal-tula-G0jjWKKj/");
                ("QVXtgrS3", "/soccer/russia/premier-league-2018-2019/krasnodar-cska-moscow-QVXtgrS3/");
                ("8f1GxNsN", "/soccer/russia/premier-league-2018-2019/zenit-petersburg-krylya-sovetov-samara-8f1GxNsN/");
                ("4QnnX05p", "/soccer/russia/premier-league-2018-2019/lokomotiv-moscow-yenisey-4QnnX05p/");
                ("ddTxfOCc", "/soccer/russia/premier-league-2018-2019/ufa-ural-ddTxfOCc/");
                ("4MWph2sA", "/soccer/russia/premier-league-2018-2019/dynamo-moscow-akhmat-grozny-4MWph2sA/");
                ("no0KyscT", "/soccer/russia/premier-league-2018-2019/anzhi-fk-rostov-no0KyscT/");
                ("SEjtCHkt", "/soccer/russia/premier-league-2018-2019/arsenal-tula-spartak-moscow-SEjtCHkt/");
                ("8UZT8oOI", "/soccer/russia/premier-league-2018-2019/krylya-sovetov-samara-ural-8UZT8oOI/");
                ("jgaEGcsP", "/soccer/russia/premier-league-2018-2019/yenisey-orenburg-jgaEGcsP/");
                ("KU4k8Zl5", "/soccer/russia/premier-league-2018-2019/zenit-petersburg-dynamo-moscow-KU4k8Zl5/");
                ("0SllAeKh", "/soccer/russia/premier-league-2018-2019/cska-moscow-anzhi-0SllAeKh/");
                ("MyjpBy5n", "/soccer/russia/premier-league-2018-2019/rubin-kazan-ufa-MyjpBy5n/");
                ("tv6o9FZb", "/soccer/russia/premier-league-2018-2019/akhmat-grozny-krasnodar-tv6o9FZb/");
                ("nLYX75wP", "/soccer/russia/premier-league-2018-2019/fk-rostov-lokomotiv-moscow-nLYX75wP/");
                ("rmGjrRDo", "/soccer/russia/premier-league-2018-2019/spartak-moscow-yenisey-rmGjrRDo/");
                ("424at5rb", "/soccer/russia/premier-league-2018-2019/ural-rubin-kazan-424at5rb/");
                ("jJ6Bw3SG", "/soccer/russia/premier-league-2018-2019/krasnodar-zenit-petersburg-jJ6Bw3SG/");
                ("I583uPc4", "/soccer/russia/premier-league-2018-2019/lokomotiv-moscow-cska-moscow-I583uPc4/");
                ("EXusONST", "/soccer/russia/premier-league-2018-2019/dynamo-moscow-krylya-sovetov-samara-EXusONST/");
                ("GtS8Jsyo", "/soccer/russia/premier-league-2018-2019/orenburg-fk-rostov-GtS8Jsyo/");
                ("MwFfsoTi", "/soccer/russia/premier-league-2018-2019/ufa-arsenal-tula-MwFfsoTi/");
                ("CS77vqDA", "/soccer/russia/premier-league-2018-2019/anzhi-akhmat-grozny-CS77vqDA/");
                ("f3eDU7qn", "/soccer/russia/premier-league-2018-2019/fk-rostov-spartak-moscow-f3eDU7qn/");
                ("I5nYPqbH", "/soccer/russia/premier-league-2018-2019/zenit-petersburg-anzhi-I5nYPqbH/");
                ("W8jxP3DN", "/soccer/russia/premier-league-2018-2019/dynamo-moscow-krasnodar-W8jxP3DN/");
                ("OrmUQPrB", "/soccer/russia/premier-league-2018-2019/akhmat-grozny-lokomotiv-moscow-OrmUQPrB/");
                ("SGgLSoEb", "/soccer/russia/premier-league-2018-2019/arsenal-tula-ural-SGgLSoEb/");
                ("phlQR5T4", "/soccer/russia/premier-league-2018-2019/cska-moscow-orenburg-phlQR5T4/");
                ("lQfHTRbh", "/soccer/russia/premier-league-2018-2019/yenisey-ufa-lQfHTRbh/");
                ("dIxAVmUu", "/soccer/russia/premier-league-2018-2019/krylya-sovetov-samara-rubin-kazan-dIxAVmUu/");
                ("lEWgPC76", "/soccer/russia/premier-league-2018-2019/orenburg-akhmat-grozny-lEWgPC76/");
                ("ImybOWMC", "/soccer/russia/premier-league-2018-2019/lokomotiv-moscow-zenit-petersburg-ImybOWMC/");
                ("SbroRYxf", "/soccer/russia/premier-league-2018-2019/rubin-kazan-arsenal-tula-SbroRYxf/");
                ("EevsSENm", "/soccer/russia/premier-league-2018-2019/ural-yenisey-EevsSENm/");
                ("KYvwTf8s", "/soccer/russia/premier-league-2018-2019/ufa-fk-rostov-KYvwTf8s/");
                ("ANXkQhh0", "/soccer/russia/premier-league-2018-2019/spartak-moscow-cska-moscow-ANXkQhh0/");
                ("IPhBXz0Q", "/soccer/russia/premier-league-2018-2019/krasnodar-krylya-sovetov-samara-IPhBXz0Q/");
                ("dvz2NjxJ", "/soccer/russia/premier-league-2018-2019/anzhi-dynamo-moscow-dvz2NjxJ/");
                ("dQCjsY4a", "/soccer/russia/premier-league-2018-2019/orenburg-krylya-sovetov-samara-dQCjsY4a/");
                ("xEyEYGpK", "/soccer/russia/premier-league-2018-2019/krasnodar-anzhi-xEyEYGpK/");
                ("lheJCZN0", "/soccer/russia/premier-league-2018-2019/cska-moscow-ufa-lheJCZN0/");
                ("pKlSADhD", "/soccer/russia/premier-league-2018-2019/zenit-petersburg-orenburg-pKlSADhD/");
                ("tSiBEegl", "/soccer/russia/premier-league-2018-2019/fk-rostov-ural-tSiBEegl/");
                ("WGpOBgw7", "/soccer/russia/premier-league-2018-2019/akhmat-grozny-spartak-moscow-WGpOBgw7/");
                ("bNxAZdVD", "/soccer/russia/premier-league-2018-2019/dynamo-moscow-lokomotiv-moscow-bNxAZdVD/");
                ("A1dFDF8f", "/soccer/russia/premier-league-2018-2019/yenisey-rubin-kazan-A1dFDF8f/");
                ("2yg7Fyvr", "/soccer/russia/premier-league-2018-2019/krylya-sovetov-samara-arsenal-tula-2yg7Fyvr/");
                ("jcDWLw1E", "/soccer/russia/premier-league-2018-2019/spartak-moscow-zenit-petersburg-jcDWLw1E/");
                ("dfGvKHVQ", "/soccer/russia/premier-league-2018-2019/lokomotiv-moscow-krasnodar-dfGvKHVQ/");
                ("C6ESMJo8", "/soccer/russia/premier-league-2018-2019/ufa-akhmat-grozny-C6ESMJo8/");
                ("dbWuaGg1", "/soccer/russia/premier-league-2018-2019/arsenal-tula-yenisey-dbWuaGg1/");
                ("2yXy0dve", "/soccer/russia/premier-league-2018-2019/rubin-kazan-fk-rostov-2yXy0dve/");
                ("zkVqbz97", "/soccer/russia/premier-league-2018-2019/ural-cska-moscow-zkVqbz97/");
                ("pKCzLcGK", "/soccer/russia/premier-league-2018-2019/orenburg-dynamo-moscow-pKCzLcGK/");
                ("lYSX0xPl", "/soccer/russia/premier-league-2018-2019/anzhi-krylya-sovetov-samara-lYSX0xPl/");
                ("40i16b1R", "/soccer/russia/premier-league-2018-2019/krasnodar-orenburg-40i16b1R/");
                ("lAlk90H8", "/soccer/russia/premier-league-2018-2019/akhmat-grozny-ural-lAlk90H8/");
                ("69wT1I9r", "/soccer/russia/premier-league-2018-2019/anzhi-lokomotiv-moscow-69wT1I9r/");
                ("APxb7vnL", "/soccer/russia/premier-league-2018-2019/dynamo-moscow-spartak-moscow-APxb7vnL/");
                ("WYwf8KWE", "/soccer/russia/premier-league-2018-2019/zenit-petersburg-ufa-WYwf8KWE/");
                ("M3d3ocP1", "/soccer/russia/premier-league-2018-2019/fk-rostov-arsenal-tula-M3d3ocP1/");
                ("z7poAt22", "/soccer/russia/premier-league-2018-2019/cska-moscow-rubin-kazan-z7poAt22/")]
        Assert.That(actual, Is.EqualTo(expected))


