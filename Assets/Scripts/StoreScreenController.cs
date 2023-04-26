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

    public TextMeshProUGUI purchaseErrorText;

    // Start is called before the first frame update
    void Start()
    {
        currentCoins.text = SaveDataManager.instance.GetPlayerCoins().ToString("000");

        int preferredColorScheme = SaveDataManager.instance.GetPlayerPreferredColorScheme();
        //Button preferredSchemeButton = schemeButtons.GetChild(preferredColorScheme).GetComponent<Button>();
        //preferredSchemeButton.GetComponent<Image>().sprite = colorSchemes[preferredColorScheme].storeSpriteHighlighted;
        for(int i = 0; i < schemeButtons.childCount; i++) {
            Image schemeButton = schemeButtons.GetChild(i).GetComponent<Image>();
            Image useButton = schemeButton.transform.GetChild(0).GetComponent<Image>();
            useButton.enabled = true;
            if (i == preferredColorScheme)
            {
                schemeButton.sprite = colorSchemes[i].storeSpriteHighlighted;
                //schemeButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
                useButton.enabled = false;
            }
            else if (SaveDataManager.instance.PlayerOwnsScheme(i))
                useButton.sprite = useSprite;
            else
                schemeButton.sprite = colorSchemes[i].storeSprite;
        }
        coinPurchaseMenu.gameObject.SetActive(false);
        schemePurchaseMenu.gameObject.SetActive(false);
        purchaseErrorText.text = "";
    }

    public void ReturnToTitleScreen() {
        LoadingScreenManager.instance.LoadScene(SceneType.TitleScreen);
    }

    public void OpenCoinPurchaseMenu() {
        coinPurchaseMenu.gameObject.SetActive(true);
        purchaseErrorText.text = "";
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
        int oldScheme = SaveDataManager.instance.GetPlayerPreferredColorScheme();
        for(int i = 0; i < schemeButtons.childCount; i++) {
            if (i == schemeId)
            {
                schemeButtons.GetChild(i).GetComponent<Image>().sprite = colorSchemes[i].storeSpriteHighlighted;
                schemeButtons.GetChild(i).GetChild(0).GetComponent<Image>().enabled = false;
            }
            else
            {
                schemeButtons.GetChild(i).GetComponent<Image>().sprite = colorSchemes[i].storeSprite;
                if (i == oldScheme)
                    schemeButtons.GetChild(i).GetChild(0).GetComponent<Image>().enabled = true;
            }
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
                return -500;
            case SchemePricePoint.OneThousand:
                return -1000;
            case SchemePricePoint.TwoThousand:
                return -2000;
            case SchemePricePoint.ThreeThousand:
                return -3000;
            case SchemePricePoint.FourThousand:
                return -4000;
            case SchemePricePoint.FiveThousand:
                return -5000;
            default:
                return 0;
        }
    }

    public void Purchase2000Coins() { 
        iapManager.BuyProductID(coinPurchaseLevels.twoThousandCoins);
    }

    public void Purchase5kCoins() {
        iapManager.BuyProductID(coinPurchaseLevels.fiveThousandCoins);
    }

    public void ShowPurchaseError(string errorString) {
        purchaseErrorText.text = errorString;
    }
    
    public void CoinsUpdated() { 
        currentCoins.text = SaveDataManager.instance.GetPlayerCoins().ToString("000");
        coinPurchaseMenu.gameObject.SetActive(false);
        purchaseErrorText.text = "";
    }
}
