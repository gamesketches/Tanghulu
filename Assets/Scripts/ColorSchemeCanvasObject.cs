using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeCanvasObject : MonoBehaviour
{
    public ColorSchemeObject schemeObject;

    Image coloredObject;
    // Start is called before the first frame update
    void Start()
    {
        coloredObject = GetComponent<Image>();
        ColorSchemeManager.ColorSchemeChanged += UpdateColorScheme;
        UpdateColorScheme();
    }

    void UpdateColorScheme() {
        ColorScheme curColorScheme = ColorSchemeManager.currentColorScheme;
        switch(schemeObject) {
            case ColorSchemeObject.Banner:
                coloredObject.sprite = curColorScheme.banner;
                break;
            case ColorSchemeObject.Background:
                coloredObject.sprite = curColorScheme.background;
                break;
            case ColorSchemeObject.TableSprite:
                coloredObject.sprite = curColorScheme.tableSprite;
                break;
            case ColorSchemeObject.PotBottom:
                Debug.LogWarning("Canvas object set to pot top, mistake?");
                coloredObject.sprite = curColorScheme.potBottom;
                break;
            case ColorSchemeObject.PotTop:
                Debug.LogWarning("Canvas object set to pot top, mistake?");
                coloredObject.sprite = curColorScheme.potTop;
                break;
        }
    }
}
