using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        LoadingScreenManager.instance.LoadScene(SceneType.RotatingPot);
    }

    public void OpenShop() {
        LoadingScreenManager.instance.LoadScene(SceneType.StoreScreen); 
    }
}
