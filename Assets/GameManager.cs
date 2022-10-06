using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType { Strawberry, Kiwi, Lemon };
public class GameManager : MonoBehaviour
{
    List<CustomerController> customerBubbles;

    public GameObject customerPrefab;

    public static int orderSize = 4;

    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        customerBubbles = new List<CustomerController>();
        GenerateOrder();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VerifyFruit(PokeableFruit[] pokedFruits) {
        FruitType[] fruitOnStick = new FruitType[orderSize];
        for(int i = 0; i < orderSize; i++) {
            fruitOnStick[i] = pokedFruits[i].fruitType;
        }
        foreach(CustomerController customer in customerBubbles) { 
            if(customer.OrderSatisfied(fruitOnStick)) {
                Debug.Log("Order satisfied!");
                customer.gameObject.SetActive(false);
                break;
            }
        }
    }
    
    void GenerateOrder() {
        CustomerController newCustomer = GetCustomer();
        FruitType[] newOrder = new FruitType[orderSize];
        for(int i = 0; i < orderSize; i++) {
            newOrder[i] = (FruitType)Random.Range(0, 3);
        }
        newCustomer.Initialize(newOrder);
    }

    CustomerController GetCustomer() { 
        for(int i = 0; i < customerBubbles.Count; i++) { 
            if(!customerBubbles[i].gameObject.activeSelf) {
                customerBubbles[i].gameObject.SetActive(true);
                return customerBubbles[i];
            }
        }
        CustomerController newCustomer = Instantiate(customerPrefab).GetComponent<CustomerController>();
        customerBubbles.Add(newCustomer);
        return newCustomer;
    }
}
