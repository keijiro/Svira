using UnityEngine;

public sealed class SlitScan : MonoBehaviour
{
    [SerializeField] WebCamSelector _webcam = null;
    [SerializeField, Range(0, 1)] float _opacity = 1;
    [SerializeField, Range(0, 1)] float _delayAmount = 0.5f;
    [SerializeField] bool _rotateAxis = false;
    [SerializeField] bool _enableBuffer = true;
    [SerializeField, HideInInspector] Shader _shader = null;

    public float Opacity
      { get => _opacity; set => _opacity = value; }

    public float DelayAmount
      { get => _delayAmount; set => _delayAmount = value; }

    public bool RotateAxis
      { get => _rotateAxis; set => _rotateAxis = value; }

    public bool EnableBuffer
      { get => _enableBuffer; set => _enableBuffer = value; }

    const int History = 256;

    Material _material;
    Texture2DArray _buffer;
    int _bufferCount;

    void Start()
    {
        _material = new Material(_shader);

        _buffer = new Texture2DArray
          (1920, 1080, History, TextureFormat.RGB565, false);
        _buffer.filterMode = FilterMode.Bilinear;
        _buffer.wrapMode = TextureWrapMode.Clamp;
    }

    void Update()
      => _bufferCount = _enableBuffer ? _bufferCount + 1 : 0;

    void OnPostRender()
    {
        if (!_webcam.ready) return;

        var frame = Time.frameCount & (History - 1);

        var ac = RenderTexture.active;
        Graphics.ConvertTexture(_webcam.texture, 0, _buffer, frame);
        RenderTexture.active = ac;

        _material.SetPass(0);
        _material.SetTexture("_BufferTex", _buffer);
        _material.SetFloat("_Opacity", _opacity);
        _material.SetFloat("_DelayAmount", _delayAmount * 255);
        _material.SetFloat("_AxisSwitch", _rotateAxis ? 0 : 1);
        _material.SetInt("_FrameCount", frame);
        _material.SetInt("_BufferCount", _bufferCount);
        Graphics.DrawProceduralNow(MeshTopology.Quads, 4, 1);
    }
}
