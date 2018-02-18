Shader "Custom/LocalProjection" {
  Properties {
    _Color ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    [NoScaleOffset]_MainTex ("Noise Texture", 2D) = "white" {}
  }

  SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
    Pass {
      Stencil{
                Ref 200
                Comp greater
                Pass keep
            }

      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
    
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      
      struct appdata_t {
        float4 vertex : POSITION;
        fixed4 color : COLOR;
        fixed3 normal : NORMAL;
        UNITY_VERTEX_INPUT_INSTANCE_ID
      };

      struct v2f
      {
        float4 pos : SV_POSITION;
        half2 uv : TEXCOORD0;
        float4 screenUV : TEXCOORD1;
        float3 ray : TEXCOORD2;
        UNITY_VERTEX_INPUT_INSTANCE_ID
      };
      

      v2f vert (appdata_t v)
      {
        v2f o;
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_TRANSFER_INSTANCE_ID (v, o);
        
        o.pos = UnityObjectToClipPos (v.vertex);
        o.uv = v.vertex.xz+0.5;
        o.screenUV = ComputeScreenPos (o.pos);
        o.ray = UnityObjectToViewPos(float4(v.vertex)).xyz * float3(-1,-1,1);
        return o;
      }

      sampler2D_float _CameraDepthTexture;
      sampler2D _CameraDepthNormalsTexture;
      fixed4 _Color;
      sampler2D _MainTex;
      uniform float4x4 _CameraMV;
      
      fixed4 frag (v2f i) : SV_Target
      {
        i.ray = i.ray * (_ProjectionParams.z / i.ray.z);
        float2 uv = i.screenUV.xy / i.screenUV.w;
        // read depth and reconstruct world position
        float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
        depth = Linear01Depth (depth);
        float4 vpos = float4(i.ray * depth,1);
        float3 wpos = mul (unity_CameraToWorld, vpos).xyz;
        float3 opos = mul (unity_WorldToObject, float4(wpos,1)).xyz;


        clip(float3(0.5, 0.5, 0.5) - abs(opos));

        float4 dn = tex2D(_CameraDepthNormalsTexture, uv);
        float3 n = DecodeViewNormalStereo(dn);

        float3 wNorm = mul((float3x3)_CameraMV, n).xyz;

        _Color.a *= smoothstep(0.25, 0.5, saturate(dot(float3(0, 1, 0), wNorm)));

        return tex2D(_MainTex, opos.xz + float2(0.5, 0.5)) * _Color;
      }
      ENDCG 
    }
  }
}