using UnityEngine;

public class ColorSchemeSpriteObject : MonoBehaviour
{
    public ColorSchemeObject schemeObject;
    SpriteRenderer coloredObject;
    // Start is called before the first frame update
    void Start()
    {
        coloredObject = GetComponent<SpriteRenderer>();
        ColorSchemeManager.ColorSchemeChanged += UpdateColorScheme;
        UpdateColorScheme();
    }

    void UpdateColorScheme() {
        Debug.Log("updating color scheme from " + gameObject.name.ToString());
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
        }
    }

}
