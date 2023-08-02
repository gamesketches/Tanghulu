using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeCanvasObject : MonoBehaviour
{
    public ColorSchemeObject schemeObject;

    Image coloredObject;
    // Start is called before the first frame update
    void Awake()
    {
        coloredObject = GetComponent<Image>();
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
            case ColorSchemeObject.MainColor:
                coloredObject.sprite = curColorScheme.mainColorSprite;
                break;
            case ColorSchemeObject.MainColorColorChange:
                coloredObject.color = curColorScheme.mainColor;
                break;
        }
    }
    
    void OnEnable() { 
        ColorSchemeManager.ColorSchemeChanged += UpdateColorScheme;
        UpdateColorScheme();
    }

    void OnDisable()
    {
        ColorSchemeManager.ColorSchemeChanged -= UpdateColorScheme;
    }
}
