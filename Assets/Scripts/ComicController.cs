using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ComicController : MonoBehaviour
{
    public ComicImage[] comicImages;
    public int particleSystemFirePoint;

    bool gameStarted;

    public ParticleSystem fruitParticles;
    // Start is called before the first frame update
    void Start()
    {
        foreach (ComicImage panel in comicImages) panel.image.enabled = false;
        StartCoroutine(PlayComic());
        gameStarted = false;
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            foreach(ComicImage comicImage in comicImages) {
                comicImage.image.enabled = true;
            }
            StopCoroutine(PlayComic());
        }
    }

    IEnumerator PlayComic() { 
        for(int i = 0; i < comicImages.Length; i++) {
            if (i == particleSystemFirePoint) fruitParticles.Play();
            comicImages[i].image.enabled = true;
            yield return new WaitForSeconds(comicImages[i].playTime);
        }
    }

    public void LoadGameplayScene() {
        if (!gameStarted)
        {
            LoadingScreenManager.instance.LoadScene(SceneType.RotatingPot);
            gameStarted = true;
        }
    }
}

[Serializable]
public class ComicImage {
    public Image image;
    public float playTime;
}
