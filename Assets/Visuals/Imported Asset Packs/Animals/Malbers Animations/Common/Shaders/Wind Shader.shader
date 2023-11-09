// Made with Amplify Shader Editor v1.9.1.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Malbers/Wind"
{
	Properties
	{
		[NoScaleOffset]_Pattern("Pattern", 2D) = "white" {}
		[NoScaleOffset]_Gradient("Gradient", 2D) = "white" {}
		_WindColor1("Wind Color 1", Color) = (0.7924528,0.6378148,0.3999644,0)
		_Tillling1("Tillling 1", Vector) = (1,1,0,0)
		_Tilling2("Tilling 2", Vector) = (2,2,0,0)
		_Speed1("Speed 1", Vector) = (0.2,0.2,0,0)
		_Speed2("Speed 2", Vector) = (0.2,0,0,0)
		_Opacity("Opacity", Range( 0 , 1)) = 0.75
		_Volume("Volume", Range( 0 , 5)) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap noforwardadd 
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float _Volume;
		uniform sampler2D _Pattern;
		uniform float2 _Speed1;
		uniform float4 _Pattern_ST;
		uniform float2 _Tillling1;
		uniform float2 _Speed2;
		uniform float2 _Tilling2;
		uniform float _Opacity;
		uniform sampler2D _Gradient;
		uniform float4 _WindColor1;


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV34 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode34 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV34, 1.0 ) );
			float temp_output_49_0 = ( 1.0 - ( fresnelNode34 + 0.0 ) );
			float4 temp_cast_0 = (( temp_output_49_0 * temp_output_49_0 * temp_output_49_0 )).xxxx;
			float2 uv_Pattern = i.uv_texcoord * _Pattern_ST.xy + _Pattern_ST.zw;
			float2 panner4 = ( 1.0 * _Time.y * _Speed1 + ( uv_Pattern * _Tillling1 ));
			float4 Noise128 = tex2D( _Pattern, panner4 );
			float2 panner18 = ( 1.0 * _Time.y * _Speed2 + ( uv_Pattern * _Tilling2 ));
			float4 Noise227 = tex2D( _Pattern, panner18 );
			float2 uv_Gradient95 = i.uv_texcoord;
			float4 temp_output_68_0 = ( CalculateContrast(_Volume,temp_cast_0) * ( ( Noise128 * Noise227 ) * _Opacity * tex2D( _Gradient, uv_Gradient95 ) ) );
			float4 blendOpSrc80 = temp_output_68_0;
			float4 blendOpDest80 = _WindColor1;
			o.Emission = ( saturate( 2.0f*blendOpDest80*blendOpSrc80 + blendOpDest80*blendOpDest80*(1.0f - 2.0f*blendOpSrc80) )).rgb;
			o.Alpha = temp_output_68_0.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19103
Node;AmplifyShaderEditor.CommentaryNode;115;-985.1505,-548.4113;Inherit;False;1413.752;814.6819;Comment;14;27;78;18;20;104;19;76;28;77;4;5;105;3;21;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-969.6968,-297.4472;Inherit;False;0;76;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;3;-942.8373,-469.4958;Float;False;Property;_Tillling1;Tillling 1;3;0;Create;True;0;0;0;False;0;False;1,1;1,0.09;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;19;-911.2415,2.527771;Float;False;Property;_Tilling2;Tilling 2;4;0;Create;True;0;0;0;False;0;False;2,2;2,0.24;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;-771.3392,-466.4333;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;5;-637.4875,-336.1215;Float;False;Property;_Speed1;Speed 1;5;0;Create;True;0;0;0;False;0;False;0.2,0.2;-0.05,-0.06;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-665.2872,-10.63413;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;20;-681.2475,119.4916;Float;False;Property;_Speed2;Speed 2;6;0;Create;True;0;0;0;False;0;False;0.2,0;0.03,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;4;-453.2163,-474.7483;Inherit;True;3;0;FLOAT2;1,1;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;76;-434.463,-211.9266;Float;True;Property;_Pattern;Pattern;0;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;None;b44ea59463ef6d941872d19b3faf784d;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;18;-443.85,2.36969;Inherit;True;3;0;FLOAT2;1,1;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;34;-995.988,351.9596;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;77;-168.5467,-469.2143;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;78;-131.5816,-94.30083;Inherit;True;Property;_TextureSample1;Texture Sample 1;9;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Instance;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-682.2303,357.1205;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;176.4551,-80.68872;Float;False;Noise2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;28;135.1404,-464.9338;Float;False;Noise1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;49;-529.9689,346.5976;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;-800.4556,638.4846;Inherit;False;28;Noise1;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;-798.1633,720.3222;Inherit;False;27;Noise2;1;0;OBJECT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;94;-330.0337,607.5114;Float;False;Property;_Volume;Volume;8;0;Create;True;0;0;0;False;0;False;2;3.64;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-272.9473,338.1209;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-553.457,642.7834;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-415.7929,726.8705;Float;False;Property;_Opacity;Opacity;7;0;Create;True;0;0;0;False;0;False;0.75;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;95;-530.0156,817.3436;Inherit;True;Property;_Gradient;Gradient;1;1;[NoScaleOffset];Create;True;0;0;0;False;0;False;-1;None;5a65cb47d4ad68f4cb5c6c7561ce4155;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;93;-29.05303,354.1356;Inherit;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-51.61368,706.2062;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;10;294.2161,684.3468;Float;False;Property;_WindColor1;Wind Color 1;2;0;Create;True;0;0;0;False;0;False;0.7924528,0.6378148,0.3999644,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;80;423.9652,343.7705;Inherit;True;SoftLight;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;195.984,540.7802;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;8;791.6617,386.6082;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Malbers/Wind;False;False;False;False;True;True;True;True;True;False;False;True;False;False;True;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;;10;False;;2;5;False;;10;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;105;0;21;0
WireConnection;105;1;3;0
WireConnection;104;0;21;0
WireConnection;104;1;19;0
WireConnection;4;0;105;0
WireConnection;4;2;5;0
WireConnection;18;0;104;0
WireConnection;18;2;20;0
WireConnection;77;0;76;0
WireConnection;77;1;4;0
WireConnection;78;0;76;0
WireConnection;78;1;18;0
WireConnection;55;0;34;0
WireConnection;27;0;78;0
WireConnection;28;0;77;0
WireConnection;49;0;55;0
WireConnection;60;0;49;0
WireConnection;60;1;49;0
WireConnection;60;2;49;0
WireConnection;23;0;29;0
WireConnection;23;1;30;0
WireConnection;93;1;60;0
WireConnection;93;0;94;0
WireConnection;58;0;23;0
WireConnection;58;1;59;0
WireConnection;58;2;95;0
WireConnection;80;0;68;0
WireConnection;80;1;10;0
WireConnection;68;0;93;0
WireConnection;68;1;58;0
WireConnection;8;2;80;0
WireConnection;8;9;68;0
ASEEND*/
//CHKSM=FDC4594750FD7F13EF25D0A51303695DFCFF6821