// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sprite Shaders Ultimate/Standard/Color/Color Replace"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_ColorReplaceFade("Color Replace: Fade", Range( 0 , 1)) = 1
		_ColorReplaceTargetColor("Color Replace: Target Color", Color) = (0,0,0,0)
		_ColorReplaceBias("Color Replace: Bias", Float) = 1
		[HDR]_ColorReplaceColor("Color Replace: Color", Color) = (1,0,0,0)
		_ColorReplaceContrast("Color Replace: Contrast", Float) = 1
		_ColorReplaceHueTolerance("Color Replace: Hue Tolerance", Float) = 1
		_ColorReplaceSaturationTolerance("Color Replace: Saturation Tolerance", Float) = 1
		_ColorReplaceBrightnessTolerance("Color Replace: Brightness Tolerance", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_COLOR


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _MainTex_ST;
			uniform float _ColorReplaceContrast;
			uniform float4 _ColorReplaceColor;
			uniform float4 _ColorReplaceTargetColor;
			uniform float _ColorReplaceHueTolerance;
			uniform float _ColorReplaceSaturationTolerance;
			uniform float _ColorReplaceBrightnessTolerance;
			uniform float _ColorReplaceBias;
			uniform float _ColorReplaceFade;
			float3 RGBToHSV(float3 c)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 p = lerp( float4( c.bg, K.wz ), float4( c.gb, K.xy ), step( c.b, c.g ) );
				float4 q = lerp( float4( p.xyw, c.r ), float4( c.r, p.yzx ), step( p.x, c.r ) );
				float d = q.x - min( q.w, q.y );
				float e = 1.0e-10;
				return float3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 temp_output_1_0_g70 = tex2D( _MainTex, uv_MainTex );
				float3 temp_output_2_0_g70 = (temp_output_1_0_g70).rgb;
				float4 break2_g71 = float4( temp_output_2_0_g70 , 0.0 );
				float temp_output_9_0_g72 = max( _ColorReplaceContrast , 0.0 );
				float saferPower7_g72 = max( ( ( ( break2_g71.x + break2_g71.y + break2_g71.z ) / 3.0 ) + ( 0.1 * max( ( 1.0 - temp_output_9_0_g72 ) , 0.0 ) ) ) , 0.0001 );
				float3 hsvTorgb7_g70 = RGBToHSV( (_ColorReplaceTargetColor).rgb );
				float3 hsvTorgb5_g70 = RGBToHSV( temp_output_2_0_g70 );
				float clampResult35_g70 = clamp( ( 1.0 - ( min( min( distance( hsvTorgb7_g70.x , hsvTorgb5_g70.x ) , distance( hsvTorgb7_g70.x , ( hsvTorgb5_g70.x + 1.0 ) ) ) , distance( hsvTorgb7_g70.x , ( hsvTorgb5_g70.x + -1.0 ) ) ) / max( ( _ColorReplaceHueTolerance * 0.15 ) , 0.001 ) ) ) , 0.0 , 1.0 );
				float clampResult30_g70 = clamp( ( 1.0 - ( distance( hsvTorgb7_g70.y , hsvTorgb5_g70.y ) / max( ( _ColorReplaceSaturationTolerance * 2.0 ) , 0.001 ) ) ) , 0.0 , 1.0 );
				float clampResult40_g70 = clamp( ( 1.0 - ( distance( hsvTorgb7_g70.z , hsvTorgb5_g70.z ) / max( ( _ColorReplaceBrightnessTolerance * 1.5 ) , 0.001 ) ) ) , 0.0 , 1.0 );
				float saferPower48_g70 = max( ( clampResult35_g70 * clampResult30_g70 * clampResult40_g70 ) , 0.0001 );
				float3 lerpResult23_g70 = lerp( temp_output_2_0_g70 , ( pow( saferPower7_g72 , temp_output_9_0_g72 ) * (_ColorReplaceColor).rgb ) , ( pow( saferPower48_g70 , max( _ColorReplaceBias , 0.001 ) ) * _ColorReplaceFade ));
				float4 appendResult4_g70 = (float4(lerpResult23_g70 , temp_output_1_0_g70.a));
				
				fixed4 c = ( appendResult4_g70 * IN.color );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "SpriteShadersUltimate.SingleShaderGUI"
	
	
}
/*ASEBEGIN
Version=18800
112;331;1543;878;2336.692;399.6608;2.774485;True;True
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;29;-989.5579,-16.30571;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-687.5431,30.52385;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;69;-276.5068,5.056967;Inherit;False;_ColorReplace;0;;70;626c705a4346bf34c8da30de12f2c574;0;1;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;22;19.08116,55.61475;Inherit;False;TintVertex;-1;;19;b0b94dd27c0f3da49a89feecae766dcc;0;1;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;14;309.1169,40.40625;Float;False;True;-1;2;SpriteShadersUltimate.SingleShaderGUI;0;8;Sprite Shaders Ultimate/Standard/Color/Color Replace;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;24;0;29;0
WireConnection;69;1;24;0
WireConnection;22;1;69;0
WireConnection;14;0;22;0
ASEEND*/
//CHKSM=DAA1F2E7C8273E45545F7060E9EE9C80A2C0BB98