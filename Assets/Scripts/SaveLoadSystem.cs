using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    public static void SaveScore(Score score)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.fun";

        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
        fileStream.Position = 0;
        PlayerScore bestScore = new PlayerScore(score);

        binaryFormatter.Serialize(fileStream, bestScore);
        fileStream.Close();
    }

    public static PlayerScore LoadScore()
    {
        string path = Application.persistentDataPath + "/Player.fun";
        if (File.Exists(path))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            fileStream.Position = 0;

            PlayerScore bestScore = binaryFormatter.Deserialize(fileStream) as PlayerScore;
            fileStream.Close();
            return bestScore;           
        }
        else
        {
            return null;
        }
    }
}
