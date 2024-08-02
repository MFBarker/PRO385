using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Customer
{
    public string name;
    public Texture2D picture;
    public string occupation;
    public string personality;
}

public class Customers : MonoBehaviour
{
    public Customer[] customers;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
