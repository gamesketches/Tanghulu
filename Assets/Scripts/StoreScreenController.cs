using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FillOutPurchases();
    }

    public void ReturnToTitleScreen() {
        LoadingScreenManager.instance.LoadScene(SceneType.TitleScreen);
    }

    public void FillOutPurchases() { 
        
    }
}
