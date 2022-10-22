using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ComicController : MonoBehaviour
{
    public ComicImage[] comicImages;
    // Start is called before the first frame update
    void Start()
    {
        foreach (ComicImage panel in comicImages) panel.image.enabled = false;
        StartCoroutine(PlayComic());
    }

    IEnumerator PlayComic() { 
        foreach(ComicImage panel in comicImages) {
            panel.image.enabled = true;
            yield return new WaitForSeconds(panel.playTime);
        }
    }

    public void LoadGameplayScene() {
        SceneManager.LoadScene(2);
        }
}

[Serializable]
public class ComicImage {
    public Image image;
    public float playTime;
}
