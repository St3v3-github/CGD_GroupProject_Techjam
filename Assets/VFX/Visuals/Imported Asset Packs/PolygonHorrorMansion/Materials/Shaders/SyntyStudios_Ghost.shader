// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/Ghost"
{
	Properties
	{
		_RimColor("RimColor", Color) = (0,0,0,0)
		_RimPower("RimPower", Range( 0 , 10)) = 0
		_Normals("Normals", 2D) = "bump" {}
		_BaseColour("BaseColour", Color) = (0,0.5448275,1,0)
		_Transparency("Transparency", Range( 0 , 1)) = 0.7519556
		_Metalic("Metalic", Range( 0 , 1)) = 1
		[Toggle(_FADE_TOGGLE_ON)] _Fade_Toggle("Fade_Toggle", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma shader_feature _FADE_TOGGLE_ON
		#pragma surface surf Standard alpha:fade keepalpha noshadow exclude_path:deferred noambient 
		struct Input
		{
			float2 uv_texcoord;
			float3 viewDir;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform float4 _BaseColour;
		uniform sampler2D _Normals;
		uniform float4 _Normals_ST;
		uniform float _RimPower;
		uniform float4 _RimColor;
		uniform float _Metalic;
		uniform float _Transparency;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			o.Albedo = _BaseColour.rgb;
			float2 uv_Normals = i.uv_texcoord * _Normals_ST.xy + _Normals_ST.zw;
			float dotResult21 = dot( UnpackNormal( tex2D( _Normals, uv_Normals ) ) , i.viewDir );
			o.Emission = ( pow( ( 1.0 - saturate( dotResult21 ) ) , _RimPower ) * _RimColor ).rgb;
			o.Metallic = _Metalic;
			float3 ase_worldPos = i.worldPos;
			float4 transform45 = mul(unity_WorldToObject,float4( ase_worldPos , 0.0 ));
			#ifdef _FADE_TOGGLE_ON
				float staticSwitch51 = _Transparency;
			#else
				float staticSwitch51 = ( (abs( transform45 )).y * _Transparency );
			#endif
			o.Alpha = staticSwitch51;
		}

		ENDCG
	}
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
-3065;368;3058;845;2310.32;-315.2244;1.3;True;True
Node;AmplifyShaderEditor.WorldPosInputsNode;44;-1083,885.6724;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;3;-1610.49,129.6406;Inherit;True;Property;_Normals;Normals;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;0,0;False;1;FLOAT2;1,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;22;-1642.992,428.9419;Float;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldToObjectTransfNode;45;-834.9944,909.1722;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DotProductOpNode;21;-1186.994,242.1411;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;46;-594.1953,927.8583;Inherit;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SaturateNode;20;-973.493,225.3406;Inherit;True;1;0;FLOAT;1.23;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;48;-363.1953,914.8583;Inherit;False;False;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-516.6855,1152.01;Inherit;False;Property;_Transparency;Transparency;4;0;Create;True;0;0;0;False;0;False;0.7519556;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;5;-792.8896,223.2398;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-916.4914,490.6421;Float;False;Property;_RimPower;RimPower;1;0;Create;True;0;0;0;False;0;False;0;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;-25.1953,902.8583;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;26;-604.0919,231.6406;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;-613.2914,542.4415;Float;False;Property;_RimColor;RimColor;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-314.6579,612.0264;Inherit;False;Property;_Metalic;Metalic;5;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;51;275.3799,1001.624;Inherit;False;Property;_Fade_Toggle;Fade_Toggle;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-299.7596,-145.1712;Inherit;False;Property;_BaseColour;BaseColour;3;0;Create;True;0;0;0;False;0;False;0,0.5448275,1,0;0.1441393,0.4101315,0.6323529,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-343.392,253.8403;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;147.2,339.6;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SyntyStudios/Ghost;False;False;False;False;True;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;0;4;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;5;False;-1;10;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;44;0
WireConnection;21;0;3;0
WireConnection;21;1;22;0
WireConnection;46;0;45;0
WireConnection;20;0;21;0
WireConnection;48;0;46;0
WireConnection;5;0;20;0
WireConnection;50;0;48;0
WireConnection;50;1;33;0
WireConnection;26;0;5;0
WireConnection;26;1;28;0
WireConnection;51;1;50;0
WireConnection;51;0;33;0
WireConnection;27;0;26;0
WireConnection;27;1;25;0
WireConnection;0;0;29;0
WireConnection;0;2;27;0
WireConnection;0;3;31;0
WireConnection;0;9;51;0
ASEEND*/
//CHKSM=9C89FDD92D54FC0A4FA1D4A27F5629D919F034AF