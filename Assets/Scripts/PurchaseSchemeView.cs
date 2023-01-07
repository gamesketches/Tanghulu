using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseSchemeView : MonoBehaviour
{

    public Image purchaseButton;
    public Sprite[] coinPriceButtons;

    public void SetPurchaseButton(SchemePricePoint coinPriceButton) {
        purchaseButton.sprite = coinPriceButtons[(int) coinPriceButton];
    }
}
