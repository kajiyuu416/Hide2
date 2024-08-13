Shader "Custom/TransparentShader"
{
    //プロパティを設定することで、カラーの変更を行う
    Properties
    {
        _Color("Color", Color) = (0.5,0.5,0.5,0.1)
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}