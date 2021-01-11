using UnityEngine;
using UnityEngine.UI;

public sealed class WebCamSelector : MonoBehaviour
{
    [SerializeField] Dropdown _dropdown = null;

    WebCamTexture _webcam;

    public bool ready => _webcam != null;
    public Texture texture => _webcam;

    void Start()
    {
        _dropdown.ClearOptions();
        _dropdown.options.Add(new Dropdown.OptionData("--"));
        foreach (var device in WebCamTexture.devices)
            _dropdown.options.Add(new Dropdown.OptionData(device.name));
        _dropdown.value = 0;
        _dropdown.RefreshShownValue();
    }

    public void OnChangeValue(int value)
    {
        if (_webcam != null)
        {
            _webcam.Stop();
            Destroy(_webcam);
        }

        if (value == 0) return;

        var deviceName = _dropdown.options[value].text;
        _webcam = new WebCamTexture(deviceName, 1920, 1080, 60);
        _webcam.Play();
    }
}
