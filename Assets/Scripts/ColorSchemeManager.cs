using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorSchemeObject { Banner, Background, TableSprite, PotTop, PotBottom};
public class ColorSchemeManager : MonoBehaviour
{
    public ColorScheme[] colorSchemes;

    public static ColorScheme currentColorScheme;

    public delegate void ColorSchemeChangeEvent();
    public static event ColorSchemeChangeEvent ColorSchemeChanged;
    // Start is called before the first frame update
    void Awake()
    {
        UpdateColorScheme(SaveDataManager.instance.GetPlayerPreferredColorScheme());
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateColorScheme(int newColorScheme) {
        currentColorScheme = colorSchemes[newColorScheme];
        ColorSchemeChanged();
    }
}
