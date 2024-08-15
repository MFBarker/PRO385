using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#region Customer Class
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
    public string _name;
    public Sprite picture;
    public string pictureTexture;
    public string occupation;
    public CustomerPersonality personality;

    /*
    https://stackoverflow.com/questions/26043161/how-to-reference-a-texture-with-a-string
    CS0619: Resources.LoadAssetAtPath is obsolete => AssetDatabase.LoadAssetAtPath
     */
    static string texture = "Assets/Art/Characters/Default.png";
    //Texture2D defaultTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(texture); //moved to Customers.cs
    public Customer(string name, string occupation, CustomerPersonality personality, string pictureT)
    {
        _name = name;
        //if picture is null, use default texture; else use given asset path
        this.pictureTexture = (pictureT != null) ? pictureT : texture;
        this.occupation = occupation;
        this.personality = personality;
    }
}
#endregion

public class Customers : Singleton<Customers>
{
    //All Customers
    [DoNotSerialize]
    public Customer[] customers = {
        new Customer("Akio Tanaka","Farmer", CustomerPersonality.Grumpy, null),
        new Customer("Haruto Nakamura","Salaryman", CustomerPersonality.Polite, null),
        new Customer("Hayato Kami","Manager", CustomerPersonality.Prideful, null),
        new Customer("Logan Smith","English Teacher", CustomerPersonality.Outgoing, null)
    };


    // Start is called before the first frame update
    void Start()
    {
        foreach (Customer customer in customers) 
        {
            //load asset
            customer.picture = AssetDatabase.LoadAssetAtPath<Sprite>(customer.pictureTexture);
        }
    }

    public Customer GetCustomer(string name)
    {
        foreach (var customer in customers)
        {
            if (customer._name == name)
            {
                return customer;
            }
        }

        return null;
    }
}
