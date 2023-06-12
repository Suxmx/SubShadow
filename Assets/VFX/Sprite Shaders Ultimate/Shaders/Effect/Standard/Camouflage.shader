// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sprite Shaders Ultimate/Standard/Effect/Camouflage"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[KeywordEnum(UV,UV_Raw,Object,Object_Scaled,World,UI_Element,Screen)] _ShaderSpace("Shader Space", Float) = 0
		_PixelsPerUnit("Pixels Per Unit", Float) = 100
		_ScreenWidthUnits("Screen Width Units", Float) = 10
		_RectWidth("Rect Width", Float) = 100
		_RectHeight("Rect Height", Float) = 100
		[KeywordEnum(Linear_Default,Linear_Scaled,Linear_FPS,Frequency,Frequency_FPS,Custom_Value)] _TimeSettings("Time Settings", Float) = 0
		_TimeScale("Time Scale", Float) = 1
		_TimeFrequency("Time Frequency", Float) = 2
		_TimeRange("Time Range", Float) = 0.5
		_TimeFPS("Time FPS", Float) = 5
		_TimeValue("Time Value", Float) = 0
		_CamouflageFade("Camouflage: Fade", Range( 0 , 1)) = 1
		[NoScaleOffset]_CamouflageShaderMask("Camouflage: Shader Mask", 2D) = "white" {}
		_CamouflageBaseColor("Camouflage: Base Color", Color) = (0.7450981,0.7254902,0.5686275,0)
		_CamouflageContrast("Camouflage: Contrast", Float) = 1
		_CamouflageColorA("Camouflage: Color A", Color) = (0.627451,0.5882353,0.4313726,0)
		_CamouflageDensityA("Camouflage: Density A", Range( 0 , 1)) = 0.4
		_CamouflageSmoothnessA("Camouflage: Smoothness A", Range( 0 , 1)) = 0.2
		_CamouflageNoiseScaleA("Camouflage: Noise Scale A", Vector) = (0.25,0.25,0,0)
		_CamouflageColorB("Camouflage: Color B", Color) = (0.4705882,0.4313726,0.3137255,0)
		_CamouflageDensityB("Camouflage: Density B", Range( 0 , 1)) = 0.4
		_CamouflageSmoothnessB("Camouflage: Smoothness B", Range( 0 , 1)) = 0.2
		_CamouflageNoiseScaleB("Camouflage: Noise Scale B", Vector) = (0.25,0.25,0,0)
		[Toggle]_CamouflageAnimated("Camouflage: Animated", Float) = 0
		_CamouflageAnimationSpeed("Camouflage: Animation Speed", Vector) = (0.1,0.1,0,0)
		_CamouflageDistortionIntensity("Camouflage: Distortion Intensity", Vector) = (0.1,0.1,0,0)
		_CamouflageDistortionScale("Camouflage: Distortion Scale", Vector) = (0.5,0.5,0,0)
		[ASEEnd]_CamouflageNoiseTexture("Camouflage: Noise Texture", 2D) = "white" {}
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
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_POSITION
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local _SHADERSPACE_UV _SHADERSPACE_UV_RAW _SHADERSPACE_OBJECT _SHADERSPACE_OBJECT_SCALED _SHADERSPACE_WORLD _SHADERSPACE_UI_ELEMENT _SHADERSPACE_SCREEN
			#pragma shader_feature _TIMESETTINGS_LINEAR_DEFAULT _TIMESETTINGS_LINEAR_SCALED _TIMESETTINGS_LINEAR_FPS _TIMESETTINGS_FREQUENCY _TIMESETTINGS_FREQUENCY_FPS _TIMESETTINGS_CUSTOM_VALUE


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
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _MainTex_ST;
			uniform float4 _CamouflageBaseColor;
			uniform float4 _CamouflageColorA;
			uniform float _CamouflageDensityA;
			uniform sampler2D _CamouflageNoiseTexture;
			uniform float _CamouflageAnimated;
			uniform float _PixelsPerUnit;
			float4 _MainTex_TexelSize;
			uniform float _RectWidth;
			uniform float _RectHeight;
			uniform float _ScreenWidthUnits;
			uniform float _TimeScale;
			uniform float _TimeFPS;
			uniform float _TimeFrequency;
			uniform float _TimeRange;
			uniform float _TimeValue;
			uniform float2 _CamouflageAnimationSpeed;
			uniform float2 _CamouflageDistortionScale;
			uniform float2 _CamouflageDistortionIntensity;
			uniform float2 _CamouflageNoiseScaleA;
			uniform float _CamouflageSmoothnessA;
			uniform float4 _CamouflageColorB;
			uniform float _CamouflageDensityB;
			uniform float2 _CamouflageNoiseScaleB;
			uniform float _CamouflageSmoothnessB;
			uniform float _CamouflageContrast;
			uniform float _CamouflageFade;
			uniform sampler2D _CamouflageShaderMask;
			uniform float4 _CamouflageShaderMask_ST;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				float3 ase_worldPos = mul(unity_ObjectToWorld, IN.vertex).xyz;
				OUT.ase_texcoord2.xyz = ase_worldPos;
				float4 ase_clipPos = UnityObjectToClipPos(IN.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				OUT.ase_texcoord3 = screenPos;
				
				OUT.ase_texcoord1 = IN.vertex;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				OUT.ase_texcoord2.w = 0;
				
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
				float4 temp_output_1_0_g310 = tex2D( _MainTex, uv_MainTex );
				float2 texCoord2_g301 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 texCoord22_g301 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float3 ase_objectScale = float3( length( unity_ObjectToWorld[ 0 ].xyz ), length( unity_ObjectToWorld[ 1 ].xyz ), length( unity_ObjectToWorld[ 2 ].xyz ) );
				float3 ase_worldPos = IN.ase_texcoord2.xyz;
				float2 texCoord23_g301 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult28_g301 = (float2(_RectWidth , _RectHeight));
				float4 screenPos = IN.ase_texcoord3;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				#if defined(_SHADERSPACE_UV)
				float2 staticSwitch1_g301 = ( texCoord2_g301 / ( _PixelsPerUnit * (_MainTex_TexelSize).xy ) );
				#elif defined(_SHADERSPACE_UV_RAW)
				float2 staticSwitch1_g301 = texCoord22_g301;
				#elif defined(_SHADERSPACE_OBJECT)
				float2 staticSwitch1_g301 = (IN.ase_texcoord1.xyz).xy;
				#elif defined(_SHADERSPACE_OBJECT_SCALED)
				float2 staticSwitch1_g301 = ( (IN.ase_texcoord1.xyz).xy * (ase_objectScale).xy );
				#elif defined(_SHADERSPACE_WORLD)
				float2 staticSwitch1_g301 = (ase_worldPos).xy;
				#elif defined(_SHADERSPACE_UI_ELEMENT)
				float2 staticSwitch1_g301 = ( texCoord23_g301 * ( appendResult28_g301 / _PixelsPerUnit ) );
				#elif defined(_SHADERSPACE_SCREEN)
				float2 staticSwitch1_g301 = ( ( (ase_screenPosNorm).xy * (_ScreenParams).xy ) / ( _ScreenParams.x / _ScreenWidthUnits ) );
				#else
				float2 staticSwitch1_g301 = ( texCoord2_g301 / ( _PixelsPerUnit * (_MainTex_TexelSize).xy ) );
				#endif
				float2 temp_output_99_0_g310 = staticSwitch1_g301;
				float mulTime5_g435 = _Time.y * _TimeScale;
				float mulTime7_g435 = _Time.y * _TimeFrequency;
				#if defined(_TIMESETTINGS_LINEAR_DEFAULT)
				float staticSwitch1_g435 = _Time.y;
				#elif defined(_TIMESETTINGS_LINEAR_SCALED)
				float staticSwitch1_g435 = mulTime5_g435;
				#elif defined(_TIMESETTINGS_LINEAR_FPS)
				float staticSwitch1_g435 = ( _TimeScale * ( floor( ( _Time.y * _TimeFPS ) ) / _TimeFPS ) );
				#elif defined(_TIMESETTINGS_FREQUENCY)
				float staticSwitch1_g435 = ( ( sin( mulTime7_g435 ) * _TimeRange ) + 100.0 );
				#elif defined(_TIMESETTINGS_FREQUENCY_FPS)
				float staticSwitch1_g435 = ( ( _TimeRange * sin( ( _TimeFrequency * ( floor( ( _TimeFPS * _Time.y ) ) / _TimeFPS ) ) ) ) + 100.0 );
				#elif defined(_TIMESETTINGS_CUSTOM_VALUE)
				float staticSwitch1_g435 = _TimeValue;
				#else
				float staticSwitch1_g435 = _Time.y;
				#endif
				float clampResult52_g310 = clamp( ( ( _CamouflageDensityA - tex2D( _CamouflageNoiseTexture, ( (( _CamouflageAnimated )?( ( ( ( tex2D( _CamouflageNoiseTexture, ( ( ( staticSwitch1_g435 * _CamouflageAnimationSpeed ) + temp_output_99_0_g310 ) * _CamouflageDistortionScale ) ).r - 0.25 ) * _CamouflageDistortionIntensity ) + temp_output_99_0_g310 ) ):( temp_output_99_0_g310 )) * _CamouflageNoiseScaleA ) ).r ) / max( _CamouflageSmoothnessA , 0.005 ) ) , 0.0 , 1.0 );
				float4 lerpResult55_g310 = lerp( _CamouflageBaseColor , ( _CamouflageColorA * clampResult52_g310 ) , clampResult52_g310);
				float clampResult65_g310 = clamp( ( ( _CamouflageDensityB - tex2D( _CamouflageNoiseTexture, ( ( (( _CamouflageAnimated )?( ( ( ( tex2D( _CamouflageNoiseTexture, ( ( ( staticSwitch1_g435 * _CamouflageAnimationSpeed ) + temp_output_99_0_g310 ) * _CamouflageDistortionScale ) ).r - 0.25 ) * _CamouflageDistortionIntensity ) + temp_output_99_0_g310 ) ):( temp_output_99_0_g310 )) + float2( 12.3,12.3 ) ) * _CamouflageNoiseScaleB ) ).r ) / max( _CamouflageSmoothnessB , 0.005 ) ) , 0.0 , 1.0 );
				float4 lerpResult68_g310 = lerp( lerpResult55_g310 , ( _CamouflageColorB * clampResult65_g310 ) , clampResult65_g310);
				float4 break2_g313 = temp_output_1_0_g310;
				float temp_output_9_0_g312 = max( _CamouflageContrast , 0.0 );
				float saferPower7_g312 = max( ( ( ( break2_g313.x + break2_g313.y + break2_g313.z ) / 3.0 ) + ( 0.1 * max( ( 1.0 - temp_output_9_0_g312 ) , 0.0 ) ) ) , 0.0001 );
				float2 uv_CamouflageShaderMask = IN.texcoord.xy * _CamouflageShaderMask_ST.xy + _CamouflageShaderMask_ST.zw;
				float4 tex2DNode3_g314 = tex2D( _CamouflageShaderMask, uv_CamouflageShaderMask );
				float3 lerpResult4_g310 = lerp( (temp_output_1_0_g310).rgb , ( (lerpResult68_g310).rgb * pow( saferPower7_g312 , temp_output_9_0_g312 ) ) , ( _CamouflageFade * ( tex2DNode3_g314.r * tex2DNode3_g314.a ) ));
				float4 appendResult7_g310 = (float4(lerpResult4_g310 , temp_output_1_0_g310.a));
				
				fixed4 c = ( appendResult7_g310 * IN.color );
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "SpriteShadersUltimate.SingleShaderGUI"
	
	
}
/*ASEBEGIN
Version=18900
0;0;1920;1019;1287.755;382.3263;1;True;True
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;85;-1008.374,-85.04341;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-736.775,33.25894;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;98;-687.7551,-97.32629;Inherit;False;ShaderSpace;0;;301;be729ef05db9c224caec82a3516038dc;0;1;3;SAMPLER2D;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;100;-657.755,-193.3263;Inherit;False;ShaderTime;6;;435;06a15e67904f217499045f361bad56e7;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;99;-385.5576,-99.26548;Inherit;False;_Camouflage;13;;310;b890169120092da40a71b337181d9fec;0;4;100;FLOAT;0;False;99;FLOAT2;0,0;False;97;SAMPLER2D;;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FunctionNode;22;-63.6362,70.99575;Inherit;False;TintVertex;-1;;309;b0b94dd27c0f3da49a89feecae766dcc;0;1;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;81;211.4418,70.685;Float;False;True;-1;2;SpriteShadersUltimate.SingleShaderGUI;0;8;Sprite Shaders Ultimate/Standard/Effect/Camouflage;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;-1;10;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;24;0;85;0
WireConnection;98;3;85;0
WireConnection;99;100;100;0
WireConnection;99;99;98;0
WireConnection;99;1;24;0
WireConnection;22;1;99;0
WireConnection;81;0;22;0
ASEEND*/
//CHKSM=48DFF613A99382892F05BEA46AE62C561BDA9DB3