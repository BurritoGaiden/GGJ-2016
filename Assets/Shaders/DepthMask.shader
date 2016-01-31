// Shader from http://forum.unity3d.com/threads/invisible-depth-mask.122873/#post-826044
// Thanks BIG BUG

Shader "DepthMask"
{
    SubShader
    {
        Tags {"Queue" = "Geometry-1" }
        Lighting Off
        Pass
        {
            ZWrite On
            ZTest LEqual
            ColorMask 0    
        }
    }
}