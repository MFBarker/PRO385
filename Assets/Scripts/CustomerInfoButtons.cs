using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AdaptivePerformance.Provider;
using UnityEngine.UI;

public class CustomerInfoButtons : MonoBehaviour
{
    [SerializeField] string CustomerName;

    [SerializeField] Image pictureUI;
    [SerializeField] TMP_Text nameUI;
    [SerializeField] TMP_Text jobUI;
    [SerializeField] TMP_Text personalityUI;

    Customer customer;
    private void Start()
    {
        //get customer
        customer = Customers.Instance.GetCustomer(CustomerName);
    }

    public void OnClick()
    {
        pictureUI.sprite = customer.picture;
        nameUI.text = customer._name;
        jobUI.text = customer.occupation;
        personalityUI.text = customer.personality.ToString();
    }
}
