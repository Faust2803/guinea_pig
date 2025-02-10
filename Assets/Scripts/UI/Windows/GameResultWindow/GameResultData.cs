namespace UI.Windows.GameResultWindow
{
    public class GameResultData : WindowData
    {

        public bool IsWin;
        public int Score;

        public GameResultData (bool win, int score)
        {
            IsWin = win;
            Score = score;
        }
    }
}