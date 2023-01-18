using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneType { Preload, TitleScreen, ComicScene, RotatingPot, StoreScreen};
public class LoadingScreenManager : MonoBehaviour
{
    public static LoadingScreenManager instance;

    public Canvas canvas;
    public RectTransform curtainRect;

    public float curtainMoveTime;
    public float curtainHoldTime;

    public int activeSortOrder;

    bool loading;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        curtainRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, Screen.height);
        loading = false;
    }

    void Start()
    { 
        LoadScene(SceneType.TitleScreen, false);
    }

    public void LoadScene(SceneType screenType, bool curtainDown = true, bool curtainUp = true) {
        if (!loading)
        {
            loading = true;
            StartCoroutine(LoadScreenWithCurtain(screenType, curtainDown, curtainUp));
        }
    }

    private IEnumerator LoadScreenWithCurtain(SceneType screenType, bool curtainDown = true, bool curtainUp = true) {

        //canvas.enabled = true;
        canvas.sortingOrder = activeSortOrder;
        AsyncOperation sceneLoadHandle = SceneManager.LoadSceneAsync(screenType.ToString());

        sceneLoadHandle.allowSceneActivation = false;

        if (curtainDown) yield return MoveCurtain(true);

        while (sceneLoadHandle.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(curtainHoldTime);
        sceneLoadHandle.allowSceneActivation = true;

        if (curtainUp) yield return MoveCurtain(false);
        
        //canvas.enabled = false;
        canvas.sortingOrder = 0;
        loading = false;
    }

    private IEnumerator MoveCurtain(bool curtainDown) {
        float proportion;
        for(float t = 0; t < curtainMoveTime; t += Time.deltaTime) {
            if (curtainDown) proportion = Mathf.SmoothStep(-Screen.height, 0, t / curtainMoveTime);
            else proportion = Mathf.SmoothStep(0, -Screen.height, t / curtainMoveTime);

            curtainRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, proportion, curtainRect.rect.size.y);
            yield return null;
        }
    }
}
