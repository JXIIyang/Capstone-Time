// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ShadowShadow"
{
	Properties
	{
		_MainColor("Main Color", Color) = (0,0,0,0)
		_N0R1("N[0] R[1]", Int) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float3 worldPos;
		};

		uniform float4 _MainColor;
		uniform int _N0R1;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			o.Emission = _MainColor.rgb;
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float4 transform35 = mul(unity_ObjectToWorld,float4( ase_vertex3Pos , 0.0 ));
			float clampResult19 = clamp( (0.0 + (( transform35.y + ( (-0.8 + (_SinTime.w - -1.0) * (0.2 - -0.8) / (1.0 - -1.0)) * 0.1 ) ) - -0.15) * (0.15 - 0.0) / (0.2 - -0.15)) , 0.0 , 1.0 );
			float clampResult34 = clamp( ( ( clampResult19 * -1.5 ) + 0.145 ) , 0.0 , 1.0 );
			float ifLocalVar27 = 0;
			UNITY_BRANCH 
			if( _N0R1 == 0 )
				ifLocalVar27 = clampResult19;
			else
				ifLocalVar27 = clampResult34;
			o.Alpha = ifLocalVar27;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
7;6;1906;1013;2490.393;771.3163;1.6;True;False
Node;AmplifyShaderEditor.SinTimeNode;21;-1835.018,-19.96565;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;23;-1633.539,17.59843;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;-0.8;False;4;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;2;-1937.429,-449.4418;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;35;-1659.399,-533.2181;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-1416.57,-104.7207;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;20;-1166.223,-352.9934;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;18;-809.6741,-189.3629;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-0.15;False;2;FLOAT;0.2;False;3;FLOAT;0;False;4;FLOAT;0.15;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;19;-527.4898,-106.7149;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-302.445,-43.83617;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-172.6995,-44.85781;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.145;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;28;-533.2074,-292.5679;Inherit;False;Property;_N0R1;N[0] R[1];3;0;Create;True;0;0;True;0;0;0;0;1;INT;0
Node;AmplifyShaderEditor.ClampOpNode;34;-61.34306,-40.77131;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;29;-355.5536,67.10852;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-705.6819,426.84;Inherit;False;Property;_YDisapprear;Y Disapprear;2;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-806.1564,577.6855;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;27;34.63984,-220.4046;Inherit;False;True;5;0;INT;0;False;1;INT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-575.4007,635.8983;Inherit;True;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;12;-568.3274,516.366;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-651.03,251.2632;Inherit;False;Property;_YDissolve;Y Dissolve;1;0;Create;True;0;0;False;0;0.5;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;1;-271,-354.5;Inherit;False;Property;_MainColor;Main Color;0;0;Create;True;0;0;False;0;0,0,0,0;0.7254902,0.509804,0.509804,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;13;-1016.78,675.4126;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;3;-211.03,184.2632;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;8;-319.3712,375.3317;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-556.03,355.2632;Inherit;False;Constant;_Float2;Float 2;2;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;307.373,-316.5651;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;ShadowShadow;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;1;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;21;4
WireConnection;35;0;2;0
WireConnection;22;0;23;0
WireConnection;20;0;35;2
WireConnection;20;1;22;0
WireConnection;18;0;20;0
WireConnection;19;0;18;0
WireConnection;31;0;19;0
WireConnection;33;0;31;0
WireConnection;34;0;33;0
WireConnection;29;0;19;0
WireConnection;14;0;35;1
WireConnection;14;1;13;0
WireConnection;27;0;28;0
WireConnection;27;2;34;0
WireConnection;27;3;19;0
WireConnection;27;4;34;0
WireConnection;12;0;14;0
WireConnection;3;0;35;2
WireConnection;3;1;4;0
WireConnection;3;2;6;0
WireConnection;3;3;6;0
WireConnection;3;4;8;0
WireConnection;8;0;35;2
WireConnection;8;1;9;0
WireConnection;8;2;12;0
WireConnection;8;3;12;0
WireConnection;8;4;5;0
WireConnection;0;2;1;0
WireConnection;0;9;27;0
ASEEND*/
//CHKSM=4EFBB81778C060929FCC00018582CFE1279BDBD1