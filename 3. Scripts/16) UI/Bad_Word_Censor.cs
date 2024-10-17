using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Bad_Word_Censor
{
    private static HashSet<string> bad_words;
    private static bool isInitialized = false;

    public static void Initialize()
    {
        if (isInitialized)
        {
            Debug.LogWarning("Bad_Word_Censor is already initialized.");
            return;
        }

        TextAsset file = Resources.Load<TextAsset>("BadWord");
        bad_words = new HashSet<string>();

        if (file != null)
        {
            string[] words = file.text.Split('\n');

            foreach (string word in words)
            {
                bad_words.Add(word.Trim());
            }

            isInitialized = true;
        }
        else
        {
            Debug.LogError("Bad words text file is null.");
        }
    }

    public static bool Contains_Bad_Word(string censor_text)
    {
        if (!isInitialized)
        {
            Debug.LogError("Bad_Word_Censor is not initialized. Please call Initialize() with a valid TextAsset.");
            return false;
        }

        string removed_special_symbol = Remove_Special_Symbol(censor_text);

        foreach (var bad_word in bad_words)
        {
            if (removed_special_symbol.Contains(bad_word))
            {
                return true;
            }
        }

        return false;
    }

    private static string Remove_Special_Symbol(string text)
    {
        return Regex.Replace(text, @"[^a-zA-Z°¡-ÆR-¤¡-¤¾]", "");
    }
}