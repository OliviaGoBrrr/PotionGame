using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string paramName;
    void Start()
    {
        slider = GetComponent<Slider>();
        audioMixer.GetFloat(paramName, out float audioLevel);
        if (audioLevel == -80f) { slider.value = 0; }
        else { slider.value = Mathf.Pow(10, audioLevel / 20); }
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value == 0) { audioMixer.SetFloat(paramName, -80f); }
        else { audioMixer.SetFloat(paramName, Mathf.Log10(slider.value) * 20); }
    }
}
