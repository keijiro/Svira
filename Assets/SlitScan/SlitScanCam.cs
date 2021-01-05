using UnityEngine;

class SlitScanCam : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _delayAmount = 0.5f;
    [SerializeField] bool _rotateAxis = false;
    [SerializeField, HideInInspector] Shader _shader = null;

    const int History = 256;

    Material _material;
    Texture2DArray _buffer;
    WebCamTexture _webcam;

    void Start()
    {
        _material = new Material(_shader);

        _buffer = new Texture2DArray
          (1920, 1080, History, TextureFormat.RGB565, false);
        _buffer.filterMode = FilterMode.Bilinear;
        _buffer.wrapMode = TextureWrapMode.Clamp;

        _webcam = new WebCamTexture();
        _webcam.Play();
    }

    void OnPostRender()
    {
        var frame = Time.frameCount & (History - 1);

        var ac = RenderTexture.active;
        Graphics.ConvertTexture(_webcam, 0, _buffer, frame);
        RenderTexture.active = ac;

        _material.SetPass(0);
        _material.SetTexture("_BufferTex", _buffer);
        _material.SetFloat("_AxisSwitch", _rotateAxis ? 0 : 1);
        _material.SetFloat("_DelayAmount", _delayAmount * 255);
        _material.SetInt("_FrameCount", frame);
        Graphics.DrawProceduralNow(MeshTopology.Quads, 4, 1);
    }
}
