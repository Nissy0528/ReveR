using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class PlayerPrefsUtility
{

    /// <summary>
    /// リストを保存
    /// </summary>
    public static void SavaList<T>(string key, List<T> value)
    {
        string serizlizedList = Serialize<List<T>>(value);
        PlayerPrefs.SetString(key, serizlizedList);
    }

    /// <summary>
    /// リストの読み込み
    /// </summary>
    public static List<T> LoadList<T>(string key)
    {
        //keyがあるときだけ読み込む
        if (PlayerPrefs.HasKey(key))
        {
            string serizlizedList = PlayerPrefs.GetString(key);
            return Deseralize<List<T>>(serizlizedList);
        }

        return new List<T>();
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    private static string Serialize<T>(T obj)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, obj);
        return Convert.ToBase64String(memoryStream.GetBuffer());
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    private static T Deseralize<T>(string str)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
        return (T)binaryFormatter.Deserialize(memoryStream);
    }
}
