using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType { Strawberry, Kiwi, Lemon };
public class GameManager : MonoBehaviour
{
    List<OrderBubble> orderBubbles;

    public GameObject orderPrefab;

    public static int orderSize = 4;

    public static GameManager instance;
    // Start is called before the first frame update
    void Start()
    {
        orderBubbles = new List<OrderBubble>();
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
        foreach(OrderBubble orderBubble in orderBubbles) { 
            if(orderBubble.OrderSatisfied(fruitOnStick)) {
                Debug.Log("Order satisfied!");
                orderBubble.gameObject.SetActive(false);
                break;
            }
        }
    }
    
    void GenerateOrder() {
        OrderBubble newOrderBubble = GetOrderBubble();
        FruitType[] newOrder = new FruitType[orderSize];
        for(int i = 0; i < orderSize; i++) {
            newOrder[i] = (FruitType)Random.Range(0, 3);
        }
        newOrderBubble.SetFruits(newOrder);
    }

    OrderBubble GetOrderBubble() { 
        for(int i = 0; i < orderBubbles.Count; i++) { 
            if(!orderBubbles[i].gameObject.activeSelf) {
                orderBubbles[i].gameObject.SetActive(true);
                return orderBubbles[i];
            }
        }
        OrderBubble newBubble = Instantiate(orderPrefab).GetComponent<OrderBubble>();
        orderBubbles.Add(newBubble);
        return newBubble;
    }
}
