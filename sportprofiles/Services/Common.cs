using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using sportprofiles.Models;

namespace sportprofiles.Services
{
    public class Commons : ICommons
    {
        public Commons()
        {
        }

        /// <summary>
        /// Gets the recent news.
        /// </summary>
        /// <returns>The recent news.</returns>
        public Task<List<RecentNewsModel>> GetRecentNews()
        {
            return GetNews();
        }

        async Task<List<RecentNewsModel>> GetNews()
        {
            //simulate an async operation (e.g. data fetch from a DB or API)
            await Task.Delay(1000); //simulate a delay

            //Create and populate the list with data
            List<RecentNewsModel> lst = new List<RecentNewsModel>
            {
                new() {
                    ImageUrl = "nba.jpg",
                    HeaderText = "What you need to know about Giannis Antetokounmpo next contract",
                    PostingDate = Convert.ToDateTime("09/21/2023"),
                    TextField = "Milwaukee Bucks star Giannis Antetokounmpo becomes eligible to sign a contract extension on Friday, his first chance to recommit to the franchise since he signed a supermax extension before the 2020-21 season.",
                    NavigateUrl = "https://www.espn.com/nba/story/_/id/38439222/giannis-antetokounmpo-milwaukee-bucks-contract-extension-questions",
                    Id = 1
                },
                new() {
                    ImageUrl = "baseball.jpg",
                    HeaderText = "Which MLB playoff contenders can win the 2023 World Series?",
                    PostingDate = Convert.ToDateTime("09/11/2023"),
                    TextField = "The stretch run is here, and the MLB playoff field is slowly narrowing (good night, Red Sox). The National League wild-card race remains a free for all for the final spot, and the American League West and wild-card races are suddenly dealing with dueling slumps from the Rangers and Mariners -- but at least one of them will make it in.",
                    NavigateUrl = "https://www.espn.com/mlb/insider/story/_/id/38352480/mlb-playoff-contenders-win-2023-world-series",
                    Id = 2
                },
                new() {
                    ImageUrl = "nfl.jpg",
                    HeaderText = "Jonathan Taylor trade: Five offers for Colts, best team fits",
                    PostingDate = Convert.ToDateTime("09/21/2023"),
                    TextField = "The clock is ticking on a return to the field -- and a potential trade -- for Indianapolis Colts running back Jonathan Taylor, who is on the physically unable to perform list and is eligible to play in Week 5.",
                    NavigateUrl = "https://www.espn.com/nfl/insider/insider/story/_/id/38440074/jonathan-taylor-trade-offers-best-team-fits-draft-picks-2023",
                    Id = 3
                },
                new() {
                    ImageUrl = "nhl.jpg",
                    HeaderText = "How many goals for Bedard? Who wins the Vezina? Bold predictions for all 32 NHL teams",
                    PostingDate = Convert.ToDateTime("09/21/2023"),
                    TextField = "With opening night approaching on Oct. 10, we take big swings on what will transpire for each team in the 2023-24 season.",
                    NavigateUrl = "https://www.espn.com/nhl/insider/story/_/id/38445948/nhl-predictions-2023-24-stanley-cup-playoffs-awards-goals",
                    Id = 4
                },
                new() {
                    ImageUrl = "soccer.jpg",
                    HeaderText = "Messi, Haaland, Mbappe lead FIFA The Best list",
                    PostingDate = Convert.ToDateTime("09/21/2023"),
                    TextField = "Lionel Messi, Erling Haaland and Kylian Mbappé lead the 12-player shortlist for FIFA's The Best Men's Player nominees, with England midfielder Declan Rice also nominated after captaining former club West Ham United to Europa Conference League success.",
                    NavigateUrl = "https://www.espn.com/soccer/story/_/id/38398333/messi-haaland-mbappe-lead-fifa-best-award-shortlist",
                    Id = 5
                },
                new() {
                    ImageUrl = "college.png",
                    HeaderText = "Create a path to the College Football Playoff for top contenders",
                    PostingDate = Convert.ToDateTime("09/19/2023"),
                    TextField = "The Allstate Playoff Predictor breaks down which teams have the best chance to make this season's College Football Playoff.",
                    NavigateUrl = "https://www.espn.com/espn/feature/story/_/page/cfbplayoffpredictor/cfb-playoff-predictor",
                    Id = 6
                }

            };

            return lst;
        }

    }

    public interface ICommons
    {
        Task<List<RecentNewsModel>> GetRecentNews();
    }
}
