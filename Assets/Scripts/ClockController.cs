using UnityEngine;
using UnityEngine.UI;

public class ClockController : MonoBehaviour
{

    public Image clockBacking;

    public void UpdateClockProportion(float newProportion) {
        clockBacking.fillAmount = newProportion;
    }
}
