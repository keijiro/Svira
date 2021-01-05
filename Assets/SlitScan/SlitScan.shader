Shader "Hidden/Svira/SlitScan"
{
    Properties
    {
        _BufferTex("", 2DArray) = "" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    #define HISTORY 256

    UNITY_DECLARE_TEX2DARRAY(_BufferTex);
    float _AxisSwitch;
    float _DelayAmount;
    uint _FrameCount;
    uint _BufferCount;

    float3 GetHistory(float2 uv, uint offset)
    {
        offset = min(_BufferCount, offset);
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
        float delay = lerp(uv.x, 1 - uv.y, _AxisSwitch) * _DelayAmount;
        uint offset = (uint)delay;
        float3 p1 = GetHistory(uv, offset + 0);
        float3 p2 = GetHistory(uv, offset + 1);
        return float4(lerp(p1, p2, frac(delay)), 1);
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
