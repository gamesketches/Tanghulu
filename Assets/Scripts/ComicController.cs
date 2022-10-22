using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class ComicImage {
    public Image image;
    public float playTime;
}
