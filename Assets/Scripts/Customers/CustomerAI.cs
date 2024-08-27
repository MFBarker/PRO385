using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
/*
 Customer's AI State Machine & Sprite in-store
 */
public class CustomerAI
{
    public Customer customer;
    public Sprite spriteNormal;
    public Sprite spriteMad;
    public string order;

    public float timer = 0.0f;
    public bool isSeated;

    public CustomerAI(string customer, string spriteNormal, string spriteMad, string order)
    {
        this.customer = Customers.Instance.GetCustomer(customer);
        this.spriteNormal = AssetDatabase.LoadAssetAtPath<Sprite>(spriteNormal);
        this.spriteMad = AssetDatabase.LoadAssetAtPath<Sprite>(spriteMad);
        this.order = order;
    }

    public float SetTimer()
    {
        switch (customer.personality)
        {
            case CustomerPersonality.Grumpy:
                timer = 30;
                break;
            case CustomerPersonality.Polite:
                timer = 100;
                break;
            case CustomerPersonality.Prideful:
                timer = 60;
                break;
            case CustomerPersonality.Outgoing:
                timer = 90;
                break;
        }

        return timer;
    }

    public void MadSprite()
    { 
        
    }
}

