using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    bool gameStarted;
    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() {
        if (!gameStarted)
        {
            gameStarted = true;
            LoadingScreenManager.instance.LoadScene(SceneType.ComicScene);
        }
    }
}
