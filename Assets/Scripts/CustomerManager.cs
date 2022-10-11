using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    List<CustomerController> customerBubbles;

    public GameObject customerPrefab;
    public Vector3 firstCustomerPosition;
    public float customerOffset;
    
    public Sprite[] customerSprites;

    public static CustomerManager instance;
    public delegate void PointsScored(int points);
    public static event PointsScored ScorePoints;

    int numCustomers;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        customerBubbles = new List<CustomerController>();
        numCustomers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CustomerServed(FruitType[] curOrder) { 
        foreach(CustomerController customer in customerBubbles) {
            if (!customer.gameObject.activeSelf) continue;
            int score = customer.OrderSatisfied(curOrder);
            Debug.Log($"You scored {score} points");
            if(score > 0) {
                customer.Leave();
                ScorePoints(score);
                numCustomers--;
                return true;
            }
        }
        return false;
    }

    public void MakeNewCustomer(FruitType[] newOrder) {
        CustomerController newCustomer = GetCustomer();
        newCustomer.Initialize(newOrder, GetCustomerSprite(), GetCustomerPositionInLine());
        numCustomers++;
    }

    Sprite GetCustomerSprite() {
        return customerSprites[Random.Range(0, customerSprites.Length)];
    }
    
    Vector3 GetCustomerPositionInLine() {
        return firstCustomerPosition + new Vector3(customerOffset * numCustomers, 0, 0);
    }

    CustomerController GetCustomer() { 
        for(int i = 0; i < customerBubbles.Count; i++) { 
            if(!customerBubbles[i].gameObject.activeSelf) {
                customerBubbles[i].gameObject.SetActive(true);
                return customerBubbles[i];
            }
        }
        CustomerController newCustomer = Instantiate(customerPrefab).GetComponent<CustomerController>();
        newCustomer.transform.position = firstCustomerPosition + new Vector3(customerOffset * 5, 0, 0);
        customerBubbles.Add(newCustomer);
        return newCustomer;
    }
}
