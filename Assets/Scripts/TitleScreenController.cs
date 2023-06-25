using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour
{

    public AudioMixerGroup mixerGroup;
    public Image soundButton;
    public Sprite mutedButton;
    public Sprite soundOnButton;

    // Start is called before the first frame update
    void Start()
    {
        if (SaveDataManager.instance.PlayerPrefersMute())
        {
            soundButton.sprite = mutedButton;
            mixerGroup.audioMixer.SetFloat("Volume", -80f);
        }
        else
        {
            soundButton.sprite = soundOnButton;
            mixerGroup.audioMixer.SetFloat("Volume", 0f);
        }

    }

    public void StartGame() {
        SFXManager.instance.PlaySoundEffect(SoundEffectType.PokeStick);
        if (SystemInfo.deviceModel.StartsWith("iPad")) LoadingScreenManager.instance.LoadScene(SceneType.RotatingPotiPad);
        else LoadingScreenManager.instance.LoadScene(SceneType.RotatingPot);
    }

    public void OpenShop() {
        SFXManager.instance.PlaySoundEffect(SoundEffectType.PokeStick);
        LoadingScreenManager.instance.LoadScene(SceneType.StoreScreen); 
    }

    public void SwapVolumeSetting() { 
        if (SaveDataManager.instance.PlayerPrefersMute()) {
            soundButton.sprite = soundOnButton;
            SaveDataManager.instance.SetPlayerPrefersMute(false);
            mixerGroup.audioMixer.SetFloat("Volume", 0f);
        } else { 
            soundButton.sprite = mutedButton;
            SaveDataManager.instance.SetPlayerPrefersMute(true);
            mixerGroup.audioMixer.SetFloat("Volume", -80f);
            }
        SFXManager.instance.PlaySoundEffect(SoundEffectType.PokeStick);
    }
}
