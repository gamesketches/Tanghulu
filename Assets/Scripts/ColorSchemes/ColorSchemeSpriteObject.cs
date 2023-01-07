using UnityEngine;

public class ColorSchemeSpriteObject : MonoBehaviour
{
    public ColorSchemeObject schemeObject;
    SpriteRenderer coloredObject;
    // Start is called before the first frame update
    void Awake()
    {
        coloredObject = GetComponent<SpriteRenderer>();
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
                coloredObject.sprite = curColorScheme.potBottom;
                break;
            case ColorSchemeObject.PotTop:
                coloredObject.sprite = curColorScheme.potTop;
                break;
            case ColorSchemeObject.MainColor:
                coloredObject.sprite = curColorScheme.mainColorSprite;
                break;
        }
    }

    void OnEnable() { 
        ColorSchemeManager.ColorSchemeChanged += UpdateColorScheme;
        UpdateColorScheme();
    }

    void OnDisable() { 
        ColorSchemeManager.ColorSchemeChanged -= UpdateColorScheme;
    }
}
