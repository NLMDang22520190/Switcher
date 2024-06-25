public class MusicScoreData
{
    public int highestScore;
    public int highestCombo;
    public bool isFullCombo;

    public MusicScoreData(int score, int combo, bool fullCombo)
    {
        highestScore = score;
        highestCombo = combo;
        isFullCombo = fullCombo;
    }
}
