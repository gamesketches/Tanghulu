using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    List<CustomerController> customerPool;
    List<CustomerController> activeCustomers;


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
        customerPool = new List<CustomerController>();
        activeCustomers = new List<CustomerController>();
        numCustomers = 0;
    }

    public CustomerController SatisfiesCustomer(FruitType[] curOrder)
    {
        for (int i = 0; i < activeCustomers.Count; i++)
        {
            CustomerController customer = activeCustomers[i];
            if (!customer.gameObject.activeSelf) continue;
            int score = customer.OrderSatisfied(curOrder);
            if (score > 0)
            {
                return activeCustomers[i];
            }
        }
        return null;
    }

    public void ServeCustomer(CustomerController customer, FruitType[] curOrder) {
        int customerIndex = activeCustomers.IndexOf(customer);
        int score = customer.OrderSatisfied(curOrder);
        customer.Leave();
        activeCustomers.RemoveAt(customerIndex);
        for(int i = customerIndex; i < activeCustomers.Count; i++) {
            activeCustomers[i].MoveToSpotInLine(GetCustomerPhysicalPosition(i));
        }
        ScorePoints(score);
        numCustomers--;
    }

    public bool CustomerServed(FruitType[] curOrder) { 
        for(int i = 0; i < activeCustomers.Count; i++) {
            CustomerController customer = activeCustomers[i];
            if (!customer.gameObject.activeSelf) continue;
            int score = customer.OrderSatisfied(curOrder);
            if(score > 0) {
                customer.Leave();
                activeCustomers.RemoveAt(i);
                for(int j = i; j < activeCustomers.Count; j++) {
                    activeCustomers[j].MoveToSpotInLine(GetCustomerPhysicalPosition(j));
                }
                ScorePoints(score);
                numCustomers--;
                return true;
            }
        }
        return false;
    }

    public void MakeNewCustomer(FruitType[] newOrder) {
        CustomerController newCustomer = GetCustomer();
        newCustomer.Initialize(newOrder, GetCustomerSprite(), GetCustomerPhysicalPosition(numCustomers));
        numCustomers++;
        activeCustomers.Add(newCustomer);
    }

    Sprite GetCustomerSprite() {
        return customerSprites[Random.Range(0, customerSprites.Length)];
    }

    Vector3 GetCustomerPhysicalPosition(int spotInLine) {
        return firstCustomerPosition + new Vector3(customerOffset * spotInLine, 0, 0);
    }

    private void DismissCustomers() { 
        foreach(CustomerController customer in customerPool) {
            customer.GetDismissed();
        }
    }
    
    CustomerController GetCustomer() { 
        for(int i = 0; i < customerPool.Count; i++) { 
            if(!customerPool[i].gameObject.activeSelf) {
                customerPool[i].gameObject.SetActive(true);
                return customerPool[i];
            }
        }
        CustomerController newCustomer = Instantiate(customerPrefab).GetComponent<CustomerController>();
        newCustomer.transform.position = firstCustomerPosition + new Vector3(customerOffset * 5, 0, 0);
        customerPool.Add(newCustomer);
        return newCustomer;
    }

    private void OnEnable() {
        GameManager.EndGame += DismissCustomers;
    }
    
    private void OnDisable() {
        GameManager.EndGame -= DismissCustomers;
    }
}
