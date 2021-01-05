using UnityEngine;

public sealed class Delay : MonoBehaviour
{
    [SerializeField, Range(0, 1)] float _delayAmount = 0.5f;
    [SerializeField, HideInInspector] Shader _shader = null;

    public float DelayAmount
      { get => _delayAmount; set => _delayAmount = value; }

    const int History = 64;

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
        _material.SetFloat("_DelayAmount", _delayAmount * 7.99f);
        _material.SetInt("_FrameCount", frame);
        Graphics.DrawProceduralNow(MeshTopology.Quads, 4, 1);
    }
}
