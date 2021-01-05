Shader "Hidden/Svira/Delay"
{
    Properties
    {
        _BufferTex("", 2DArray) = "" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    #define HISTORY 64

    UNITY_DECLARE_TEX2DARRAY(_BufferTex);
    float _DelayAmount;
    uint _FrameCount;

    float3 GetHistory(float2 uv, uint offset)
    {
        uint i = (_FrameCount + HISTORY - offset) & (HISTORY - 1);
        return UNITY_SAMPLE_TEX2DARRAY(_BufferTex, float3(uv, i)).rgb;
    }

    void Vertex(uint vid : SV_VertexID,
                out float4 pos : SV_Position,
                out float2 uv : TEXCOORD0)
    {
        float x = vid >> 1;
        float y = (vid & 1) ^ (vid >> 1);

        pos = float4(float2(x, y) * 2 - 1, 1, 1);
        uv = float2(x, y);
    }

    float4 Fragment(float4 pos : SV_Position,
                    float2 uv : TEXCOORD0) : SV_Target
    {
        float3 acc = 0;

        for (uint i = 0; i < 8; i++)
        {
            // Source with monochrome + contrast
            float3 c = GetHistory(uv, i * _DelayAmount);

            // Hue
            float h = i / 8.0 * 6 - 2;
            c *= saturate(float3(abs(h - 1) - 1, 2 - abs(h), 2 - abs(h - 2)));

            // Accumulation
            acc += c / 4;
        }

        return float4(acc, 1);
    }

    ENDCG

    SubShader
    {
        Pass
        {
            Cull off ZTest Always
            CGPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            ENDCG
        }
    }
}
