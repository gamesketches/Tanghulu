using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum SchemePricePoint { FiveHundred, OneThousand, TwoThousand, ThreeThousand, FourThousand, FiveThousand};
public class StoreScreenController : MonoBehaviour
{
    public Transform schemeButtons;
    public Sprite useSprite;
    public TextMeshProUGUI currentCoins;
    public IAPManager iapManager;

    public ColorSchemeManager colorSchemeManager;

    public Image coinPurchaseMenu;
    public PurchaseSchemeView schemePurchaseMenu;

    int currentInspectedScheme;

    public ColorScheme[] colorSchemes;

    // Start is called before the first frame update
    void Start()
    {
        currentCoins.text = SaveDataManager.instance.GetPlayerCoins().ToString("000");

        int preferredColorScheme = SaveDataManager.instance.GetPlayerPreferredColorScheme();
        //Button preferredSchemeButton = schemeButtons.GetChild(preferredColorScheme).GetComponent<Button>();
        //preferredSchemeButton.GetComponent<Image>().sprite = colorSchemes[preferredColorScheme].storeSpriteHighlighted;
        for(int i = 0; i < schemeButtons.childCount; i++) {
            if(i == preferredColorScheme)
                schemeButtons.GetChild(i).GetComponent<Image>().sprite = colorSchemes[i].storeSpriteHighlighted;
            else if(SaveDataManager.instance.PlayerOwnsScheme(i)) 
                schemeButtons.GetChild(i).GetChild(0).GetComponent<Image>().sprite = useSprite;
            else
                schemeButtons.GetChild(i).GetComponent<Image>().sprite = colorSchemes[i].storeSprite;
        }
        coinPurchaseMenu.gameObject.SetActive(false);
        schemePurchaseMenu.gameObject.SetActive(false);
    }

    public void ReturnToTitleScreen() {
        LoadingScreenManager.instance.LoadScene(SceneType.TitleScreen);
    }

    public void OpenCoinPurchaseMenu() {
        coinPurchaseMenu.gameObject.SetActive(true);
    }

    public void CloseCoinPurchaseMenu() {
        coinPurchaseMenu.gameObject.SetActive(false);
    }

    public void OpenSchemeMenu(int schemeId) {
        if (SaveDataManager.instance.PlayerOwnsScheme(schemeId)) UpdateSelectedScheme(schemeId);
        else
        {
            schemePurchaseMenu.gameObject.SetActive(true);
            schemePurchaseMenu.SetPurchaseButton(GetSchemePriceType(schemeId));
            currentInspectedScheme = schemeId;
        }
    }

    public void CloseSchemeMenu() {
        schemePurchaseMenu.gameObject.SetActive(false);
    }

    public void PurchaseScheme() {
        int numCoins = int.Parse(currentCoins.text);
        int schemePrice = GetSchemePrice(currentInspectedScheme);
        //TODO: Remove before release!!
        //schemePrice = 1;
        if (numCoins > schemePrice) {
            SaveDataManager.instance.UpdateOwnedColorSchemes(currentInspectedScheme);
            SaveDataManager.instance.UpdatePlayerCoins(schemePrice);
            UpdateSelectedScheme(currentInspectedScheme);
        } else {
            Debug.Log("Not enough coins :<");
        }
    }

    void UpdateSelectedScheme(int schemeId) { 
        for(int i = 0; i < schemeButtons.childCount; i++) {
            if(i == schemeId)
                schemeButtons.GetChild(i).GetComponent<Image>().sprite = colorSchemes[i].storeSpriteHighlighted;
            else
                schemeButtons.GetChild(i).GetComponent<Image>().sprite = colorSchemes[i].storeSprite;
        }
        
        SaveDataManager.instance.SetPlayerPreferredColorScheme(schemeId);
        colorSchemeManager.UpdateColorScheme(schemeId);
    }

    SchemePricePoint GetSchemePriceType(int schemeId) {
        return colorSchemes[schemeId].pricePoint;
    }

    int GetSchemePrice(int schemeId) { 
        switch(colorSchemes[schemeId].pricePoint) {
            case SchemePricePoint.FiveHundred:
                return 500;
            case SchemePricePoint.OneThousand:
                return 1000;
            case SchemePricePoint.TwoThousand:
                return 2000;
            case SchemePricePoint.ThreeThousand:
                return 3000;
            case SchemePricePoint.FourThousand:
                return 4000;
            case SchemePricePoint.FiveThousand:
                return 5000;
            default:
                return 0;
        }
    }

    public void Purchase500Coins() { 
        iapManager.BuyProductID(coinPurchaseLevels.fiveThousandCoins);
        coinPurchaseMenu.gameObject.SetActive(false);
    }

    public void Purchase2500Coins() { 
        iapManager.BuyProductID(coinPurchaseLevels.fiveThousandCoins);
        coinPurchaseMenu.gameObject.SetActive(false);
    }

    public void Purchase5kCoins() {
        iapManager.BuyProductID(coinPurchaseLevels.fiveThousandCoins);
        coinPurchaseMenu.gameObject.SetActive(false);
    }
    
    public void Purchase50kCoins() {
        iapManager.BuyProductID(coinPurchaseLevels.fiftyThousandCoins);
        coinPurchaseMenu.gameObject.SetActive(false);
    }
    
    public void Purchase500kCoins() {
        iapManager.BuyProductID(coinPurchaseLevels.fiveHundredThousandCoins);
        coinPurchaseMenu.gameObject.SetActive(false);
    }

}
