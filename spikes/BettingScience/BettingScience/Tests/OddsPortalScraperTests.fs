module OddsPortalScraperTests
open NUnit.Framework
open OddsPortalScraper
open Domain

[<TestFixture>]
type InternetTests() =
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
    member this.ScrapBaseballMLB18Match() =
        let matchID = "Of9rIjv8"
        let matchUrl = "baseball/usa/mlb-2018/cincinnati-reds-pittsburgh-pirates-" + matchID + "/"
        let actual = extractMatchOdds baseballID [outHomeAwayID; outOverUnderID; outAsianHandicapID] (matchID, matchUrl)
        let expected =
            Some {
                ID = "Of9rIjv8";
                Url = "http://www.oddsportal.com/baseball/usa/mlb-2018/cincinnati-reds-pittsburgh-pirates-Of9rIjv8/";
                TeamHome = "Cincinnati Reds";
                TeamAway = "Pittsburgh Pirates";
                Time = 1538334600;
                Score = { Home = 5; Away = 6 };
                Periods = [|
                    { Home = 1; Away = 0 }; { Home = 2; Away = 0 }; { Home = 0; Away = 0 }; { Home = 0; Away = 0 }; { Home = 1; Away = 2 };
                    { Home = 1; Away = 2 }; { Home = 0; Away = 1 }; { Home = 0; Away = 0 }; { Home = 0; Away = 0 }; { Home = 0; Away = 1 }
                |]
                Odds = [|
                    { OutcomeID = "3";
                    Values = [|{ Value = None; Odds = { Opening = X2 {O1 = 1.78f; O2 = 2.18f}; Closing = X2 {O1 = 1.81f; O2 = 2.14f}} } |]};
                    { OutcomeID = "2";
                    Values =
                    [|
                        { Value = Some 8.5f; Odds = {Opening = X2 {O1 = 1.61f; O2 = 2.45f}; Closing = X2 {O1 = 1.61f; O2 = 2.48f } } };
                        { Value = Some 9.0f; Odds = {Opening = X2 {O1 = 1.73f; O2 = 2.20f;}; Closing = X2 {O1 = 1.75f; O2 = 2.21f } } };
                        { Value = Some 9.5f; Odds = {Opening = X2 {O1 = 1.95f; O2 = 1.95f;}; Closing = X2 {O1 = 1.94f; O2 = 1.96f } } };
                        { Value = Some 10.0f; Odds = {Opening = X2 {O1 = 2.11f; O2 = 1.79f;}; Closing = X2 {O1 = 2.12f; O2 = 1.81f } } };
                        { Value = Some 10.5f; Odds = {Opening = X2 {O1 = 2.27f; O2 = 1.69f;}; Closing = X2 {O1 = 2.27f; O2 = 1.71f } } }
                    |]};
                    { OutcomeID = "5";
                    Values =
                    [|
                        { Value = Some -2.5f; Odds = {Opening = X2 {O1 = 3.43f; O2 = 1.34f;}; Closing = X2 {O1 = 3.48f; O2 = 1.33f } } };
                        { Value = Some -2.0f; Odds = {Opening = X2 {O1 = 3.12f; O2 = 1.40f;}; Closing = X2 {O1 = 3.17f; O2 = 1.39f } } };
                        { Value = Some -1.5f; Odds = {Opening = X2 {O1 = 2.58f; O2 = 1.57f;}; Closing = X2 {O1 = 2.62f; O2 = 1.56f } } };
                        { Value = Some -1.0f; Odds = {Opening = X2 {O1 = 2.12f; O2 = 1.79f;}; Closing = X2 {O1 = 2.17f; O2 = 1.76f } } };
                        { Value = Some 1.5f; Odds = {Opening = X2 {O1 = 1.47f; O2 = 2.86f;}; Closing = X2 {O1 = 1.48f; O2 = 2.83f } } }
                    |]}
                |]}
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
    [<Test>]
    member this.ScrapSoccerRPL1819Match() =
        let matchID = "6mrwJVoQ"
        let matchUrl = "soccer/russia/premier-league-2018-2019/dynamo-moscow-arsenal-tula-" + matchID + "/"
        let actual = extractMatchOdds soccerID [out1x2ID; outOverUnderID; outAsianHandicapID] (matchID, matchUrl)
        let expected =
            Some {
                ID = "6mrwJVoQ";
                Url = "http://www.oddsportal.com/soccer/russia/premier-league-2018-2019/dynamo-moscow-arsenal-tula-6mrwJVoQ/";
                TeamHome = "Dynamo Moscow";
                TeamAway = "Arsenal Tula";
                Time = 1558868400;
                Score = { Home = 3; Away = 3 };
                Periods = [| { Home = 2; Away = 1 }; { Home = 1; Away = 2 } |]
                Odds = [|
                    { OutcomeID = "1";
                    Values = [| { Value = None; Odds = { Opening = X3 {O1 = 2.19f; O0 = 3.32f; O2 = 3.41f }; Closing = X3 {O1 = 1.74f; O0 = 3.73f; O2 = 5.17f } } }|] };
                    { OutcomeID = "2";
                    Values =
                    [|
                        { Value = Some 1.75f; Odds = { Opening = X2 {O1 = 1.53f; O2 = 2.41f;}; Closing = X2 {O1 = 1.43f; O2 = 2.82f } } };
                        { Value = Some 2.0f; Odds = { Opening = X2 {O1 = 1.75f; O2 = 2.07f;}; Closing = X2 {O1 = 1.58f; O2 = 2.42f } } };
                        { Value = Some 2.25f; Odds = { Opening = X2 {O1 = 2.07f; O2 = 1.76f;}; Closing = X2 {O1 = 1.88f; O2 = 2.0f } } };
                        { Value = Some 2.5f; Odds = { Opening = X2 {O1 = 2.35f; O2 = 1.56f;}; Closing = X2 {O1 = 2.15f; O2 = 1.75f } } };
                        { Value = Some 2.75f; Odds = { Opening = X2 {O1 = 2.81f; O2 = 1.37f;}; Closing = X2 {O1 = 2.51f; O2 = 1.54f } } }
                    |]};
                    { OutcomeID = "5";
                    Values =
                    [|
                        { Value = Some -1.25f; Odds = { Opening = X2 {O1 = 3.06f; O2 = 1.38f;}; Closing = X2 {O1 = 2.94f; O2 = 1.41f } } };
                        { Value = Some -1.0f; Odds = { Opening = X2 {O1 = 3.29f; O2 = 1.28f;}; Closing = X2 {O1 = 2.48f; O2 = 1.57f } } };
                        { Value = Some -0.75f; Odds = { Opening = X2 {O1 = 2.62f; O2 = 1.46f;}; Closing = X2 {O1 = 1.99f; O2 = 1.91f } } };
                        { Value = Some -0.5f; Odds = { Opening = X2 {O1 = 2.17f; O2 = 1.69f;}; Closing = X2 {O1 = 1.73f; O2 = 2.21f } } };
                        { Value = Some -0.25f; Odds = { Opening = X2 {O1 = 1.88f; O2 = 1.96f;}; Closing = X2 {O1 = 1.49f; O2 = 2.69f } } };
                        { Value = Some 0.0f; Odds = { Opening = X2 {O1 = 1.54f; O2 = 2.44f;}; Closing = X2 {O1 = 1.30f; O2 = 3.48f } } };
                        { Value = Some 0.25f; Odds = { Opening = X2 {O1 = 1.37f; O2 = 2.88f;}; Closing = X2 {O1 = 1.34f; O2 = 2.99f } } }
                    |]}
                |]}
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapBasketballNBA1819Match() =
        let matchID = "juA7zL51"
        let matchUrl = "basketball/usa/nba/toronto-raptors-boston-celtics-" + matchID + "/"
        let actual = extractMatchOdds basketballID [outHomeAwayID; outOverUnderID; outAsianHandicapID] (matchID, matchUrl)
        let expected =
            Some {
                ID = "juA7zL51";
                Url = "http://www.oddsportal.com/basketball/usa/nba/toronto-raptors-boston-celtics-juA7zL51/";
                TeamHome = "Toronto Raptors";
                TeamAway = "Boston Celtics";
                Time = 1551229200;
                Score = { Home = 118; Away = 95 };
                Periods = [|
                    { Home = 30; Away = 32 };
                    { Home = 36; Away = 13 };
                    { Home = 29; Away = 23 };
                    { Home = 23; Away = 27 }
                |];
                Odds =
                [|
                    { OutcomeID = "3"; Values = [|{ Value = None; Odds = { Opening = X2 {O1 = 1.57f; O2 = 2.57f;}; Closing = X2 {O1 = 1.63f; O2 = 2.43f } } }|] };
                    { OutcomeID = "2";
                    Values =
                    [|
                        { Value = Some 223.0f; Odds = { Opening = X2 {O1 = 1.75f; O2 = 2.19f }; Closing = X2 {O1 = 1.76f; O2 = 2.20f } } };
                        { Value = Some 223.5f; Odds = { Opening = X2 {O1 = 1.79f; O2 = 2.13f;}; Closing = X2 {O1 = 1.80f; O2 = 2.13f } } };
                        { Value = Some 224.0f; Odds = { Opening = X2 {O1 = 1.83f; O2 = 2.08f;}; Closing = X2 {O1 = 1.81f; O2 = 2.12f } } };
                        { Value = Some 224.5f; Odds = { Opening = X2 {O1 = 1.79f; O2 = 2.10f;}; Closing = X2 {O1 = 1.82f; O2 = 2.11f } } };
                        { Value = Some 225.0f; Odds = { Opening = X2 {O1 = 1.84f; O2 = 2.04f;}; Closing = X2 {O1 = 1.87f; O2 = 2.04f } } };
                        { Value = Some 225.5f; Odds = { Opening = X2 {O1 = 1.88f; O2 = 1.99f;}; Closing = X2 {O1 = 1.92f; O2 = 1.99f } } };
                        { Value = Some 226.0f; Odds = { Opening = X2 {O1 = 1.93f; O2 = 1.93f;}; Closing = X2 {O1 = 1.97f; O2 = 1.93f } } };
                        { Value = Some 226.5f; Odds = { Opening = X2 {O1 = 1.99f; O2 = 1.88f;}; Closing = X2 {O1 = 2.03f; O2 = 1.88f } } };
                        { Value = Some 227.0f; Odds = { Opening = X2 {O1 = 2.05f; O2 = 1.83f;}; Closing = X2 {O1 = 2.09f; O2 = 1.83f } } };
                        { Value = Some 227.5f; Odds = { Opening = X2 {O1 = 2.12f; O2 = 1.78f;}; Closing = X2 {O1 = 2.16f; O2 = 1.78f } } };
                        { Value = Some 228.0f; Odds = { Opening = X2 {O1 = 2.15f; O2 = 1.79f;}; Closing = X2 {O1 = 2.18f; O2 = 1.77f } } };
                        { Value = Some 228.5f; Odds = { Opening = X2 {O1 = 2.13f; O2 = 1.80f;}; Closing = X2 {O1 = 2.13f; O2 = 1.80f } } };
                        { Value = Some 229.0f; Odds = { Opening = X2 {O1 = 2.20f; O2 = 1.76f;}; Closing = X2 {O1 = 2.20f; O2 = 1.76f } } }
                    |]};
                    { OutcomeID = "5";
                    Values =
                    [|
                        { Value = Some -6.0f; Odds = { Opening = X2 {O1 = 2.25f; O2 = 1.72f;}; Closing = X2 {O1 = 2.27f; O2 = 1.71f } } };
                        { Value = Some -5.5f; Odds = { Opening = X2 {O1 = 2.24f; O2 = 1.73f;}; Closing = X2 {O1 = 2.34f; O2 = 1.68f } } };
                        { Value = Some -5.0f; Odds = { Opening = X2 {O1 = 2.14f; O2 = 1.79f;}; Closing = X2 {O1 = 2.30f; O2 = 1.69f } } };
                        { Value = Some -4.5f; Odds = { Opening = X2 {O1 = 2.04f; O2 = 1.87f;}; Closing = X2 {O1 = 2.18f; O2 = 1.77f } } };
                        { Value = Some -4.0f; Odds = { Opening = X2 {O1 = 1.95f; O2 = 1.95f;}; Closing = X2 {O1 = 2.09f; O2 = 1.83f } } };
                        { Value = Some -3.5f; Odds = { Opening = X2 {O1 = 1.88f; O2 = 2.03f;}; Closing = X2 {O1 = 1.99f; O2 = 1.92f } } };
                        { Value = Some -3.0f; Odds = { Opening = X2 {O1 = 1.79f; O2 = 2.14f;}; Closing = X2 {O1 = 1.91f; O2 = 2.0f } } };
                        { Value = Some -2.5f; Odds = { Opening = X2 {O1 = 1.74f; O2 = 2.23f;}; Closing = X2 {O1 = 1.84f; O2 = 2.08f } } };
                        { Value = Some -2.0f; Odds = { Opening = X2 {O1 = 1.73f; O2 = 2.24f;}; Closing = X2 {O1 = 1.76f; O2 = 2.19f } } }
                    |]}
                |]}
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapBasketballNBA1819League() =
        let leagueID, pageCount = ("C2416Q6r", 2)
        let (sportID, _) = basketballID
        let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + sportID + "/" + leagueID + "/X0/1/0/"
        let actual =
            [1..pageCount]
            |> List.map (fun pageNum -> fetchLeagueMatches leagueRelativeUrl pageNum)
            |> List.concat
        let expected =
            [
                ("fFwk0uQp", "/basketball/usa/nba/golden-state-warriors-toronto-raptors-fFwk0uQp/");
                ("ribDVziK", "/basketball/usa/nba/toronto-raptors-golden-state-warriors-ribDVziK/");
                ("WY1z5noK", "/basketball/usa/nba/golden-state-warriors-toronto-raptors-WY1z5noK/");
                ("IVcW5SVD", "/basketball/usa/nba/golden-state-warriors-toronto-raptors-IVcW5SVD/");
                ("4S9lsqgD", "/basketball/usa/nba/toronto-raptors-golden-state-warriors-4S9lsqgD/");
                ("QyBprPw7", "/basketball/usa/nba/toronto-raptors-golden-state-warriors-QyBprPw7/");
                ("tpirxifr", "/basketball/usa/nba/toronto-raptors-milwaukee-bucks-tpirxifr/");
                ("MBhD4Nl0", "/basketball/usa/nba/milwaukee-bucks-toronto-raptors-MBhD4Nl0/");
                ("StchdiX2", "/basketball/usa/nba/toronto-raptors-milwaukee-bucks-StchdiX2/");
                ("6XedeBn9", "/basketball/usa/nba/portland-trail-blazers-golden-state-warriors-6XedeBn9/");
                ("0z56Ymud", "/basketball/usa/nba/toronto-raptors-milwaukee-bucks-0z56Ymud/");
                ("vR3AX7f2", "/basketball/usa/nba/portland-trail-blazers-golden-state-warriors-vR3AX7f2/");
                ("48BczAAq", "/basketball/usa/nba/milwaukee-bucks-toronto-raptors-48BczAAq/");
                ("IBF1ZTPk", "/basketball/usa/nba/golden-state-warriors-portland-trail-blazers-IBF1ZTPk/");
                ("8xQRuWnM", "/basketball/usa/nba/milwaukee-bucks-toronto-raptors-8xQRuWnM/");
                ("nTOVvj2S", "/basketball/usa/nba/golden-state-warriors-portland-trail-blazers-nTOVvj2S/");
                ("tAnyogRP", "/basketball/usa/nba/toronto-raptors-philadelphia-76ers-tAnyogRP/");
                ("GrurspQi", "/basketball/usa/nba/denver-nuggets-portland-trail-blazers-GrurspQi/");
                ("EksYeXdP", "/basketball/usa/nba/houston-rockets-golden-state-warriors-EksYeXdP/");
                ("xdxkLeH0", "/basketball/usa/nba/portland-trail-blazers-denver-nuggets-xdxkLeH0/");
                ("ALS5Pkik", "/basketball/usa/nba/philadelphia-76ers-toronto-raptors-ALS5Pkik/");
                ("xUN0DL9g", "/basketball/usa/nba/golden-state-warriors-houston-rockets-xUN0DL9g/");
                ("EXiz0L4g", "/basketball/usa/nba/milwaukee-bucks-boston-celtics-EXiz0L4g/");
                ("UJvMlomH", "/basketball/usa/nba/denver-nuggets-portland-trail-blazers-UJvMlomH/");
                ("j70egopo", "/basketball/usa/nba/toronto-raptors-philadelphia-76ers-j70egopo/");
                ("IoOGA3gG", "/basketball/usa/nba/houston-rockets-golden-state-warriors-IoOGA3gG/");
                ("GxxFyOkf", "/basketball/usa/nba/boston-celtics-milwaukee-bucks-GxxFyOkf/");
                ("zy8bwRcN", "/basketball/usa/nba/portland-trail-blazers-denver-nuggets-zy8bwRcN/");
                ("O2uNZ1K6", "/basketball/usa/nba/philadelphia-76ers-toronto-raptors-O2uNZ1K6/");
                ("OOOCBqvA", "/basketball/usa/nba/houston-rockets-golden-state-warriors-OOOCBqvA/");
                ("fPj1v7rH", "/basketball/usa/nba/portland-trail-blazers-denver-nuggets-fPj1v7rH/");
                ("KAr6wpLs", "/basketball/usa/nba/boston-celtics-milwaukee-bucks-KAr6wpLs/");
                ("AayJzr50", "/basketball/usa/nba/philadelphia-76ers-toronto-raptors-AayJzr50/");
                ("GYicumTA", "/basketball/usa/nba/denver-nuggets-portland-trail-blazers-GYicumTA/");
                ("ALK8CPO3", "/basketball/usa/nba/golden-state-warriors-houston-rockets-ALK8CPO3/");
                ("Ovczq6cK", "/basketball/usa/nba/milwaukee-bucks-boston-celtics-Ovczq6cK/");
                ("MBhgtTD4", "/basketball/usa/nba/denver-nuggets-portland-trail-blazers-MBhgtTD4/");
                ("xnwBx4zl", "/basketball/usa/nba/toronto-raptors-philadelphia-76ers-xnwBx4zl/");
                ("SdV3D59c", "/basketball/usa/nba/golden-state-warriors-houston-rockets-SdV3D59c/");
                ("I7wJJkU8", "/basketball/usa/nba/milwaukee-bucks-boston-celtics-I7wJJkU8/");
                ("vVcLS3hP", "/basketball/usa/nba/denver-nuggets-san-antonio-spurs-vVcLS3hP/");
                ("I1dvrQCQ", "/basketball/usa/nba/toronto-raptors-philadelphia-76ers-I1dvrQCQ/");
                ("ddgpi3ks", "/basketball/usa/nba/los-angeles-clippers-golden-state-warriors-ddgpi3ks/");
                ("Y9dq1vE5", "/basketball/usa/nba/san-antonio-spurs-denver-nuggets-Y9dq1vE5/");
                ("EFSqNLbr", "/basketball/usa/nba/golden-state-warriors-los-angeles-clippers-EFSqNLbr/");
                ("b1yzcGW8", "/basketball/usa/nba/houston-rockets-utah-jazz-b1yzcGW8/");
                ("Wl245nOJ", "/basketball/usa/nba/portland-trail-blazers-oklahoma-city-thunder-Wl245nOJ/");
                ("niaaXe8T", "/basketball/usa/nba/denver-nuggets-san-antonio-spurs-niaaXe8T/");
                ("WfHKen4E", "/basketball/usa/nba/philadelphia-76ers-brooklyn-nets-WfHKen4E/");
                ("IB7ryv94", "/basketball/usa/nba/toronto-raptors-orlando-magic-IB7ryv94/");
                ("65RBf5B1", "/basketball/usa/nba/utah-jazz-houston-rockets-65RBf5B1/");
                ("bahS2T3L", "/basketball/usa/nba/detroit-pistons-milwaukee-bucks-bahS2T3L/");
                ("pdQFgPQ7", "/basketball/usa/nba/oklahoma-city-thunder-portland-trail-blazers-pdQFgPQ7/");
                ("C6Y2dRtk", "/basketball/usa/nba/orlando-magic-toronto-raptors-C6Y2dRtk/");
                ("Otx7eode", "/basketball/usa/nba/los-angeles-clippers-golden-state-warriors-Otx7eode/");
                ("GzmX1mJR", "/basketball/usa/nba/indiana-pacers-boston-celtics-GzmX1mJR/");
                ("CrjG5VJ2", "/basketball/usa/nba/utah-jazz-houston-rockets-CrjG5VJ2/");
                ("vkxglUlS", "/basketball/usa/nba/detroit-pistons-milwaukee-bucks-vkxglUlS/");
                ("YBJcI838", "/basketball/usa/nba/san-antonio-spurs-denver-nuggets-YBJcI838/");
                ("IqZbc7Rr", "/basketball/usa/nba/brooklyn-nets-philadelphia-76ers-IqZbc7Rr/");
                ("QofK4kZ8", "/basketball/usa/nba/oklahoma-city-thunder-portland-trail-blazers-QofK4kZ8/");
                ("pWELqlcq", "/basketball/usa/nba/indiana-pacers-boston-celtics-pWELqlcq/");
                ("WnDTsSRe", "/basketball/usa/nba/orlando-magic-toronto-raptors-WnDTsSRe/");
                ("AwCXtns2", "/basketball/usa/nba/los-angeles-clippers-golden-state-warriors-AwCXtns2/");
                ("K8gO39lF", "/basketball/usa/nba/san-antonio-spurs-denver-nuggets-K8gO39lF/");
                ("OMDPr8Ck", "/basketball/usa/nba/brooklyn-nets-philadelphia-76ers-OMDPr8Ck/");
                ("hYZtiW49", "/basketball/usa/nba/houston-rockets-utah-jazz-hYZtiW49/");
                ("2gprWUSq", "/basketball/usa/nba/milwaukee-bucks-detroit-pistons-2gprWUSq/");
                ("pIZnVlrj", "/basketball/usa/nba/boston-celtics-indiana-pacers-pIZnVlrj/");
                ("WOYpjjKF", "/basketball/usa/nba/portland-trail-blazers-oklahoma-city-thunder-WOYpjjKF/");
                ("QoYlkAZL", "/basketball/usa/nba/denver-nuggets-san-antonio-spurs-QoYlkAZL/");
                ("d6tYhhzc", "/basketball/usa/nba/toronto-raptors-orlando-magic-d6tYhhzc/");
                ("zcuxhCk3", "/basketball/usa/nba/golden-state-warriors-los-angeles-clippers-zcuxhCk3/");
                ("r3hjU8cd", "/basketball/usa/nba/philadelphia-76ers-brooklyn-nets-r3hjU8cd/");
                ("I3J6kJT4", "/basketball/usa/nba/houston-rockets-utah-jazz-I3J6kJT4/");
                ("QDYjYHbU", "/basketball/usa/nba/milwaukee-bucks-detroit-pistons-QDYjYHbU/");
                ("dfIAlwqB", "/basketball/usa/nba/portland-trail-blazers-oklahoma-city-thunder-dfIAlwqB/");
                ("lORkg1Uu", "/basketball/usa/nba/boston-celtics-indiana-pacers-lORkg1Uu/");
                ("QTMEmcbH", "/basketball/usa/nba/denver-nuggets-san-antonio-spurs-QTMEmcbH/");
                ("zuU1jaEb", "/basketball/usa/nba/golden-state-warriors-los-angeles-clippers-zuU1jaEb/");
                ("MyQciuah", "/basketball/usa/nba/toronto-raptors-orlando-magic-MyQciuah/");
                ("SEQghLqn", "/basketball/usa/nba/philadelphia-76ers-brooklyn-nets-SEQghLqn/");
                ("G4MXYd6T", "/basketball/usa/nba/denver-nuggets-minnesota-timberwolves-G4MXYd6T/");
                ("IJ3UZxiN", "/basketball/usa/nba/los-angeles-clippers-utah-jazz-IJ3UZxiN/");
                ("zeEPzJyH", "/basketball/usa/nba/portland-trail-blazers-sacramento-kings-zeEPzJyH/");
                ("d4FLyaMA", "/basketball/usa/nba/atlanta-hawks-indiana-pacers-d4FLyaMA/");
                ("SUAHxu74", "/basketball/usa/nba/brooklyn-nets-miami-heat-SUAHxu74/");
                ("lvCDwLib", "/basketball/usa/nba/charlotte-hornets-orlando-magic-lvCDwLib/");
                ("v9UMGcpo", "/basketball/usa/nba/memphis-grizzlies-golden-state-warriors-v9UMGcpo/");
                ("x0reLwxU", "/basketball/usa/nba/milwaukee-bucks-oklahoma-city-thunder-x0reLwxU/");
                ("6FM8v1xh", "/basketball/usa/nba/new-york-knicks-detroit-pistons-6FM8v1xh/");
                ("hCI4usNo", "/basketball/usa/nba/philadelphia-76ers-chicago-bulls-hCI4usNo/");
                ("dlRcMJMN", "/basketball/usa/nba/san-antonio-spurs-dallas-mavericks-dlRcMJMN/");
                ("IcSgNa7H", "/basketball/usa/nba/los-angeles-lakers-portland-trail-blazers-IcSgNa7H/");
                ("OzTkOuhB", "/basketball/usa/nba/oklahoma-city-thunder-houston-rockets-OzTkOuhB/");
                ("AXOoPLx5", "/basketball/usa/nba/utah-jazz-denver-nuggets-AXOoPLx5/");
                ("S8ZtQ1Nb", "/basketball/usa/nba/dallas-mavericks-phoenix-suns-S8ZtQ1Nb/");
                ("lIzwRs8h", "/basketball/usa/nba/chicago-bulls-new-york-knicks-lIzwRs8h/");
                ("jo3t2ezG", "/basketball/usa/nba/minnesota-timberwolves-toronto-raptors-jo3t2ezG/");
                ("KrWYRNhn", "/basketball/usa/nba/new-orleans-pelicans-golden-state-warriors-KrWYRNhn/")]
        Assert.That(actual, Is.EqualTo(expected))


