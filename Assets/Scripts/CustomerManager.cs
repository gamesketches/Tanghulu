using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerManager : MonoBehaviour
{
    List<CustomerController> customerPool;
    List<CustomerController> activeCustomers;

    [Header("Point Text Stuff")]
    public TextMeshProUGUI pointText;
    public float pointFloatTime;
    public float pointFloatDistance;

    [Header("Customer stuff")]
    public GameObject customerPrefab;
    public Vector3 firstCustomerPosition;
    public float customerOffset;
    
    public CustomerSpritePack[] customerSprites;

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
        pointText.enabled = false;
    }

    public CustomerController SatisfiesCustomer(FruitType[] curOrder)
    {
        int highestScore = -1;
        int highestScoringCustomer = -1;
        for (int i = 0; i < activeCustomers.Count; i++)
        {
            CustomerController customer = activeCustomers[i];
            if (!customer.gameObject.activeSelf) continue;
            int score = customer.CalculateSatisfaction(curOrder);
            if (score > highestScore)
            {
                highestScore = score;
                highestScoringCustomer = i;
                //return activeCustomers[i];
            }
        }
        if (highestScoringCustomer > -1) return activeCustomers[highestScoringCustomer];
        else return null;
    }

    public void ServeCustomer(CustomerController customer, FruitType[] curOrder) {
        int customerIndex = activeCustomers.IndexOf(customer);
        int score = customer.CalculateSatisfaction(curOrder);
        customer.UpdateSprite(score);
        StartCoroutine(ShowPointsScored(customer.transform.position, score));
        SFXManager.instance.PlaySoundEffect(SoundEffectType.Success);
        customer.Leave();
        activeCustomers.RemoveAt(customerIndex);
        for(int i = customerIndex; i < activeCustomers.Count; i++) {
            activeCustomers[i].MoveToSpotInLine(GetCustomerPhysicalPosition(i));
        }
        ScorePoints(score);
        numCustomers--;
    }

    IEnumerator ShowPointsScored(Vector3 startPoint, int pointsScored) {
        pointText.transform.position = startPoint;
        pointText.text = pointsScored.ToString();
        pointText.enabled = true;
        Vector3 endPoint = startPoint + (Vector3.up * pointFloatDistance);
        for(float t = 0; t < pointFloatTime; t += Time.deltaTime) {
            pointText.transform.position = Vector3.Lerp(startPoint, endPoint, t);
            yield return null;
        }
        pointText.enabled = false;
    }

    public bool CustomerServed(FruitType[] curOrder) { 
        for(int i = 0; i < activeCustomers.Count; i++) {
            CustomerController customer = activeCustomers[i];
            if (!customer.gameObject.activeSelf) continue;
            int score = customer.CalculateSatisfaction(curOrder);
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

    CustomerSpritePack GetCustomerSprite() {
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

    public FruitType[] GetNeededFruits() {
        List<FruitType> neededFruits;
        neededFruits = new List<FruitType>();
        for(int i = 0; i < activeCustomers.Count; i++) { 
            for(int j = 0; j < GameManager.orderSize; j++) {
                neededFruits.Add(activeCustomers[i].order[j]);
            }
        }
        return neededFruits.ToArray();
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
        //PokingStickController.StickFinishedPoking += DecreasePokeBonus;
    }
    
    private void OnDisable() {
        GameManager.EndGame -= DismissCustomers;
    }
}
