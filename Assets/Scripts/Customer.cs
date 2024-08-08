using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;

public enum CustomerPersonality
{
    Grumpy,
    Polite,
    Prideful,
    Outgoing
}

[Serializable]
public class Customer
{
    public string name;
    public Texture2D picture;
    public string occupation;
    public CustomerPersonality personality;

    /*
    https://stackoverflow.com/questions/26043161/how-to-reference-a-texture-with-a-string
    CS0619: Resources.LoadAssetAtPath is obsolete => AssetDatabase.LoadAssetAtPath
     */
    static string texture = "Assets/Art/Characters/Default.png";
    Texture2D defaultTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texture);

    public Customer(string name, string occupation, CustomerPersonality personality, Texture2D picture)
    {
        this.name = name;
        this.picture = (picture != null) ? picture : defaultTexture; //if picture is null, use default texture 
        this.occupation = occupation;
        this.personality = personality;
    }
}
