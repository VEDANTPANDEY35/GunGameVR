Shader "Custom/CrosshairAlwaysOnTop"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue"="Overlay" }

        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            SetTexture [_MainTex] { combine texture }
        }
    }
}