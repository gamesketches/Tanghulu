using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PurchaseSchemeView : MonoBehaviour
{
    public Image purchaseButton;
    public Image previewImage;
    public TextMeshProUGUI errorText;
    public Sprite[] coinPriceButtons;
    public float previewSpinSpeed;

    public void Open(Sprite previewSprite, SchemePricePoint pricePoint) {
        SetPreviewImage(previewSprite);
        SetPurchaseButton(pricePoint);
        BeginSpinningScheme();
    }

    public void Close()
    {
        errorText.text = "";
        StopSpinningScheme();
        gameObject.SetActive(false);
    }

    public void SetPreviewImage(Sprite newPreviewSprite) {
        previewImage.sprite = newPreviewSprite;
    }

    public void SetPurchaseButton(SchemePricePoint coinPriceButton) {
        purchaseButton.sprite = coinPriceButtons[(int) coinPriceButton];
    }

    public void ShowError(string error)
    {
        errorText.text = error;
    }

    public void BeginSpinningScheme() {
        StartCoroutine(SpinSchemePreview());
    }

    public void StopSpinningScheme()
    {
        StopAllCoroutines();
        previewImage.transform.rotation = Quaternion.identity;
    }

    IEnumerator SpinSchemePreview() { 
        while(true) {
            previewImage.transform.Rotate(0, 0, previewSpinSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
