using UnityEngine;

public sealed class Bypass : MonoBehaviour
{
    [SerializeField] WebCamSelector _webcam = null;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_webcam.ready)
            Graphics.Blit(_webcam.texture, destination);
        else
            Graphics.Blit(source, destination);
    }
}
