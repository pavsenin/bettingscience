module OddsPortalScraperTests
open NUnit.Framework
open OddsPortalScraper
open Domain

[<TestFixture>]
type InternetTests() =
    [<Test>]
    member this.ScrapBaseballMLB18League() =
        let leagueID, pageCount = ("r3414Mwe", 2)
        let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + baseballID + "/" + leagueID + "/X0/1/0/"
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
        let actual = extractMatchOdds [| Pin; BF; B365; Mar |] (baseballID, baseballDataID) [| HA; OU; AH |] (matchID, matchUrl)
        let expected =
            Some({ID = "Of9rIjv8";
                 Url =
                  "http://www.oddsportal.com/baseball/usa/mlb-2018/cincinnati-reds-pittsburgh-pirates-Of9rIjv8/";
                 TeamHome = "Cincinnati Reds";
                 TeamAway = "Pittsburgh Pirates";
                 Time = 1538334600;
                 Score = {Home = 5;
                          Away = 6;};
                 ScoreWithoutOT = None;
                 Periods =
                  [|{Home = 1;
                     Away = 0;}; {Home = 2;
                                  Away = 0;}; {Home = 0;
                                               Away = 0;}; {Home = 0;
                                                            Away = 0;}; {Home = 1;
                                                                         Away = 2;}; {Home = 1;
                                                                                      Away = 2;};
                    {Home = 0;
                     Away = 1;}; {Home = 0;
                                  Away = 0;}; {Home = 0;
                                               Away = 0;}; {Home = 0;
                                                            Away = 1;}|];
                 Odds =
                  [|{Outcome = HA;
                     Values =
                      [|{Value = None;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.77999997f, 1538315769);
                                                   O2 = (2.18000007f, 1538315769);};
                                     Closing = X2 {O1 = (1.80999994f, 1538322295);
                                                   O2 = (2.1400001f, 1538322295);};};};
                            {Book = BF;
                             Odds = {Opening = X2 {O1 = (1.73000002f, 1538319251);
                                                   O2 = (2.1500001f, 1538319251);};
                                     Closing = X2 {O1 = (1.75f, 1538323462);
                                                   O2 = (2.0999999f, 1538323462);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.74000001f, 1538317062);
                                                   O2 = (2.1500001f, 1538317062);};
                                     Closing = X2 {O1 = (1.79999995f, 1538321211);
                                                   O2 = (2.04999995f, 1538321211);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.04999995f, 1538276911);
                                                   O2 = (1.88f, 1538276911);};
                                     Closing = X2 {O1 = (1.82000005f, 1538322565);
                                                   O2 = (2.11999989f, 1538322565);};};}|];}|];};
                    {Outcome = OU;
                     Values =
                      [|{Value = Some 8.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.61000001f, 1538315769);
                                                   O2 = (2.45000005f, 1538315769);};
                                     Closing = X2 {O1 = (1.61000001f, 1538334566);
                                                   O2 = (2.48000002f, 1538334566);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.65999997f, 1538316790);
                                                   O2 = (2.25f, 1538316790);};
                                     Closing = X2 {O1 = (1.63999999f, 1538334599);
                                                   O2 = (2.28999996f, 1538334599);};};}|];};
                        {Value = Some 9.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.73000002f, 1538315769);
                                                   O2 = (2.20000005f, 1538315769);};
                                     Closing = X2 {O1 = (1.75f, 1538334566);
                                                   O2 = (2.21000004f, 1538334566);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.96000004f, 1538276911);
                                                   O2 = (1.96000004f, 1538276911);};
                                     Closing = X2 {O1 = (1.79999995f, 1538334599);
                                                   O2 = (2.05999994f, 1538334599);};};}|];};
                        {Value = Some 9.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.95000005f, 1538315769);
                                                   O2 = (1.95000005f, 1538315769);};
                                     Closing = X2 {O1 = (1.94000006f, 1538334566);
                                                   O2 = (1.96000004f, 1538334566);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.89999998f, 1538317062);
                                                   O2 = (1.89999998f, 1538317062);};
                                     Closing = X2 {O1 = (2.0f, 1538322035);
                                                   O2 = (1.83000004f, 1538322035);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.96000004f, 1538315946);
                                                   O2 = (1.96000004f, 1538315946);};
                                     Closing = X2 {O1 = (2.01999998f, 1538333897);
                                                   O2 = (1.89999998f, 1538333897);};};}|];};
                        {Value = Some 10.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.1099999f, 1538315769);
                                                   O2 = (1.78999996f, 1538315769);};
                                     Closing = X2 {O1 = (2.11999989f, 1538334566);
                                                   O2 = (1.80999994f, 1538334566);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.1099999f, 1538316790);
                                                   O2 = (1.75999999f, 1538316790);};
                                     Closing = X2 {O1 = (2.1400001f, 1538334599);
                                                   O2 = (1.74000001f, 1538334599);};};}|];};
                        {Value = Some 10.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.26999998f, 1538315769);
                                                   O2 = (1.69000006f, 1538315769);};
                                     Closing = X2 {O1 = (2.26999998f, 1538334566);
                                                   O2 = (1.71000004f, 1538334566);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.25f, 1538316790);
                                                   O2 = (1.65999997f, 1538316790);};
                                     Closing = X2 {O1 = (2.23000002f, 1538334599);
                                                   O2 = (1.66999996f, 1538334599);};};}|];}|];};
                    {Outcome = AH;
                     Values =
                      [|{Value = Some -2.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (3.43000007f, 1538316188);
                                                   O2 = (1.34000003f, 1538316188);};
                                     Closing = X2 {O1 = (3.48000002f, 1538322362);
                                                   O2 = (1.33000004f, 1538321116);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (3.57999992f, 1538316790);
                                                   O2 = (1.30999994f, 1538316790);};
                                     Closing = X2 {O1 = (3.5999999f, 1538333897);
                                                   O2 = (1.29999995f, 1538319792);};};}|];};
                        {Value = Some -2.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (3.11999989f, 1538316188);
                                                   O2 = (1.39999998f, 1538316188);};
                                     Closing = X2 {O1 = (3.17000008f, 1538322362);
                                                   O2 = (1.38999999f, 1538321116);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (3.20000005f, 1538316790);
                                                   O2 = (1.37f, 1538316790);};
                                     Closing = X2 {O1 = (3.25f, 1538319792);
                                                   O2 = (1.36000001f, 1538319792);};};}|];};
                        {Value = Some -1.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.57999992f, 1538316188);
                                                   O2 = (1.57000005f, 1538316188);};
                                     Closing = X2 {O1 = (2.61999989f, 1538322362);
                                                   O2 = (1.55999994f, 1538322362);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (2.54999995f, 1538317062);
                                                   O2 = (1.57000005f, 1538317062);};
                                     Closing = X2 {O1 = (2.6500001f, 1538321211);
                                                   O2 = (1.53999996f, 1538321211);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.6400001f, 1538315946);
                                                   O2 = (1.55999994f, 1538315946);};
                                     Closing = X2 {O1 = (2.61999989f, 1538333897);
                                                   O2 = (1.57000005f, 1538325096);};};}|];};
                        {Value = Some -1.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.11999989f, 1538316188);
                                                   O2 = (1.78999996f, 1538316188);};
                                     Closing = X2 {O1 = (2.17000008f, 1538322362);
                                                   O2 = (1.75999999f, 1538322362);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.1099999f, 1538316790);
                                                   O2 = (1.75999999f, 1538316790);};
                                     Closing = X2 {O1 = (2.1400001f, 1538325096);
                                                   O2 = (1.74000001f, 1538325096);};};}|];};
                        {Value = Some 1.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.62f, 1538316790);
                                                   O2 = (2.33999991f, 1538316790);};
                                     Closing = X2 {O1 = (1.63f, 1538328297);
                                                   O2 = (2.30999994f, 1538328297);};};}|];};
                        {Value = Some 1.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.47000003f, 1538318683);
                                                   O2 = (2.8599999f, 1538318683);};
                                     Closing = X2 {O1 = (1.48000002f, 1538333781);
                                                   O2 = (2.82999992f, 1538334566);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.63999999f, 1538276911);
                                                   O2 = (2.44000006f, 1538276911);};
                                     Closing = X2 {O1 = (1.48000002f, 1538319677);
                                                   O2 = (2.6500001f, 1538319677);};};}|];}|];}|];})
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapSoccerRPL1819League() =
        let leagueID, pageCount = ("jytwvQhq", 2)
        let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + soccerID + "/" + leagueID + "/X0/1/0/"
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
        let actual = extractMatchOdds [| Pin; BF; B365; Mar |] (soccerID, soccerDataID) [| O1X2; OU; AH |] (matchID, matchUrl)
        let expected =
            Some({ID = "6mrwJVoQ";
                 Url =
                  "http://www.oddsportal.com/soccer/russia/premier-league-2018-2019/dynamo-moscow-arsenal-tula-6mrwJVoQ/";
                 TeamHome = "Dynamo Moscow";
                 TeamAway = "Arsenal Tula";
                 Time = 1558868400;
                 Score = {Home = 3;
                          Away = 3;};
                 ScoreWithoutOT = None;
                 Periods = [|{Home = 2;
                              Away = 1;}; {Home = 1;
                                           Away = 2;}|];
                 Odds =
                  [|{Outcome = O1X2;
                     Values =
                      [|{Value = None;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X3 {O1 = (2.19000006f, 1558695617);
                                                   O0 = (3.31999993f, 1558695617);
                                                   O2 = (3.41000009f, 1558695617);};
                                     Closing = X3 {O1 = (1.74000001f, 1558868299);
                                                   O0 = (3.73000002f, 1558868356);
                                                   O2 = (5.17000008f, 1558868299);};};};
                            {Book = BF;
                             Odds = {Opening = X3 {O1 = (2.1500001f, 1558330018);
                                                   O0 = (3.0999999f, 1558330018);
                                                   O2 = (3.20000005f, 1558330018);};
                                     Closing = X3 {O1 = (1.79999995f, 1558867397);
                                                   O0 = (3.4000001f, 1558867397);
                                                   O2 = (4.5999999f, 1558867397);};};};
                            {Book = B365;
                             Odds = {Opening = X3 {O1 = (2.20000005f, 1558617455);
                                                   O0 = (3.0999999f, 1558617455);
                                                   O2 = (3.0f, 1558617455);};
                                     Closing = X3 {O1 = (1.72000003f, 1558867736);
                                                   O0 = (3.5999999f, 1558867736);
                                                   O2 = (5.0f, 1558867736);};};};
                            {Book = Mar;
                             Odds = {Opening = X3 {O1 = (2.25f, 1558333900);
                                                   O0 = (3.24000001f, 1558333900);
                                                   O2 = (3.38000011f, 1558333900);};
                                     Closing = X3 {O1 = (1.74000001f, 1558868344);
                                                   O0 = (3.6400001f, 1558868344);
                                                   O2 = (5.0f, 1558868344);};};}|];}|];};
                    {Outcome = OU;
                     Values =
                      [|{Value = Some 0.5f;
                         BookOdds =
                          [|{Book = BF;
                             Odds = {Opening = X2 {O1 = (1.04999995f, 1558330018);
                                                   O2 = (8.0f, 1558330018);};
                                     Closing = X2 {O1 = (1.07000005f, 1558868012);
                                                   O2 = (8.0f, 1558868012);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.10000002f, 1558631301);
                                                   O2 = (7.5f, 1558631301);};
                                     Closing = X2 {O1 = (1.08000004f, 1558725017);
                                                   O2 = (8.5f, 1558857723);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.01999998f, 1558652652);
                                                   O2 = (8.80000019f, 1558652652);};
                                     Closing = X2 {O1 = (1.00999999f, 1558861258);
                                                   O2 = (10.25f, 1558868344);};};}|];};
                        {Value = Some 1.0f;
                         BookOdds = [|{Book = Mar;
                                       Odds = {Opening = X2 {O1 = (1.07000005f, 1558652652);
                                                             O2 = (6.75f, 1558652652);};
                                               Closing = X2 {O1 = (1.04999995f, 1558866058);
                                                             O2 = (8.0f, 1558868344);};};}|];};
                        {Value = Some 1.5f;
                         BookOdds =
                          [|{Book = BF;
                             Odds = {Opening = X2 {O1 = (1.33000004f, 1558330018);
                                                   O2 = (3.0f, 1558330018);};
                                     Closing = X2 {O1 = (1.36000001f, 1558867397);
                                                   O2 = (3.0999999f, 1558867397);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.39999998f, 1558631301);
                                                   O2 = (2.75f, 1558631301);};
                                     Closing = X2 {O1 = (1.36000001f, 1558725017);
                                                   O2 = (3.0f, 1558725017);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.38f, 1558652652);
                                                   O2 = (2.9000001f, 1558652652);};
                                     Closing = X2 {O1 = (1.32000005f, 1558867869);
                                                   O2 = (3.20000005f, 1558867869);};};}|];};
                        {Value = Some 1.75f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.52999997f, 1558695616);
                                                   O2 = (2.41000009f, 1558695616);};
                                     Closing = X2 {O1 = (1.42999995f, 1558868356);
                                                   O2 = (2.81999993f, 1558868356);};};}|];};
                        {Value = Some 2.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.75f, 1558695616);
                                                   O2 = (2.06999993f, 1558695616);};
                                     Closing = X2 {O1 = (1.58000004f, 1558868356);
                                                   O2 = (2.42000008f, 1558868356);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.66999996f, 1558652652);
                                                   O2 = (2.18000007f, 1558652652);};
                                     Closing = X2 {O1 = (1.53999996f, 1558868344);
                                                   O2 = (2.45000005f, 1558868344);};};}|];};
                        {Value = Some 2.25f;
                         BookOdds = [|{Book = Pin;
                                       Odds = {Opening = X2 {O1 = (2.06999993f, 1558695616);
                                                             O2 = (1.75999999f, 1558695616);};
                                               Closing = X2 {O1 = (1.88f, 1558868356);
                                                             O2 = (2.0f, 1558868356);};};}|];};
                        {Value = Some 2.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.3499999f, 1558695617);
                                                   O2 = (1.55999994f, 1558695617);};
                                     Closing = X2 {O1 = (2.1500001f, 1558868356);
                                                   O2 = (1.75f, 1558868356);};};};
                            {Book = BF;
                             Odds = {Opening = X2 {O1 = (2.20000005f, 1558330018);
                                                   O2 = (1.60000002f, 1558330018);};
                                     Closing = X2 {O1 = (2.1500001f, 1558867397);
                                                   O2 = (1.66999996f, 1558867397);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (2.25f, 1558680307);
                                                   O2 = (1.62f, 1558680307);};
                                     Closing = X2 {O1 = (2.0999999f, 1558867602);
                                                   O2 = (1.70000005f, 1558867602);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.33999991f, 1558333900);
                                                   O2 = (1.66999996f, 1558333900);};
                                     Closing = X2 {O1 = (2.1500001f, 1558868344);
                                                   O2 = (1.77999997f, 1558868344);};};}|];};
                        {Value = Some 2.75f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.80999994f, 1558695617);
                                                   O2 = (1.37f, 1558695617);};
                                     Closing = X2 {O1 = (2.50999999f, 1558868356);
                                                   O2 = (1.53999996f, 1558868356);};};}|];};
                        {Value = Some 3.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (3.53999996f, 1558652652);
                                                   O2 = (1.28999996f, 1558652652);};
                                     Closing = X2 {O1 = (3.07999992f, 1558867869);
                                                   O2 = (1.36000001f, 1558866058);};};}|];};
                        {Value = Some 3.5f;
                         BookOdds =
                          [|{Book = BF;
                             Odds = {Opening = X2 {O1 = (4.0f, 1558330018);
                                                   O2 = (1.20000005f, 1558330018);};
                                     Closing = X2 {O1 = (4.0999999f, 1558867397);
                                                   O2 = (1.22000003f, 1558867397);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (4.32999992f, 1558631301);
                                                   O2 = (1.20000005f, 1558631301);};
                                     Closing = X2 {O1 = (3.75f, 1558867736);
                                                   O2 = (1.25f, 1558867736);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (4.30000019f, 1558652652);
                                                   O2 = (1.20000005f, 1558652652);};
                                     Closing = X2 {O1 = (3.79999995f, 1558866058);
                                                   O2 = (1.24000001f, 1558866058);};};}|];};
                        {Value = Some 4.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (7.4000001f, 1558652652);
                                                   O2 = (1.05999994f, 1558652652);};
                                     Closing = X2 {O1 = (6.44999981f, 1558867745);
                                                   O2 = (1.08000004f, 1558861614);};};}|];};
                        {Value = Some 4.5f;
                         BookOdds =
                          [|{Book = BF;
                             Odds = {Opening = X2 {O1 = (8.0f, 1558330018);
                                                   O2 = (1.04999995f, 1558330018);};
                                     Closing = X2 {O1 = (8.0f, 1558863798);
                                                   O2 = (1.07000005f, 1558863798);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (10.0f, 1558631301);
                                                   O2 = (1.05999994f, 1558631301);};
                                     Closing = X2 {O1 = (8.0f, 1558867736);
                                                   O2 = (1.08000004f, 1558867736);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (8.10000038f, 1558652652);
                                                   O2 = (1.02999997f, 1558652652);};
                                     Closing = X2 {O1 = (7.19999981f, 1558867745);
                                                   O2 = (1.04999995f, 1558861614);};};}|];};
                        {Value = Some 5.5f;
                         BookOdds =
                          [|{Book = B365;
                             Odds = {Opening = X2 {O1 = (21.0f, 1558631301);
                                                   O2 = (1.00999999f, 1558631301);};
                                     Closing = X2 {O1 = (19.0f, 1558725017);
                                                   O2 = (1.01999998f, 1558725017);};};}|];}|];};
                    {Outcome = AH;
                     Values =
                      [|{Value = Some -2.5f;
                         BookOdds =
                          [|{Book = B365;
                             Odds = {Opening = X2 {O1 = (7.0f, 1558867470);
                                                   O2 = (1.10000002f, 1558867470);};
                                     Closing = X2 {O1 = (6.80000019f, 1558867668);
                                                   O2 = (1.10000002f, 1558867470);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (9.30000019f, 1558652652);
                                                   O2 = (1.01999998f, 1558652652);};
                                     Closing = X2 {O1 = (6.19999981f, 1558868344);
                                                   O2 = (1.07000005f, 1558867019);};};}|];};
                        {Value = Some -2.25f;
                         BookOdds =
                          [|{Book = B365;
                             Odds = {Opening = X2 {O1 = (7.0f, 1558857589);
                                                   O2 = (1.10000002f, 1558857589);};
                                     Closing = X2 {O1 = (6.4000001f, 1558868190);
                                                   O2 = (1.11000001f, 1558868190);};};}|];};
                        {Value = Some -2.0f;
                         BookOdds =
                          [|{Book = B365;
                             Odds = {Opening = X2 {O1 = (7.0f, 1558826701);
                                                   O2 = (1.10000002f, 1558826701);};
                                     Closing = X2 {O1 = (5.9000001f, 1558868190);
                                                   O2 = (1.13f, 1558868190);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (8.39999962f, 1558652652);
                                                   O2 = (1.03999996f, 1558652652);};
                                     Closing = X2 {O1 = (5.3499999f, 1558868344);
                                                   O2 = (1.12f, 1558867993);};};}|];};
                        {Value = Some -1.75f;
                         BookOdds =
                          [|{Book = B365;
                             Odds = {Opening = X2 {O1 = (4.80000019f, 1558725140);
                                                   O2 = (1.16999996f, 1558725140);};
                                     Closing = X2 {O1 = (3.9000001f, 1558867797);
                                                   O2 = (1.24000001f, 1558867797);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (5.80000019f, 1558652652);
                                                   O2 = (1.11000001f, 1558652652);};
                                     Closing = X2 {O1 = (3.81999993f, 1558868344);
                                                   O2 = (1.23000002f, 1558868344);};};}|];};
                        {Value = Some -1.5f;
                         BookOdds =
                          [|{Book = B365;
                             Odds = {Opening = X2 {O1 = (3.70000005f, 1558725140);
                                                   O2 = (1.25999999f, 1558725140);};
                                     Closing = X2 {O1 = (3.0999999f, 1558867668);
                                                   O2 = (1.35000002f, 1558867668);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (4.55000019f, 1558652652);
                                                   O2 = (1.17999995f, 1558652652);};
                                     Closing = X2 {O1 = (3.0999999f, 1558868344);
                                                   O2 = (1.34000003f, 1558868344);};};}|];};
                        {Value = Some -1.25f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (3.05999994f, 1558867876);
                                                   O2 = (1.38f, 1558867876);};
                                     Closing = X2 {O1 = (2.94000006f, 1558868356);
                                                   O2 = (1.40999997f, 1558868356);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (3.29999995f, 1558725140);
                                                   O2 = (1.32000005f, 1558725140);};
                                     Closing = X2 {O1 = (2.75f, 1558867668);
                                                   O2 = (1.41999996f, 1558867668);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (4.0999999f, 1558652652);
                                                   O2 = (1.22000003f, 1558652652);};
                                     Closing = X2 {O1 = (2.74000001f, 1558868344);
                                                   O2 = (1.42999995f, 1558868344);};};}|];};
                        {Value = Some -1.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (3.28999996f, 1558699620);
                                                   O2 = (1.27999997f, 1558699620);};
                                     Closing = X2 {O1 = (2.48000002f, 1558868356);
                                                   O2 = (1.57000005f, 1558868356);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (2.8499999f, 1558725140);
                                                   O2 = (1.39999998f, 1558725140);};
                                     Closing = X2 {O1 = (2.36999989f, 1558868190);
                                                   O2 = (1.54999995f, 1558868190);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (3.61999989f, 1558652652);
                                                   O2 = (1.27999997f, 1558652652);};
                                     Closing = X2 {O1 = (2.38000011f, 1558868344);
                                                   O2 = (1.57000005f, 1558868344);};};}|];};
                        {Value = Some -0.75f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.61999989f, 1558695616);
                                                   O2 = (1.46000004f, 1558695616);};
                                     Closing = X2 {O1 = (1.99000001f, 1558868299);
                                                   O2 = (1.90999997f, 1558868299);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (2.25f, 1558725140);
                                                   O2 = (1.62f, 1558725140);};
                                     Closing = X2 {O1 = (2.0f, 1558868190);
                                                   O2 = (1.79999995f, 1558868190);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.72000003f, 1558652652);
                                                   O2 = (1.46000004f, 1558652652);};
                                     Closing = X2 {O1 = (1.96000004f, 1558868344);
                                                   O2 = (1.84000003f, 1558868344);};};}|];};
                        {Value = Some -0.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.17000008f, 1558695616);
                                                   O2 = (1.69000006f, 1558695616);};
                                     Closing = X2 {O1 = (1.73000002f, 1558868356);
                                                   O2 = (2.21000004f, 1558868299);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (2.0f, 1558724749);
                                                   O2 = (1.85000002f, 1558724749);};
                                     Closing = X2 {O1 = (1.76999998f, 1558868252);
                                                   O2 = (2.0999999f, 1558868252);};};}|];};
                        {Value = Some -0.25f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.88f, 1558695616);
                                                   O2 = (1.96000004f, 1558695616);};
                                     Closing = X2 {O1 = (1.49000001f, 1558868299);
                                                   O2 = (2.69000006f, 1558868356);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.98000002f, 1558696210);
                                                   O2 = (1.88f, 1558696210);};
                                     Closing = X2 {O1 = (1.51999998f, 1558867668);
                                                   O2 = (2.42000008f, 1558867668);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.96000004f, 1558652652);
                                                   O2 = (1.86000001f, 1558652652);};
                                     Closing = X2 {O1 = (1.52999997f, 1558868344);
                                                   O2 = (2.53999996f, 1558868344);};};}|];};
                        {Value = Some 0.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.53999996f, 1558695616);
                                                   O2 = (2.44000006f, 1558695616);};
                                     Closing = X2 {O1 = (1.29999995f, 1558867738);
                                                   O2 = (3.48000002f, 1558867738);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.39999998f, 1558725140);
                                                   O2 = (2.8499999f, 1558725140);};
                                     Closing = X2 {O1 = (1.32000005f, 1558868190);
                                                   O2 = (3.29999995f, 1558868190);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.63f, 1558333900);
                                                   O2 = (2.45000005f, 1558333900);};
                                     Closing = X2 {O1 = (1.32000005f, 1558868344);
                                                   O2 = (3.77999997f, 1558868344);};};}|];};
                        {Value = Some 0.25f;
                         BookOdds =
                          [|{Book = B365;
                             Odds = {Opening = X2 {O1 = (1.29999995f, 1558725140);
                                                   O2 = (3.45000005f, 1558725140);};
                                     Closing = X2 {O1 = (1.25f, 1558868190);
                                                   O2 = (3.79999995f, 1558868190);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.44000006f, 1558652652);
                                                   O2 = (2.8599999f, 1558652652);};
                                     Closing = X2 {O1 = (1.22000003f, 1558868344);
                                                   O2 = (4.4000001f, 1558868344);};};}|];};
                        {Value = Some 0.5f;
                         BookOdds = [|{Book = B365;
                                       Odds = {Opening = X2 {O1 = (1.24000001f, 1558725140);
                                                             O2 = (3.9000001f, 1558725140);};
                                               Closing = X2 {O1 = (1.21000004f, 1558868190);
                                                             O2 = (4.25f, 1558868190);};};}|];};
                        {Value = Some 0.75f;
                         BookOdds =
                          [|{Book = B365;
                             Odds = {Opening = X2 {O1 = (1.15999997f, 1558725140);
                                                   O2 = (5.25f, 1558725140);};
                                     Closing = X2 {O1 = (1.13f, 1558868190);
                                                   O2 = (5.9000001f, 1558868190);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.23000002f, 1558652652);
                                                   O2 = (4.19999981f, 1558652652);};
                                     Closing = X2 {O1 = (1.11000001f, 1558868344);
                                                   O2 = (6.55000019f, 1558868344);};};}|];};
                        {Value = Some 1.0f;
                         BookOdds = [|{Book = Mar;
                                       Odds = {Opening = X2 {O1 = (1.11000001f, 1558652652);
                                                             O2 = (6.30000019f, 1558652652);};
                                               Closing = X2 {O1 = (1.03999996f, 1558868344);
                                                             O2 = (10.0f, 1558868344);};};}|];};
                        {Value = Some 1.25f;
                         BookOdds = [|{Book = Mar;
                                       Odds = {Opening = X2 {O1 = (1.09000003f, 1558652652);
                                                             O2 = (6.8499999f, 1558652652);};
                                               Closing = X2 {O1 = (1.02999997f, 1558868344);
                                                             O2 = (10.5f, 1558868344);};};}|];};
                        {Value = Some 1.5f;
                         BookOdds = [|{Book = Mar;
                                       Odds = {Opening = X2 {O1 = (1.07000005f, 1558652652);
                                                             O2 = (7.4000001f, 1558652652);};
                                               Closing = X2 {O1 = (1.01999998f, 1558867993);
                                                             O2 = (11.0f, 1558868344);};};}|];};
                        {Value = Some 1.75f;
                         BookOdds = [|{Book = Mar;
                                       Odds = {Opening = X2 {O1 = (1.03999996f, 1558652652);
                                                             O2 = (8.60000038f, 1558652652);};
                                               Closing = X2 {O1 = (1.00999999f, 1558868344);
                                                             O2 = (11.25f, 1558868344);};};}|];};
                        {Value = Some 2.0f;
                         BookOdds = [|{Book = Mar;
                                       Odds = {Opening = X2 {O1 = (1.01999998f, 1558652652);
                                                             O2 = (10.5f, 1558652652);};
                                               Closing = X2 {O1 = (1.00999999f, 1558696077);
                                                             O2 = (11.5f, 1558868344);};};}|];}|];}|];})
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapSoccerMatchWithPenalties() =
        let matchID = "KrdKRsTP"
        let matchUrl = "soccer/russia/premier-league-2016-2017/orenburg-ska-khabarovsk-" + matchID + "/"
        let actual = extractMatchOdds [| Pin; BF; B365; Mar |] (soccerID, soccerDataID) [| O1X2; OU; AH |] (matchID, matchUrl)
        match actual with
        | Some({
                ID = "KrdKRsTP";
                Url = "http://www.oddsportal.com/soccer/russia/premier-league-2016-2017/orenburg-ska-khabarovsk-KrdKRsTP/";
                TeamHome = "Orenburg";
                TeamAway = "SKA Khabarovsk";
                Score = { Home = 0; Away = 1 };
                ScoreWithoutOT = None;
                Periods = [|
                    { Home = 0; Away = 0 };
                    { Home = 0; Away = 0 };
                    { Home = 0; Away = 0 };
                    { Home = 3; Away = 5 }
                |];
          }) -> ()
        | _ -> failwith "Incorrect data"
    [<Test>]
    member this.ScrapSoccerMatchFromBet365() =
        let matchID = "OIyXBXea"
        let matchUrl = "soccer/russia/premier-league-2014-2015/arsenal-tula-akhmat-grozny-" + matchID + "/"
        let actual = extractMatchOdds [| B365 |] (soccerID, soccerDataID) [| O1X2 |] (matchID, matchUrl)
        match actual with
        | Some({
                ID = "OIyXBXea";
                Url = "http://www.oddsportal.com/soccer/russia/premier-league-2014-2015/arsenal-tula-akhmat-grozny-OIyXBXea/";
                Score = { Home = 1; Away = 1 };
                ScoreWithoutOT = None;
                Periods = [|
                    { Home = 1; Away = 1 };
                    { Home = 0; Away = 0 }
                |];
                Odds = [|{
                    Outcome = O1X2;
                    Values = [|{
                        Value = None;
                        BookOdds = [|{
                            Book = B365;
                            Odds = { Opening = X3 { O1 = (2.88f, 1431443386); O0 = (3.2f, 1431443386); O2 = (2.5f, 1431443386) };
                                     Closing = X3 { O1 = (2.38f, 1431763683); O0 = (3.2f, 1431443386); O2 = (3.f, 1431763683) }
                            }
                        }|]
                    }|]
                }|]
            }) -> ()
        | _ -> failwith "Incorrect data"
    [<Test>]
    member this.ScrapBasketballNBA1819Match() =
        let matchID = "juA7zL51"
        let matchUrl = "basketball/usa/nba/toronto-raptors-boston-celtics-" + matchID + "/"
        let actual = extractMatchOdds [| Pin; BF; B365; Mar |] (basketballID, baseballDataID) [| HA; OU; AH |] (matchID, matchUrl)
        let expected =
            Some({ID = "juA7zL51";
                 Url =
                  "http://www.oddsportal.com/basketball/usa/nba/toronto-raptors-boston-celtics-juA7zL51/";
                 TeamHome = "Toronto Raptors";
                 TeamAway = "Boston Celtics";
                 Time = 1551229200;
                 Score = {Home = 118;
                          Away = 95;};
                 ScoreWithoutOT = None;
                 Periods = [|{Home = 30;
                              Away = 32;}; {Home = 36;
                                            Away = 13;}; {Home = 29;
                                                          Away = 23;}; {Home = 23;
                                                                        Away = 27;}|];
                 Odds =
                  [|{Outcome = HA;
                     Values =
                      [|{Value = None;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.57000005f, 1551126082);
                                                   O2 = (2.56999993f, 1551126082);};
                                     Closing = X2 {O1 = (1.63f, 1551228685);
                                                   O2 = (2.43000007f, 1551228685);};};};
                            {Book = BF;
                             Odds = {Opening = X2 {O1 = (1.57000005f, 1551153149);
                                                   O2 = (2.5f, 1551153149);};
                                     Closing = X2 {O1 = (1.63f, 1551220008);
                                                   O2 = (2.45000005f, 1551226708);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.53999996f, 1551133743);
                                                   O2 = (2.6500001f, 1551133743);};
                                     Closing = X2 {O1 = (1.62f, 1551228673);
                                                   O2 = (2.4000001f, 1551228673);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.55999994f, 1551145074);
                                                   O2 = (2.5999999f, 1551145074);};
                                     Closing = X2 {O1 = (1.64999998f, 1551228987);
                                                   O2 = (2.38000011f, 1551228987);};};}|];}|];};
                    {Outcome = OU;
                     Values =
                      [|{Value = Some 222.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.62f, 1551145074);
                                                   O2 = (2.16000009f, 1551145074);};
                                     Closing = X2 {O1 = (1.63f, 1551229123);
                                                   O2 = (2.1400001f, 1551229123);};};}|];};
                        {Value = Some 222.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.65999997f, 1551145074);
                                                   O2 = (2.1099999f, 1551145074);};
                                     Closing = X2 {O1 = (1.65999997f, 1551229123);
                                                   O2 = (2.1099999f, 1551229123);};};}|];};
                        {Value = Some 223.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.69000006f, 1551145074);
                                                   O2 = (2.08999991f, 1551145074);};
                                     Closing = X2 {O1 = (1.70000005f, 1551229123);
                                                   O2 = (2.07999992f, 1551229123);};};}|];};
                        {Value = Some 223.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.73000002f, 1551145074);
                                                   O2 = (2.05999994f, 1551145074);};
                                     Closing = X2 {O1 = (1.74000001f, 1551229123);
                                                   O2 = (2.03999996f, 1551229123);};};}|];};
                        {Value = Some 224.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.76999998f, 1551145074);
                                                   O2 = (2.02999997f, 1551145074);};
                                     Closing = X2 {O1 = (1.76999998f, 1551229123);
                                                   O2 = (2.02999997f, 1551229123);};};}|];};
                        {Value = Some 224.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.78999996f, 1551125604);
                                                   O2 = (2.0999999f, 1551125604);};
                                     Closing = X2 {O1 = (1.82000005f, 1551229121);
                                                   O2 = (2.1099999f, 1551229121);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.80999994f, 1551145074);
                                                   O2 = (2.0f, 1551145074);};
                                     Closing = X2 {O1 = (1.80999994f, 1551229123);
                                                   O2 = (2.0f, 1551229123);};};}|];};
                        {Value = Some 225.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.84000003f, 1551125604);
                                                   O2 = (2.03999996f, 1551125604);};
                                     Closing = X2 {O1 = (1.87f, 1551229121);
                                                   O2 = (2.03999996f, 1551229121);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.85000002f, 1551145074);
                                                   O2 = (1.97000003f, 1551145074);};
                                     Closing = X2 {O1 = (1.85000002f, 1551229123);
                                                   O2 = (1.97000003f, 1551229123);};};}|];};
                        {Value = Some 225.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.88f, 1551125604);
                                                   O2 = (1.99000001f, 1551125604);};
                                     Closing = X2 {O1 = (1.91999996f, 1551229121);
                                                   O2 = (1.99000001f, 1551229121);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.89999998f, 1551145074);
                                                   O2 = (1.94000006f, 1551145074);};
                                     Closing = X2 {O1 = (1.89999998f, 1551229123);
                                                   O2 = (1.94000006f, 1551229123);};};}|];};
                        {Value = Some 226.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.92999995f, 1551125604);
                                                   O2 = (1.92999995f, 1551125604);};
                                     Closing = X2 {O1 = (1.97000003f, 1551229121);
                                                   O2 = (1.92999995f, 1551229121);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.89999998f, 1551133743);
                                                   O2 = (1.89999998f, 1551133743);};
                                     Closing = X2 {O1 = (1.89999998f, 1551133743);
                                                   O2 = (1.89999998f, 1551133743);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.92999995f, 1551145074);
                                                   O2 = (1.92999995f, 1551145074);};
                                     Closing = X2 {O1 = (1.92999995f, 1551227352);
                                                   O2 = (1.92999995f, 1551227352);};};}|];};
                        {Value = Some 226.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.99000001f, 1551125605);
                                                   O2 = (1.88f, 1551125605);};
                                     Closing = X2 {O1 = (2.02999997f, 1551229121);
                                                   O2 = (1.88f, 1551229121);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.89999998f, 1551196382);
                                                   O2 = (1.89999998f, 1551196382);};
                                     Closing = X2 {O1 = (1.89999998f, 1551196382);
                                                   O2 = (1.89999998f, 1551196382);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.95000005f, 1551145074);
                                                   O2 = (1.88999999f, 1551145074);};
                                     Closing = X2 {O1 = (1.96000004f, 1551229123);
                                                   O2 = (1.88f, 1551229123);};};}|];};
                        {Value = Some 227.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.04999995f, 1551125605);
                                                   O2 = (1.83000004f, 1551125605);};
                                     Closing = X2 {O1 = (2.08999991f, 1551229121);
                                                   O2 = (1.83000004f, 1551229121);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.99000001f, 1551145074);
                                                   O2 = (1.84000003f, 1551145074);};
                                     Closing = X2 {O1 = (1.99000001f, 1551229123);
                                                   O2 = (1.83000004f, 1551229123);};};}|];};
                        {Value = Some 227.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.11999989f, 1551125605);
                                                   O2 = (1.77999997f, 1551125605);};
                                     Closing = X2 {O1 = (2.16000009f, 1551229121);
                                                   O2 = (1.77999997f, 1551229121);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.00999999f, 1551145074);
                                                   O2 = (1.79999995f, 1551145074);};
                                     Closing = X2 {O1 = (2.00999999f, 1551229123);
                                                   O2 = (1.79999995f, 1551229123);};};}|];};
                        {Value = Some 228.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.03999996f, 1551145074);
                                                   O2 = (1.75999999f, 1551145074);};
                                     Closing = X2 {O1 = (2.03999996f, 1551229123);
                                                   O2 = (1.75999999f, 1551229123);};};}|];};
                        {Value = Some 228.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.05999994f, 1551145074);
                                                   O2 = (1.73000002f, 1551145074);};
                                     Closing = X2 {O1 = (2.06999993f, 1551229123);
                                                   O2 = (1.72000003f, 1551229123);};};}|];};
                        {Value = Some 229.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.08999991f, 1551145074);
                                                   O2 = (1.69000006f, 1551145074);};
                                     Closing = X2 {O1 = (2.1099999f, 1551229123);
                                                   O2 = (1.67999995f, 1551229123);};};}|];};
                        {Value = Some 229.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.1099999f, 1551145074);
                                                   O2 = (1.65999997f, 1551145074);};
                                     Closing = X2 {O1 = (2.13000011f, 1551229123);
                                                   O2 = (1.64999998f, 1551229123);};};}|];};
                        {Value = Some 230.0f;
                         BookOdds = [|{Book = Mar;
                                       Odds = {Opening = X2 {O1 = (2.16000009f, 1551145074);
                                                             O2 = (1.62f, 1551145074);};
                                               Closing = X2 {O1 = (2.16000009f, 1551229123);
                                                             O2 = (1.62f, 1551229123);};};}|];}|];};
                    {Outcome = AH;
                     Values =
                      [|{Value = Some -7.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.3599999f, 1551145074);
                                                   O2 = (1.54999995f, 1551145074);};
                                     Closing = X2 {O1 = (2.47000003f, 1551228660);
                                                   O2 = (1.49000001f, 1551228660);};};}|];};
                        {Value = Some -7.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.31999993f, 1551145074);
                                                   O2 = (1.58000004f, 1551145074);};
                                     Closing = X2 {O1 = (2.42000008f, 1551228987);
                                                   O2 = (1.50999999f, 1551228987);};};}|];};
                        {Value = Some -6.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.23000002f, 1551145074);
                                                   O2 = (1.63999999f, 1551145074);};
                                     Closing = X2 {O1 = (2.30999994f, 1551228987);
                                                   O2 = (1.57000005f, 1551228987);};};}|];};
                        {Value = Some -6.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.17000008f, 1551145074);
                                                   O2 = (1.69000006f, 1551145074);};
                                     Closing = X2 {O1 = (2.27999997f, 1551228860);
                                                   O2 = (1.60000002f, 1551228987);};};}|];};
                        {Value = Some -5.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.0999999f, 1551145074);
                                                   O2 = (1.75f, 1551145074);};
                                     Closing = X2 {O1 = (2.19000006f, 1551228860);
                                                   O2 = (1.65999997f, 1551228987);};};}|];};
                        {Value = Some -5.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.1400001f, 1551125413);
                                                   O2 = (1.78999996f, 1551125413);};
                                     Closing = X2 {O1 = (2.29999995f, 1551228685);
                                                   O2 = (1.69000006f, 1551228685);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.03999996f, 1551145074);
                                                   O2 = (1.80999994f, 1551145074);};
                                     Closing = X2 {O1 = (2.1400001f, 1551228987);
                                                   O2 = (1.71000004f, 1551228987);};};}|];};
                        {Value = Some -4.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.03999996f, 1551125414);
                                                   O2 = (1.87f, 1551125414);};
                                     Closing = X2 {O1 = (2.18000007f, 1551228685);
                                                   O2 = (1.76999998f, 1551228685);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.98000002f, 1551145074);
                                                   O2 = (1.88f, 1551145074);};
                                     Closing = X2 {O1 = (2.06999993f, 1551228860);
                                                   O2 = (1.76999998f, 1551228987);};};}|];};
                        {Value = Some -4.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.95000005f, 1551125413);
                                                   O2 = (1.95000005f, 1551125413);};
                                     Closing = X2 {O1 = (2.08999991f, 1551228685);
                                                   O2 = (1.83000004f, 1551228685);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.92999995f, 1551145074);
                                                   O2 = (1.95000005f, 1551145074);};
                                     Closing = X2 {O1 = (2.01999998f, 1551228860);
                                                   O2 = (1.83000004f, 1551228987);};};}|];};
                        {Value = Some -3.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.88f, 1551125414);
                                                   O2 = (2.02999997f, 1551125414);};
                                     Closing = X2 {O1 = (1.99000001f, 1551228685);
                                                   O2 = (1.91999996f, 1551228685);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.89999998f, 1551203850);
                                                   O2 = (1.89999998f, 1551203850);};
                                     Closing = X2 {O1 = (1.95000005f, 1551228673);
                                                   O2 = (1.86000001f, 1551228673);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.86000001f, 1551145074);
                                                   O2 = (2.00999999f, 1551145074);};
                                     Closing = X2 {O1 = (1.96000004f, 1551228660);
                                                   O2 = (1.89999998f, 1551228987);};};}|];};
                        {Value = Some -3.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.78999996f, 1551125414);
                                                   O2 = (2.1400001f, 1551125414);};
                                     Closing = X2 {O1 = (1.90999997f, 1551228685);
                                                   O2 = (2.0f, 1551228685);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.78999996f, 1551145074);
                                                   O2 = (2.06999993f, 1551145074);};
                                     Closing = X2 {O1 = (1.91999996f, 1551228987);
                                                   O2 = (1.96000004f, 1551228987);};};}|];};
                        {Value = Some -2.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.74000001f, 1551125414);
                                                   O2 = (2.23000002f, 1551125414);};
                                     Closing = X2 {O1 = (1.84000003f, 1551228685);
                                                   O2 = (2.07999992f, 1551228685);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.73000002f, 1551145074);
                                                   O2 = (2.13000011f, 1551145074);};
                                     Closing = X2 {O1 = (1.85000002f, 1551228987);
                                                   O2 = (2.01999998f, 1551228660);};};}|];};
                        {Value = Some -2.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.73000002f, 1551204143);
                                                   O2 = (2.24000001f, 1551204143);};
                                     Closing = X2 {O1 = (1.75999999f, 1551228685);
                                                   O2 = (2.19000006f, 1551228685);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.65999997f, 1551145074);
                                                   O2 = (2.22000003f, 1551145074);};
                                     Closing = X2 {O1 = (1.76999998f, 1551228987);
                                                   O2 = (2.0999999f, 1551228987);};};}|];};
                        {Value = Some -1.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.61000001f, 1551145074);
                                                   O2 = (2.28999996f, 1551145074);};
                                     Closing = X2 {O1 = (1.72000003f, 1551228987);
                                                   O2 = (2.1500001f, 1551228860);};};}|];};
                        {Value = Some -1.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.57000005f, 1551145074);
                                                   O2 = (2.33999991f, 1551145074);};
                                     Closing = X2 {O1 = (1.65999997f, 1551228987);
                                                   O2 = (2.22000003f, 1551228987);};};}|];};
                        {Value = Some 1.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.46000004f, 1551145074);
                                                   O2 = (2.6099999f, 1551145074);};
                                     Closing = X2 {O1 = (1.54999995f, 1551229123);
                                                   O2 = (2.42000008f, 1551229123);};};}|];};
                        {Value = Some 1.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.44000006f, 1551145074);
                                                   O2 = (2.61999989f, 1551145074);};
                                     Closing = X2 {O1 = (1.51999998f, 1551228987);
                                                   O2 = (2.46000004f, 1551228987);};};}|];};
                        {Value = Some 2.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.44000006f, 1551201723);
                                                   O2 = (2.61999989f, 1551201723);};
                                     Closing = X2 {O1 = (1.46000004f, 1551228987);
                                                   O2 = (2.5999999f, 1551228987);};};}|];};
                        {Value = Some 2.5f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.42999995f, 1551228971);
                                                   O2 = (2.66000009f, 1551228971);};
                                     Closing = X2 {O1 = (1.42999995f, 1551228971);
                                                   O2 = (2.66000009f, 1551228971);};};}|];}|];}|];})
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapBasketballNBA1819League() =
        let leagueID, pageCount = ("C2416Q6r", 2)
        let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + basketballID + "/" + leagueID + "/X0/1/0/"
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
    [<Test>]
    member this.ScrapBasketballMatchWithOT() =
        let matchID = "ChPLtsAp"
        let matchUrl = "basketball/usa/nba/atlanta-hawks-minnesota-timberwolves-" + matchID + "/"
        let actual = extractMatchOdds [| Pin |] (basketballID, basketballDataID) [| HA; OU; AH |] (matchID, matchUrl)
        match actual with
        | Some({
                ID = "ChPLtsAp";
                Url = "http://www.oddsportal.com/basketball/usa/nba/atlanta-hawks-minnesota-timberwolves-ChPLtsAp/";
                TeamHome = "Atlanta Hawks";
                TeamAway = "Minnesota Timberwolves";
                Time = 1551313800;
                Score = { Home = 131; Away = 123 };
                ScoreWithoutOT = Some { Home = 118; Away = 118 };
                Periods = [|
                    { Home = 33; Away = 40 };
                    { Home = 27; Away = 28 };
                    { Home = 26; Away = 27 };
                    { Home = 32; Away = 23 };
                    { Home = 13; Away = 5 }
                |];
          }) -> ()
        | _ -> failwith "Incorrect data"
    [<Test>]
    member this.ScrapTennisAtpWimbledon19Match() =
        let matchID = "fyXBxdlb"
        let matchUrl = "tennis/united-kingdom/atp-wimbledon/djokovic-novak-federer-roger-" + matchID + "/"
        let actual = extractMatchOdds [| Pin; BF; B365; Mar |] (tennisID, tennisDataID) [| HA; OU; AH |] (matchID, matchUrl)
        let expected =
            Some({ID = "fyXBxdlb";
                 Url =
                  "http://www.oddsportal.com/tennis/united-kingdom/atp-wimbledon/djokovic-novak-federer-roger-fyXBxdlb/";
                 TeamHome = "Djokovic N.";
                 TeamAway = "Federer R.";
                 Time = 1563109800;
                 Score = {Home = 3;
                          Away = 2;};
                 ScoreWithoutOT = None;
                 Periods = [|{Home = 7;
                              Away = 6;}; {Home = 1;
                                           Away = 6;}; {Home = 7;
                                                        Away = 6;}; {Home = 4;
                                                                     Away = 6;}; {Home = 13;
                                                                                  Away = 12;}|];
                 Odds =
                  [|{Outcome = HA;
                     Values =
                      [|{Value = None;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.55999994f, 1562957689);
                                                   O2 = (2.5999999f, 1562957689);};
                                     Closing = X2 {O1 = (1.54999995f, 1563109775);
                                                   O2 = (2.66000009f, 1563109775);};};};
                            {Book = BF;
                             Odds = {Opening = X2 {O1 = (1.48000002f, 1562959145);
                                                   O2 = (2.70000005f, 1562959145);};
                                     Closing = X2 {O1 = (1.52999997f, 1563071579);
                                                   O2 = (2.63000011f, 1563109112);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.47000003f, 1562957276);
                                                   O2 = (2.75f, 1562957276);};
                                     Closing = X2 {O1 = (1.54999995f, 1563055499);
                                                   O2 = (2.5999999f, 1563055499);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.48000002f, 1562957742);
                                                   O2 = (2.9000001f, 1562957742);};
                                     Closing = X2 {O1 = (1.57000005f, 1563105435);
                                                   O2 = (2.6099999f, 1563105435);};};}|];}|];};
                    {Outcome = OU;
                     Values =
                      [|{Value = Some 38.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.71000004f, 1563053282);
                                                   O2 = (2.21000004f, 1563053282);};
                                     Closing = X2 {O1 = (1.74000001f, 1563109597);
                                                   O2 = (2.20000005f, 1563109597);};};}|];};
                        {Value = Some 39.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.70000005f, 1562974921);
                                                   O2 = (2.21000004f, 1562974921);};
                                     Closing = X2 {O1 = (1.77999997f, 1563109597);
                                                   O2 = (2.1400001f, 1563109597);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.72000003f, 1562967201);
                                                   O2 = (2.11999989f, 1562967201);};
                                     Closing = X2 {O1 = (1.72000003f, 1563086513);
                                                   O2 = (2.11999989f, 1563086513);};};}|];};
                        {Value = Some 39.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.76999998f, 1562974921);
                                                   O2 = (2.11999989f, 1562974921);};
                                     Closing = X2 {O1 = (1.85000002f, 1563109597);
                                                   O2 = (2.04999995f, 1563109597);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.83000004f, 1562967095);
                                                   O2 = (1.83000004f, 1562967095);};
                                     Closing = X2 {O1 = (1.83000004f, 1562967095);
                                                   O2 = (1.83000004f, 1562967095);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.76999998f, 1562967201);
                                                   O2 = (2.06999993f, 1562967201);};
                                     Closing = X2 {O1 = (1.77999997f, 1563086513);
                                                   O2 = (2.05999994f, 1563086513);};};}|];};
                        {Value = Some 40.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.80999994f, 1562958346);
                                                   O2 = (2.06999993f, 1562958346);};
                                     Closing = X2 {O1 = (1.90999997f, 1563109597);
                                                   O2 = (1.99000001f, 1563109597);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.95000005f, 1562957742);
                                                   O2 = (1.95000005f, 1562957742);};
                                     Closing = X2 {O1 = (1.83000004f, 1563086513);
                                                   O2 = (2.01999998f, 1563086513);};};}|];};
                        {Value = Some 40.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.87f, 1562958346);
                                                   O2 = (2.00999999f, 1562958346);};
                                     Closing = X2 {O1 = (1.97000003f, 1563109597);
                                                   O2 = (1.92999995f, 1563109597);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.75999999f, 1562957828);
                                                   O2 = (2.08999991f, 1562957828);};
                                     Closing = X2 {O1 = (1.88999999f, 1563087179);
                                                   O2 = (1.97000003f, 1563087179);};};}|];};
                        {Value = Some 41.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.94000006f, 1562958346);
                                                   O2 = (1.94000006f, 1562958346);};
                                     Closing = X2 {O1 = (2.04999995f, 1563109597);
                                                   O2 = (1.85000002f, 1563109597);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.80999994f, 1562957828);
                                                   O2 = (2.04999995f, 1562957828);};
                                     Closing = X2 {O1 = (1.96000004f, 1563087179);
                                                   O2 = (1.94000006f, 1563087179);};};}|];};
                        {Value = Some 41.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.99000001f, 1562958346);
                                                   O2 = (1.88999999f, 1562958346);};
                                     Closing = X2 {O1 = (2.11999989f, 1563109597);
                                                   O2 = (1.78999996f, 1563109597);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.87f, 1562957828);
                                                   O2 = (2.0f, 1562957828);};
                                     Closing = X2 {O1 = (2.00999999f, 1563087179);
                                                   O2 = (1.86000001f, 1563087179);};};}|];};
                        {Value = Some 42.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.07999992f, 1562958346);
                                                   O2 = (1.80999994f, 1562958346);};
                                     Closing = X2 {O1 = (2.21000004f, 1563109597);
                                                   O2 = (1.73000002f, 1563109597);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.94000006f, 1562957828);
                                                   O2 = (1.96000004f, 1562957828);};
                                     Closing = X2 {O1 = (2.06999993f, 1563087179);
                                                   O2 = (1.78999996f, 1563087179);};};}|];};
                        {Value = Some 42.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.16000009f, 1562974921);
                                                   O2 = (1.74000001f, 1562974921);};
                                     Closing = X2 {O1 = (2.28999996f, 1563109597);
                                                   O2 = (1.67999995f, 1563109597);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.96000004f, 1562957828);
                                                   O2 = (1.89999998f, 1562957828);};
                                     Closing = X2 {O1 = (2.11999989f, 1563087179);
                                                   O2 = (1.74000001f, 1563087179);};};}|];};
                        {Value = Some 43.0f;
                         BookOdds =
                          [|{Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.01999998f, 1562957828);
                                                   O2 = (1.83000004f, 1562957828);};
                                     Closing = X2 {O1 = (2.19000006f, 1563087179);
                                                   O2 = (1.67999995f, 1563087179);};};}|];}|];};
                    {Outcome = AH;
                     Values =
                      [|{Value = Some -5.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.72000003f, 1563094089);
                                                   O2 = (1.48000002f, 1563094089);};
                                     Closing = X2 {O1 = (2.48000002f, 1563109775);
                                                   O2 = (1.57000005f, 1563109597);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.88000011f, 1563076410);
                                                   O2 = (1.41999996f, 1563076410);};
                                     Closing = X2 {O1 = (2.45000005f, 1563104351);
                                                   O2 = (1.54999995f, 1563104351);};};}|];};
                        {Value = Some -5.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (3.13000011f, 1562974921);
                                                   O2 = (1.35000002f, 1562974921);};
                                     Closing = X2 {O1 = (2.29999995f, 1563109714);
                                                   O2 = (1.66999996f, 1563109714);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (3.1500001f, 1562957828);
                                                   O2 = (1.36000001f, 1562957828);};
                                     Closing = X2 {O1 = (2.25f, 1563104351);
                                                   O2 = (1.65999997f, 1563104351);};};}|];};
                        {Value = Some -4.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.63000011f, 1562974921);
                                                   O2 = (1.49000001f, 1562974921);};
                                     Closing = X2 {O1 = (2.19000006f, 1563109714);
                                                   O2 = (1.74000001f, 1563109714);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.56999993f, 1562957828);
                                                   O2 = (1.51999998f, 1562957828);};
                                     Closing = X2 {O1 = (2.0999999f, 1563104351);
                                                   O2 = (1.76999998f, 1563104351);};};}|];};
                        {Value = Some -4.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.27999997f, 1562958404);
                                                   O2 = (1.65999997f, 1562958404);};
                                     Closing = X2 {O1 = (2.00999999f, 1563109775);
                                                   O2 = (1.88999999f, 1563109775);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (2.3599999f, 1562957828);
                                                   O2 = (1.62f, 1562957828);};
                                     Closing = X2 {O1 = (2.01999998f, 1563104351);
                                                   O2 = (1.85000002f, 1563104351);};};}|];};
                        {Value = Some -3.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (2.04999995f, 1562958404);
                                                   O2 = (1.83000004f, 1562958404);};
                                     Closing = X2 {O1 = (1.87f, 1563109775);
                                                   O2 = (2.02999997f, 1563109775);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.91999996f, 1562957742);
                                                   O2 = (1.98000002f, 1562957742);};
                                     Closing = X2 {O1 = (1.90999997f, 1563104351);
                                                   O2 = (1.99000001f, 1563104351);};};}|];};
                        {Value = Some -3.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.95000005f, 1562958404);
                                                   O2 = (1.92999995f, 1562958404);};
                                     Closing = X2 {O1 = (1.75999999f, 1563109658);
                                                   O2 = (2.16000009f, 1563109775);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.95000005f, 1562957828);
                                                   O2 = (1.95000005f, 1562957828);};
                                     Closing = X2 {O1 = (1.75f, 1563104351);
                                                   O2 = (2.1500001f, 1563104351);};};}|];};
                        {Value = Some -2.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.82000005f, 1562958404);
                                                   O2 = (2.05999994f, 1562958404);};
                                     Closing = X2 {O1 = (1.69000006f, 1563109658);
                                                   O2 = (2.25999999f, 1563109775);};};};
                            {Book = B365;
                             Odds = {Opening = X2 {O1 = (1.79999995f, 1562967095);
                                                   O2 = (1.89999998f, 1562967095);};
                                     Closing = X2 {O1 = (1.79999995f, 1562967095);
                                                   O2 = (1.89999998f, 1562967095);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.79999995f, 1562957828);
                                                   O2 = (2.07999992f, 1562957828);};
                                     Closing = X2 {O1 = (1.70000005f, 1563104351);
                                                   O2 = (2.21000004f, 1563104351);};};}|];};
                        {Value = Some -2.0f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.72000003f, 1562958404);
                                                   O2 = (2.18000007f, 1562958404);};
                                     Closing = X2 {O1 = (1.63999999f, 1563109775);
                                                   O2 = (2.3599999f, 1563109658);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.69000006f, 1562957828);
                                                   O2 = (2.22000003f, 1562957828);};
                                     Closing = X2 {O1 = (1.63f, 1563104351);
                                                   O2 = (2.30999994f, 1563104351);};};}|];};
                        {Value = Some -1.5f;
                         BookOdds =
                          [|{Book = Pin;
                             Odds = {Opening = X2 {O1 = (1.70000005f, 1562974921);
                                                   O2 = (2.21000004f, 1562974921);};
                                     Closing = X2 {O1 = (1.60000002f, 1563109597);
                                                   O2 = (2.44000006f, 1563109775);};};};
                            {Book = Mar;
                             Odds = {Opening = X2 {O1 = (1.62f, 1562957828);
                                                   O2 = (2.32999992f, 1562957828);};
                                     Closing = X2 {O1 = (1.59000003f, 1563104351);
                                                   O2 = (2.3599999f, 1563104351);};};}|];}|];}|];})
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapTennisAtpWimbeldon19League() =
        let leagueID, pageCount = ("Ieiv94gB", 2)
        let leagueRelativeUrl = "/ajax-sport-country-tournament-archive/" + tennisID + "/" + leagueID + "/X0/1/0/"
        let actual =
            [1..pageCount]
            |> List.map (fun pageNum -> fetchLeagueMatches leagueRelativeUrl pageNum)
            |> List.concat
        let expected =
            [("fyXBxdlb", "/tennis/united-kingdom/atp-wimbledon/djokovic-novak-federer-roger-fyXBxdlb/");
            ("lhqi1P31", "/tennis/united-kingdom/atp-wimbledon/nadal-rafael-federer-roger-lhqi1P31/");
            ("WQR97Wkf", "/tennis/united-kingdom/atp-wimbledon/djokovic-novak-bautista-agut-roberto-WQR97Wkf/");
            ("MoZL0bZO", "/tennis/united-kingdom/atp-wimbledon/querrey-sam-nadal-rafael-MoZL0bZO/");
            ("8nPvK4uo", "/tennis/united-kingdom/atp-wimbledon/nishikori-kei-federer-roger-8nPvK4uo/");
            ("63QkVFVe", "/tennis/united-kingdom/atp-wimbledon/djokovic-novak-goffin-david-63QkVFVe/");
            ("tjQrQcOu", "/tennis/united-kingdom/atp-wimbledon/pella-guido-bautista-agut-roberto-tjQrQcOu/");
            ("txeUhhnC", "/tennis/united-kingdom/atp-wimbledon/berrettini-matteo-federer-roger-txeUhhnC/");
            ("tOFevgXI", "/tennis/united-kingdom/atp-wimbledon/nishikori-kei-kukushkin-mikhail-tOFevgXI/");
            ("rwBl5NEa", "/tennis/united-kingdom/atp-wimbledon/pella-guido-raonic-milos-rwBl5NEa/");
            ("Eyj9kfxR", "/tennis/united-kingdom/atp-wimbledon/djokovic-novak-humbert-ugo-Eyj9kfxR/");
            ("ObRv89ZP", "/tennis/united-kingdom/atp-wimbledon/querrey-sam-sandgren-tennys-ObRv89ZP/");
            ("23JK1TjA", "/tennis/united-kingdom/atp-wimbledon/bautista-agut-roberto-paire-benoit-23JK1TjA/");
            ("OjBEcxCc", "/tennis/united-kingdom/atp-wimbledon/goffin-david-verdasco-fernando-OjBEcxCc/");
            ("6iYxpTwm", "/tennis/united-kingdom/atp-wimbledon/sousa-joao-nadal-rafael-6iYxpTwm/");
            ("v7B9XBIB", "/tennis/united-kingdom/atp-wimbledon/sousa-joao-evans-daniel-v7B9XBIB/");
            ("4lFhdE7P", "/tennis/united-kingdom/atp-wimbledon/pouille-lucas-federer-roger-4lFhdE7P/");
            ("Sj8XHCqh", "/tennis/united-kingdom/atp-wimbledon/tsonga-jo-wilfried-nadal-rafael-Sj8XHCqh/");
            ("AFcxHWbb", "/tennis/united-kingdom/atp-wimbledon/berrettini-matteo-schwartzman-diego-sebastian-AFcxHWbb/");
            ("vsSIblHA", "/tennis/united-kingdom/atp-wimbledon/struff-jan-lennard-kukushkin-mikhail-vsSIblHA/");
            ("OI1tGjE4", "/tennis/united-kingdom/atp-wimbledon/sandgren-tennys-fognini-fabio-OI1tGjE4/");
            ("zH3QOTfE", "/tennis/united-kingdom/atp-wimbledon/nishikori-kei-johnson-steve-zH3QOTfE/");
            ("h04h3B7m", "/tennis/united-kingdom/atp-wimbledon/querrey-sam-millman-john-h04h3B7m/");
            ("U7AnZvUP", "/tennis/united-kingdom/atp-wimbledon/auger-aliassime-felix-humbert-ugo-U7AnZvUP/");
            ("MPo53Mwr", "/tennis/united-kingdom/atp-wimbledon/verdasco-fernando-fabbiano-thomas-MPo53Mwr/");
            ("z1HAfnd3", "/tennis/united-kingdom/atp-wimbledon/djokovic-novak-hurkacz-hubert-z1HAfnd3/");
            ("Snv6JJBk", "/tennis/united-kingdom/atp-wimbledon/medvedev-daniil-goffin-david-Snv6JJBk/");
            ("zR01T9c4", "/tennis/united-kingdom/atp-wimbledon/anderson-kevin-pella-guido-zR01T9c4/");
            ("MB6WlNOl", "/tennis/united-kingdom/atp-wimbledon/khachanov-karen-bautista-agut-roberto-MB6WlNOl/");
            ("j72WVm1C", "/tennis/united-kingdom/atp-wimbledon/opelka-reilly-raonic-milos-j72WVm1C/");
            ("vgKgEYro", "/tennis/united-kingdom/atp-wimbledon/paire-benoit-vesely-jiri-vgKgEYro/");
            ("WYRkGkXp", "/tennis/united-kingdom/atp-wimbledon/koepfer-dominik-schwartzman-diego-sebastian-WYRkGkXp/");
            ("EZMAABst", "/tennis/united-kingdom/atp-wimbledon/berrettini-matteo-baghdatis-marcos-EZMAABst/");
            ("Q96ST81p", "/tennis/united-kingdom/atp-wimbledon/kyrgios-nick-nadal-rafael-Q96ST81p/");
            ("EV2QFIg1", "/tennis/united-kingdom/atp-wimbledon/fucsovics-marton-fognini-fabio-EV2QFIg1/");
            ("EeE9JRs8", "/tennis/united-kingdom/atp-wimbledon/simon-gilles-sandgren-tennys-EeE9JRs8/");
            ("hplrGRmp", "/tennis/united-kingdom/atp-wimbledon/pouille-lucas-barrere-gregoire-hplrGRmp/");
            ("bqGLGPRQ", "/tennis/united-kingdom/atp-wimbledon/clarke-jay-federer-roger-bqGLGPRQ/");
            ("lUClyi9A", "/tennis/united-kingdom/atp-wimbledon/berankis-ricardas-tsonga-jo-wilfried-lUClyi9A/");
            ("fRuDwMo8", "/tennis/united-kingdom/atp-wimbledon/cilic-marin-sousa-joao-fRuDwMo8/");
            ("8CCBLmQM", "/tennis/united-kingdom/atp-wimbledon/kukushkin-mikhail-isner-john-8CCBLmQM/");
            ("A1GJ8kCh", "/tennis/united-kingdom/atp-wimbledon/nishikori-kei-norrie-cameron-A1GJ8kCh/");
            ("WvLd97pD", "/tennis/united-kingdom/atp-wimbledon/evans-daniel-basilashvili-nikoloz-WvLd97pD/");
            ("n9l97LlD", "/tennis/united-kingdom/atp-wimbledon/struff-jan-lennard-fritz-taylor-harry-n9l97LlD/");
            ("d2jYAMQ1", "/tennis/united-kingdom/atp-wimbledon/johnson-steve-de-minaur-alex-d2jYAMQ1/");
            ("KflNGbve", "/tennis/united-kingdom/atp-wimbledon/millman-john-djere-laslo-KflNGbve/");
            ("Aa5tKwHL", "/tennis/united-kingdom/atp-wimbledon/querrey-sam-rublev-andrey-Aa5tKwHL/");
            ("2J7Sp1Mk", "/tennis/united-kingdom/atp-wimbledon/djokovic-novak-kudla-denis-2J7Sp1Mk/");
            ("jq4Oos7q", "/tennis/united-kingdom/atp-wimbledon/anderson-kevin-tipsarevic-janko-jq4Oos7q/");
            ("0fJ6FbyA", "/tennis/united-kingdom/atp-wimbledon/auger-aliassime-felix-moutet-corentin-0fJ6FbyA/");
            ("E7rj7zqj", "/tennis/united-kingdom/atp-wimbledon/granollers-pujol-marcel-humbert-ugo-E7rj7zqj/");
            ("AZDpGG9U", "/tennis/united-kingdom/atp-wimbledon/seppi-andreas-pella-guido-AZDpGG9U/");
            ("GOSqG7wK", "/tennis/united-kingdom/atp-wimbledon/khachanov-karen-lopez-feliciano-GOSqG7wK/");
            ("hObkCFqF", "/tennis/united-kingdom/atp-wimbledon/darcis-steve-bautista-agut-roberto-hObkCFqF/");
            ("niF2GvM3", "/tennis/united-kingdom/atp-wimbledon/edmund-kyle-verdasco-fernando-niF2GvM3/");
            ("Uws7XHv5", "/tennis/united-kingdom/atp-wimbledon/haase-robin-raonic-milos-Uws7XHv5/");
            ("rJ8BEIjG", "/tennis/united-kingdom/atp-wimbledon/chardy-jeremy-goffin-david-rJ8BEIjG/");
            ("8UuBWyfB", "/tennis/united-kingdom/atp-wimbledon/mayer-leonardo-hurkacz-hubert-8UuBWyfB/");
            ("0bgfzKfn", "/tennis/united-kingdom/atp-wimbledon/karlovic-ivo-fabbiano-thomas-0bgfzKfn/");
            ("MDs3YcPb", "/tennis/united-kingdom/atp-wimbledon/medvedev-daniil-popyrin-alexei-MDs3YcPb/");
            ("nekjy0ut", "/tennis/united-kingdom/atp-wimbledon/cuevas-pablo-vesely-jiri-nekjy0ut/");
            ("YJPnD5wp", "/tennis/united-kingdom/atp-wimbledon/wawrinka-stan-opelka-reilly-YJPnD5wp/");
            ("zFxDIOmJ", "/tennis/united-kingdom/atp-wimbledon/paire-benoit-kecmanovic-miomir-zFxDIOmJ/");
            ("4SV1LoVc", "/tennis/united-kingdom/atp-wimbledon/pouille-lucas-gasquet-richard-4SV1LoVc/");
            ("APCv4p0N", "/tennis/united-kingdom/atp-wimbledon/ruud-casper-isner-john-APCv4p0N/");
            ("4jIqDg2c", "/tennis/united-kingdom/atp-wimbledon/sousa-joao-jubb-paul-4jIqDg2c/");
            ("QaJuEZni", "/tennis/united-kingdom/atp-wimbledon/cilic-marin-mannarino-adrian-QaJuEZni/");
            ("bgRgjWfp", "/tennis/united-kingdom/atp-wimbledon/fucsovics-marton-novak-dennis-bgRgjWfp/");
            ("6ahoYINN", "/tennis/united-kingdom/atp-wimbledon/andreozzi-guido-djere-laslo-6ahoYINN/");
            ("IVZ5K5p4", "/tennis/united-kingdom/atp-wimbledon/bublik-alexander-barrere-gregoire-IVZ5K5p4/");
            ("bk9z5QpH", "/tennis/united-kingdom/atp-wimbledon/andujar-alba-pablo-kukushkin-mikhail-bk9z5QpH/");
            ("xpQckjAj", "/tennis/united-kingdom/atp-wimbledon/tiafoe-frances-fognini-fabio-xpQckjAj/");
            ("hdlsZb8H", "/tennis/united-kingdom/atp-wimbledon/dellien-hugo-millman-john-hdlsZb8H/");
            ("GjDP3kPq", "/tennis/united-kingdom/atp-wimbledon/sugita-yuichi-nadal-rafael-GjDP3kPq/");
            ("fuBdAinG", "/tennis/united-kingdom/atp-wimbledon/shapovalov-denis-berankis-ricardas-fuBdAinG/");
            ("M3tcMRFi", "/tennis/united-kingdom/atp-wimbledon/ebden-matthew-schwartzman-diego-sebastian-M3tcMRFi/");
            ("4S9W56VA", "/tennis/united-kingdom/atp-wimbledon/fritz-taylor-harry-berdych-tomas-4S9W56VA/");
            ("fclBbONp", "/tennis/united-kingdom/atp-wimbledon/struff-jan-lennard-albot-radu-fclBbONp/");
            ("YV909B1M", "/tennis/united-kingdom/atp-wimbledon/tomic-bernard-tsonga-jo-wilfried-YV909B1M/");
            ("IeDP7bQH", "/tennis/united-kingdom/atp-wimbledon/uchiyama-yasutaka-sandgren-tennys-IeDP7bQH/");
            ("jqYDIqFG", "/tennis/united-kingdom/atp-wimbledon/harris-lloyd-george-federer-roger-jqYDIqFG/");
            ("ChZ9JPaA", "/tennis/united-kingdom/atp-wimbledon/clarke-jay-rubin-noah-ChZ9JPaA/");
            ("rDsgN70o", "/tennis/united-kingdom/atp-wimbledon/koepfer-dominik-krajinovic-filip-rDsgN70o/");
            ("bZlwzugB", "/tennis/united-kingdom/atp-wimbledon/garin-christian-rublev-andrey-bZlwzugB/");
            ("OGGX1Tgd", "/tennis/united-kingdom/atp-wimbledon/istomin-denis-norrie-cameron-OGGX1Tgd/");
            ("KAfM8vBB", "/tennis/united-kingdom/atp-wimbledon/simon-gilles-caruso-salvatore-KAfM8vBB/");
            ("KrjZzLw5", "/tennis/united-kingdom/atp-wimbledon/thiem-dominic-querrey-sam-KrjZzLw5/");
            ("QyBS6nG4", "/tennis/united-kingdom/atp-wimbledon/berrettini-matteo-bedene-aljaz-QyBS6nG4/");
            ("hl2q34FT", "/tennis/united-kingdom/atp-wimbledon/schnur-brayden-baghdatis-marcos-hl2q34FT/");
            ("EsRc8VGS", "/tennis/united-kingdom/atp-wimbledon/kyrgios-nick-thompson-jordan-EsRc8VGS/");
            ("4CgKPWo3", "/tennis/united-kingdom/atp-wimbledon/cecchinato-marco-de-minaur-alex-4CgKPWo3/");
            ("nF7lCDH3", "/tennis/united-kingdom/atp-wimbledon/delbonis-federico-evans-daniel-nF7lCDH3/");
            ("QLfGQCWd", "/tennis/united-kingdom/atp-wimbledon/johnson-steve-ramos-vinolas-albert-QLfGQCWd/");
            ("ADCT29vj", "/tennis/united-kingdom/atp-wimbledon/nishikori-kei-monteiro-thiago-ADCT29vj/");
            ("0IBhBXW9", "/tennis/united-kingdom/atp-wimbledon/ward-james-basilashvili-nikoloz-0IBhBXW9/");
            ("6JzUwNgn", "/tennis/united-kingdom/atp-wimbledon/carballes-baena-roberto-kecmanovic-miomir-6JzUwNgn/");
            ("C4gkX2Jm", "/tennis/united-kingdom/atp-wimbledon/klahn-bradley-goffin-david-C4gkX2Jm/");
            ("hIgFPHGo", "/tennis/united-kingdom/atp-wimbledon/edmund-kyle-munar-jaume-hIgFPHGo/");
            ("ChVRSmS1", "/tennis/united-kingdom/atp-wimbledon/sonego-lorenzo-granollers-pujol-marcel-ChVRSmS1/");
            ("Y1WNTTCe", "/tennis/united-kingdom/atp-wimbledon/dimitrov-grigor-moutet-corentin-Y1WNTTCe/")]
        Assert.That(actual, Is.EqualTo(expected))
    [<Test>]
    member this.ScrapTennisAtpWimbledonCrossedOutOdds() =
        let matchID = "fyXBxdlb"
        let matchUrl = "tennis/united-kingdom/atp-wimbledon/djokovic-novak-federer-roger-" + matchID + "/"
        let actual = extractMatchOdds [| Pin; BF; B365; Mar |] (tennisID, tennisDataID) [| HA; OU; AH |] (matchID, matchUrl)
        match actual with
        | Some({ ID = "fyXBxdlb"; Odds = odds }) ->
            let odd = odds |> Array.find (fun odd -> odd.Outcome = OU)
            let value = odd.Values |> Array.find(fun value -> value.Value = Some 43.0f)
            let pinExist = value.BookOdds |> Array.exists (fun book -> book.Book = Pin)
            let marExist = value.BookOdds |> Array.exists (fun book -> book.Book = Mar)
            Assert.That(pinExist, Is.False)
            Assert.That(marExist, Is.True)
        | _ -> failwith "Incorrect data"


