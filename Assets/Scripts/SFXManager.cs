using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundEffectType { PokeFruit, Success, Tally, Footstep, TerribleResult, 
        OkayResult, GoodResult, Perfect, CurtainDown, CurtainUp, AimStick, RotatePot, FruitHit, Miss, PokeStick, Knock};
public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioClip pokeStick;
    public AudioClip knockClip;
    public AudioClip[] pokeClips;
    public AudioClip fruitHitClip;

    int pokeTracker;

    [Header("Curtain Clips")]
    public AudioClip curtainDown;
    public AudioClip curtainUp;

    public AudioClip successClip;

    public AudioClip tallyClip;

    public AudioClip missClip;

    [Header("Threshold Chords")]
    public AudioClip terribleClip;
    public AudioClip okayClip;
    public AudioClip goodClip;
    public AudioClip perfectClip;

    [Header("Aiming")]
    public AudioSource aimingSrc;
    float aimMovement;
    IEnumerator stickClickRoutine;
    public float aimClickThreshold;
    public float minimumClickTime;

    [Header("Rotating")]
    public AudioSource potRotationSrc;
    float potMovement;
    IEnumerator potClickRoutine;
    public float potClickThreshold;

    [Header("Sources and Mixers")]
    public AudioClip walkingClip;

    public AudioMixerGroup SFXMixer;
    public AudioMixerGroup musicMixer;


    public AudioSource menuMusic;

    public AudioSource gameplayMusic;

    private List<AudioSource> audioSourcePool;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        audioSourcePool = new List<AudioSource>();
        pokeTracker = 0;
        aimMovement = 0;
        SwitchToMenuMusic();
    }

    public void PlaySoundEffect(SoundEffectType soundType) { 
        switch(soundType) {
            case SoundEffectType.PokeFruit:
                PlayPokingSound();
                break;
            case SoundEffectType.Success:
                PlaySuccessSound();
                break;
            case SoundEffectType.Tally:
                PlaySound(tallyClip);
                break;
            case SoundEffectType.Perfect:
                PlaySound(perfectClip);
                break;
            case SoundEffectType.GoodResult:
                PlaySound(goodClip);
                break;
            case SoundEffectType.OkayResult:
                PlaySound(okayClip);
                break;
            case SoundEffectType.TerribleResult:
                PlaySound(terribleClip);
                break;
            case SoundEffectType.CurtainDown:
                PlaySound(curtainDown);
                break;
            case SoundEffectType.CurtainUp:
                PlaySound(curtainUp);
                break;
            case SoundEffectType.AimStick:
                if (!aimingSrc.isPlaying) aimingSrc.Play();
                break;
            case SoundEffectType.RotatePot:
                if (!potRotationSrc.isPlaying) potRotationSrc.Play();
                break;
            case SoundEffectType.Miss:
                PlaySound(missClip);
                break;
            case SoundEffectType.PokeStick:
                PlaySound(pokeStick);
                break;
            case SoundEffectType.Knock:
                PlaySound(knockClip);
                break;
        }
    }

    private void PlayPokingSound() {
        //AudioClip randomPoke = pokeClips[pokeTracker];
        //PlaySound(randomPoke);
        //pokeTracker = (pokeTracker + 1) % pokeClips.Length;
        PlaySound(fruitHitClip);
    }

    private void PlaySuccessSound() {
        PlaySound(successClip);
    }

    public void SwitchToGameplayMusic(float volume = 1) {
        menuMusic.volume = 0;
        gameplayMusic.volume = volume;
    }

    public void SwitchToMenuMusic(float volume = 1) {
        menuMusic.volume = 1;
        gameplayMusic.volume = volume;
    }

    public void BeginFadeMusic(float startVolume = 1, float endVolume = 0, float fadeTime = 0.8f) {
        StartCoroutine(FadeMusic(startVolume, endVolume, fadeTime));
    }

    private IEnumerator FadeMusic(float startVolume, float targetVolume, float fadeTime) { 
        for(float t = 0; t < fadeTime; t += Time.deltaTime) {
            float proportion = Mathf.Lerp(startVolume, targetVolume, t / fadeTime);
            musicMixer.audioMixer.SetFloat("MusicVolume", proportion);
            yield return null;
        }
        musicMixer.audioMixer.SetFloat("MusicVolume", targetVolume);
    }

    private void PlaySound(AudioClip audioClip, float pitch = 1, float volume = 1) {
        AudioSource openSource = FindAudioSource();
        openSource.clip = audioClip;
        openSource.pitch = pitch;
        openSource.volume = volume;
        openSource.Play();
    }

    public IEnumerator PlayFootstep(float walkingTime) {
        AudioSource openSource = FindAudioSource();
        openSource.clip = walkingClip;
        openSource.Play();
        yield return new WaitForSeconds(walkingTime);
        openSource.Stop();
    }

    public void AddToAimDistance(float amountToAdd) {
        aimMovement += Mathf.Abs(amountToAdd);
        if(stickClickRoutine == null) {
            stickClickRoutine = PlayAimingSounds();
            StartCoroutine(stickClickRoutine);
        }
    }

    public void StopAimSound() {
        aimMovement = aimMovement % aimClickThreshold;
        if(stickClickRoutine != null) {
            StopCoroutine(stickClickRoutine);
            stickClickRoutine = null;
        }
    }

    IEnumerator PlayAimingSounds() { 
        while(aimMovement > aimClickThreshold) {
            aimMovement -= aimClickThreshold;
            aimingSrc.Play();
            yield return new WaitForSeconds(minimumClickTime);
        }
        stickClickRoutine = null;
    }

    public void AddToRotateDistance(float amountToAdd) {
        potMovement += Mathf.Abs(amountToAdd);
        if(potClickRoutine == null && potMovement > potClickThreshold) {
            potClickRoutine = PlayRotatingSounds();
            StartCoroutine(potClickRoutine);
        }
        
    }

    public void StopRotateSound() {
        potMovement = potMovement % potClickThreshold;
        if(potClickRoutine != null) {
            StopCoroutine(potClickRoutine);
            potClickRoutine = null;
        }
    }

    IEnumerator PlayRotatingSounds() { 
        while(potMovement > potClickThreshold) {
            potMovement -= potClickThreshold;
            potRotationSrc.Play();
            yield return new WaitForSeconds(minimumClickTime);
        }
        potClickRoutine = null;
    }
    private AudioSource FindAudioSource() { 
        foreach(AudioSource src in audioSourcePool) { 
            if(!src.isPlaying) {
                return src;
            }
        }
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.outputAudioMixerGroup = SFXMixer;
        audioSourcePool.Add(newSource);
        return newSource;
        }
}
