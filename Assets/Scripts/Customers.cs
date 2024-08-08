using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Customers : MonoBehaviour
{
    [SerializeField]
    public Customer[] customers = {
        new Customer("... Tanaka","Farmer", CustomerPersonality.Grumpy, null),
        new Customer("Haruto Nakamura","Salaryman", CustomerPersonality.Polite, null),
        new Customer("... Soma","Manager", CustomerPersonality.Prideful, null),
        new Customer("Logan Smith","English Teacher", CustomerPersonality.Outgoing, null)
    };


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
