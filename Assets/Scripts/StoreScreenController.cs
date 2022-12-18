using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreScreenController : MonoBehaviour
{
    public Transform schemeButtons;
    public TextMeshProUGUI currentCoins;

    public ColorSchemeManager colorSchemeManager;
    // Start is called before the first frame update
    void Start()
    {
        currentCoins.text = SaveDataManager.instance.GetPlayerCoins().ToString("000");

        int preferredColorScheme = SaveDataManager.instance.GetPlayerPreferredColorScheme();
        Button preferredSchemeButton = schemeButtons.GetChild(preferredColorScheme).GetComponent<Button>();
        preferredSchemeButton.Select();
    }

    public void ReturnToTitleScreen() {
        LoadingScreenManager.instance.LoadScene(SceneType.TitleScreen);
    }

    public void OpenCoinPurchaseMenu() {
        }

    public void OpenSchemeMenu(int schemeId) {
        SaveDataManager.instance.SetPlayerPreferredColorScheme(schemeId);
        colorSchemeManager.UpdateColorScheme(schemeId);
    }

    public void PurchaseScheme(int schemeId) { 
        
    }
}
