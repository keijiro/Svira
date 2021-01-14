using UnityEngine;
using UnityEngine.InputSystem;

public sealed class EffectHotKey : MonoBehaviour
{
    [SerializeField] InputAction _action0 = null;
    [SerializeField] InputAction _action1 = null;
    [SerializeField] InputAction _action2 = null;
    [SerializeField] InputAction _action3 = null;

    void OnEnable()
    {
        _action0.performed += (_) => ChangeEffect(0);
        _action1.performed += (_) => ChangeEffect(1);
        _action2.performed += (_) => ChangeEffect(2);
        _action3.performed += (_) => ChangeEffect(3);

        _action0.Enable();
        _action1.Enable();
        _action2.Enable();
        _action3.Enable();
    }

    void ChangeEffect(int index)
      => GetComponent<UnityEngine.UI.Dropdown>().value = index;
}
