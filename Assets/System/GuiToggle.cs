using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GuiToggle : MonoBehaviour
{
    [SerializeField] GameObject _guiRoot = null;
    [SerializeField] InputAction _action = null;

    void Start()
    {
        _action.performed += OnPerformed;
        _action.Enable();
    }

    void OnPerformed(InputAction.CallbackContext ctx)
    {
        _guiRoot.SetActive(!_guiRoot.activeSelf);
        Cursor.visible = !Cursor.visible;
    }
}
