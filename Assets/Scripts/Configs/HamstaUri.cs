
namespace Configs
{
    public static class HamstaUri
    {
        private static string host = "https://back-hamsta.dev.gamemob.tech";

        public static string Authorization = host + "/api/usermini/auth";
        public static string UpdatePlayer = host + "";
        public static string Balances = host + "/api/usermini/balances";
        public static string Piramid = host + "/api/usermini/pyramid";
        public static string Collection = host + "/api/usermini/collection";
        public static string Equip = host + "/api/usermini/equip";
        public static string Market = host + "/api/usermini/market";
        public static string MarketBuy = host + "/api/usermini/market/buy";
        public static string StartGame = host + "/api/usermini/game/start";
        public static string EndGame = host + "/api/usermini/game/finish";
        public static string CancelGame = host + "/api/usermini/game/cancel";
        public static string GetStatistic = host + "/api/usermini/get-statistics";
        public static string LostUserHP = host + "/api/usermini/game/lost-hp";
        public static string AddUserHP = host + "/api/usermini/recovery";
        public static string MoonInfo = host + "/api/usermini/game/moon";
        public static string Leaderboard = host + "/api/usermini/game/leaderboard";
        public static string AchievementGetList = host + "/api/usermini/achivement";
        public static string AchievementTaskCompleat = host + "/api/usermini/achivement/progress";
        public static string AchievementCompleat = host + "/api/usermini/achivement/rewards";
        public static string SendAnalytics = host + "/api/usermini/amplitude";
        public static string UserRename = host + "/api/usermini/user/nickname";
        public static string UserReset = host + "/api/usermini/user/reset";
        public static string Session = host + "/api/usermini/session";

        public static string FakeAddUserPoints = host + "/api/usermini/add_points";
    }
    
    public enum LeaderboardOrder
    {
        collected_beans,
        total_games,
        lost_hp,
        score,
        moon_reached
    }
}