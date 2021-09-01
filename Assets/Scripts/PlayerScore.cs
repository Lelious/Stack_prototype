using System;

[Serializable]
public class PlayerScore
{
    public int bestScore;

    public PlayerScore(Score score)
    {
        bestScore = score.bestScore;
    }
}
