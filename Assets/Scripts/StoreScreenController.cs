using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreScreenController : MonoBehaviour
{
    public Transform schemeButtons;
    public TextMeshProUGUI currentCoins;
    public IAPManager iapManager;

    public ColorSchemeManager colorSchemeManager;

    public Image coinPurchaseMenu;
    public Image schemePurchaseMenu;

    public int schemePrice;
    // Start is called before the first frame update
    void Start()
    {
        currentCoins.text = SaveDataManager.instance.GetPlayerCoins().ToString("000");

        int preferredColorScheme = SaveDataManager.instance.GetPlayerPreferredColorScheme();
        Button preferredSchemeButton = schemeButtons.GetChild(preferredColorScheme).GetComponent<Button>();
        preferredSchemeButton.Select();
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
        else {
            SaveDataManager.instance.SetPlayerPreferredColorScheme(schemeId);
            colorSchemeManager.UpdateColorScheme(schemeId);
            schemePurchaseMenu.gameObject.SetActive(true);
        }
    }

    public void CloseSchemeMenu() {
        schemePurchaseMenu.gameObject.SetActive(false);
    }

    public void PurchaseScheme(int schemeId) {
        int numCoins = int.Parse(currentCoins.text);
        if (numCoins > schemePrice) {
            SaveDataManager.instance.UpdateOwnedColorSchemes(schemeId);
            SaveDataManager.instance.UpdatePlayerCoins(schemePrice);
            UpdateSelectedScheme(schemeId);
        } else {
            Debug.Log("Not enough coins :<");
        }
    }

    void UpdateSelectedScheme(int schemeId) { 
        SaveDataManager.instance.SetPlayerPreferredColorScheme(schemeId);
        colorSchemeManager.UpdateColorScheme(schemeId);
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
