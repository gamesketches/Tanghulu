using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorSchemeObject { Banner, Background, TableSprite, PotTop, PotBottom, MainColor};
public class ColorSchemeManager : MonoBehaviour
{
    public ColorScheme[] colorSchemes;

    public static ColorScheme currentColorScheme;

    public delegate void ColorSchemeChangeEvent();
    public static event ColorSchemeChangeEvent ColorSchemeChanged;
    // Start is called before the first frame update
    void Awake()
    {
        int targetColorScheme = SaveDataManager.instance.GetPlayerPreferredColorScheme();
        currentColorScheme = colorSchemes[targetColorScheme];
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateColorScheme(int newColorScheme) {
        currentColorScheme = colorSchemes[newColorScheme];
        ColorSchemeChanged();
    }
}
