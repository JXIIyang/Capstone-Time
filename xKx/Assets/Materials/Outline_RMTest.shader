// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Outline_RMTest"
{
	Properties
	{
		_FresnelScale("Fresnel Scale", Range( 0 , 100)) = 70
		_FresnelBias("Fresnel Bias", Range( -1 , 1)) = 0
		_FresnelPower("Fresnel Power", Range( 0 , 10)) = 2
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float3 worldPos;
			float3 viewDir;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 color87 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			o.Albedo = color87.rgb;
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV80 = dot( ase_worldNormal, i.viewDir );
			float fresnelNode80 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV80, _FresnelPower ) );
			float3 temp_cast_1 = (fresnelNode80).xxx;
			o.Emission = temp_cast_1;
			o.Alpha = fresnelNode80;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
407;732;1634;1327;491.9135;546.9635;1.295086;True;False
Node;AmplifyShaderEditor.WorldNormalVector;81;41.62236,-276.3266;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;82;56.1157,-164.8391;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;84;-37.53376,18.00039;Inherit;False;Property;_FresnelScale;Fresnel Scale;0;0;Create;True;0;0;False;0;70;70;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;5.946313,109.4201;Inherit;False;Property;_FresnelPower;Fresnel Power;2;0;Create;True;0;0;False;0;2;2;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;42.73723,-44.43262;Inherit;False;Property;_FresnelBias;Fresnel Bias;1;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;80;337.0642,-161.4945;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;79;138.6164,-413.4561;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;78;504.2954,-413.4562;Inherit;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CeilOpNode;86;708.3175,-52.23672;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;87;699.5654,-480.9141;Inherit;False;Constant;_Color0;Color 0;0;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;49;952.8693,-363.8212;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Outline_RMTest;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.1;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;80;0;81;0
WireConnection;80;4;82;0
WireConnection;80;1;83;0
WireConnection;80;2;84;0
WireConnection;80;3;85;0
WireConnection;78;0;79;0
WireConnection;86;0;80;0
WireConnection;49;0;87;0
WireConnection;49;2;80;0
WireConnection;49;9;80;0
ASEEND*/
//CHKSM=612079038DE876A8F74EE650F10DA9EB8BE72901