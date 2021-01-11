using UnityEngine;
using UnityEngine.UI;

public sealed class EffectSelector : MonoBehaviour
{
    [SerializeField] Dropdown _dropdown = null;
    [SerializeField] GameObject[] _effects = null;

    GameObject _activeEffect;

    void Start()
    {
        _dropdown.ClearOptions();
        foreach (var effect in _effects)
            _dropdown.options.Add(new Dropdown.OptionData(effect.name));
        _dropdown.value = 0;
        _dropdown.RefreshShownValue();

        _activeEffect = _effects[0];
        _activeEffect.SetActive(true);
    }

    public void OnChangeValue(int value)
    {
        _activeEffect.SetActive(false);
        _activeEffect = _effects[value];
        _activeEffect.SetActive(true);
    }
}
