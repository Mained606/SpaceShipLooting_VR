using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoad
{
    private static string fileName = "/playerData.dat";

    // 데이터 저장
    public static void SaveData(PlayerStatsData data)
    {
        string path = Application.persistentDataPath + fileName;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = new FileStream(path, FileMode.Create);
        formatter.Serialize(file, data);
        file.Close();
    }

    // 데이터 불러오기
    public static PlayerStatsData LoadData()
    {
        string path = Application.persistentDataPath + fileName;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = new FileStream(path, FileMode.Open);
            PlayerStatsData data = formatter.Deserialize(file) as PlayerStatsData;
            file.Close();
            return data;
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다.");
            return null;
        }
    }

    // 저장된 파일 삭제
    public static void DeleteFile()
    {
        string path = Application.persistentDataPath + fileName;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}