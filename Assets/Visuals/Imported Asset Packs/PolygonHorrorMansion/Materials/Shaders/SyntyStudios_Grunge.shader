// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SyntyStudios/GrungeTriplanar"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Normal_Map("Normal_Map", 2D) = "bump" {}
		_Grunge("Grunge", 2D) = "white" {}
		_Large_Grunge("Large_Grunge", 2D) = "white" {}
		_SmoothnessSpec("Smoothness(Spec)", Range( 0 , 1)) = 0.1962713
		_Metallic("Metallic", Range( 0 , 1)) = 0
		_Dust_Colour("Dust_Colour", Color) = (0.4558824,0.4196799,0.3653763,0)
		_Dust_Amount("Dust_Amount", Range( 0 , 1.2)) = 1
		_Dust_FallOff("Dust_FallOff", Range( 0 , 100)) = 14.11765
		_Dirt_Amount("Dirt_Amount", Range( 0 , 1.2)) = 1
		_Dirt_Tiling("Dirt_Tiling", Float) = 0
		_Dirt_Falloff("Dirt_Falloff", Range( 0 , 100)) = 14.11765
		_Large_Tiling("Large_Tiling", Float) = 0
		_Large_FallOff("Large_FallOff", Range( 0 , 100)) = 14.11765
		[HideInInspector]_Black("Black", 2D) = "black" {}
		_Large_DirtAmount("Large_DirtAmount", Range( 0 , 1.2)) = 1
		[HideInInspector]_Mask("Mask", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Normal_Map;
		uniform float4 _Normal_Map_ST;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float4 _Dust_Colour;
		uniform sampler2D _Mask;
		uniform sampler2D _Black;
		uniform float _Dust_FallOff;
		uniform float _Dust_Amount;
		uniform sampler2D _Grunge;
		uniform float _Dirt_Tiling;
		uniform float _Dirt_Falloff;
		uniform float _Dirt_Amount;
		uniform sampler2D _Large_Grunge;
		uniform float _Large_Tiling;
		uniform float _Large_FallOff;
		uniform float _Large_DirtAmount;
		uniform float _Metallic;
		uniform float _SmoothnessSpec;


		inline float4 TriplanarSampling17( sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			float negProjNormalY = max( 0, projNormal.y * -nsign.y );
			projNormal.y = max( 0, projNormal.y * nsign.y );
			half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
			xNorm  = tex2D( midTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm  = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			yNormN = tex2D( botTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm  = tex2D( midTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling16( sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			float negProjNormalY = max( 0, projNormal.y * -nsign.y );
			projNormal.y = max( 0, projNormal.y * nsign.y );
			half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
			xNorm  = tex2D( midTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm  = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			yNormN = tex2D( botTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm  = tex2D( midTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
		}


		inline float4 TriplanarSampling27( sampler2D topTexMap, sampler2D midTexMap, sampler2D botTexMap, float3 worldPos, float3 worldNormal, float falloff, float2 tiling, float3 normalScale, float3 index )
		{
			float3 projNormal = ( pow( abs( worldNormal ), falloff ) );
			projNormal /= ( projNormal.x + projNormal.y + projNormal.z ) + 0.00001;
			float3 nsign = sign( worldNormal );
			float negProjNormalY = max( 0, projNormal.y * -nsign.y );
			projNormal.y = max( 0, projNormal.y * nsign.y );
			half4 xNorm; half4 yNorm; half4 yNormN; half4 zNorm;
			xNorm  = tex2D( midTexMap, tiling * worldPos.zy * float2(  nsign.x, 1.0 ) );
			yNorm  = tex2D( topTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			yNormN = tex2D( botTexMap, tiling * worldPos.xz * float2(  nsign.y, 1.0 ) );
			zNorm  = tex2D( midTexMap, tiling * worldPos.xy * float2( -nsign.z, 1.0 ) );
			return xNorm * projNormal.x + yNorm * projNormal.y + yNormN * negProjNormalY + zNorm * projNormal.z;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal_Map = i.uv_texcoord * _Normal_Map_ST.xy + _Normal_Map_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Normal_Map, uv_Normal_Map ) );
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float4 triplanar17 = TriplanarSampling17( _Mask, _Black, _Black, ase_worldPos, ase_worldNormal, _Dust_FallOff, float2( 1,1 ), float3( 1,1,1 ), float3(0,0,0) );
			float clampResult22 = clamp( triplanar17.w , 0.0 , _Dust_Amount );
			float4 lerpResult18 = lerp( tex2D( _Texture, uv_Texture ) , _Dust_Colour , clampResult22);
			float2 temp_cast_0 = (_Dirt_Tiling).xx;
			float4 triplanar16 = TriplanarSampling16( _Grunge, _Grunge, _Grunge, ase_worldPos, ase_worldNormal, _Dirt_Falloff, temp_cast_0, float3( 1,1,1 ), float3(0,0,0) );
			float4 temp_cast_1 = (_Dirt_Amount).xxxx;
			float4 clampResult7 = clamp( triplanar16 , temp_cast_1 , float4( 1,1,1,0 ) );
			float4 temp_cast_2 = 1;
			float2 temp_cast_3 = (_Large_Tiling).xx;
			float4 triplanar27 = TriplanarSampling27( _Large_Grunge, _Large_Grunge, _Large_Grunge, ase_worldPos, ase_worldNormal, _Large_FallOff, temp_cast_3, float3( 1,1,1 ), float3(0,0,0) );
			float4 temp_cast_4 = (_Large_DirtAmount).xxxx;
			float4 clampResult31 = clamp( triplanar27 , temp_cast_4 , float4( 1,1,1,0 ) );
			float4 lerpResult33 = lerp( clampResult7 , temp_cast_2 , clampResult31);
			o.Albedo = ( lerpResult18 * lerpResult33 ).rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _SmoothnessSpec;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
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
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
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
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18909
-2285;411;2290;851;2809.233;659.3567;1.710568;True;True
Node;AmplifyShaderEditor.RangedFloatNode;1;-1208.956,-440.3828;Float;False;Property;_Dirt_Tiling;Dirt_Tiling;10;0;Create;True;0;0;0;False;0;False;0;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;8;-1166.413,-48;Float;True;Property;_Mask;Mask;16;1;[HideInInspector];Create;True;0;0;0;True;0;False;564c508749358e2429df24162e913c43;564c508749358e2429df24162e913c43;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;3;-1229.104,-241.9642;Float;False;Property;_Dirt_Falloff;Dirt_Falloff;11;0;Create;True;0;0;0;False;0;False;14.11765;14;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1208.057,395.2502;Float;False;Property;_Dust_FallOff;Dust_FallOff;8;0;Create;True;0;0;0;False;0;False;14.11765;84.6;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;11;-1152.13,150.348;Float;True;Property;_Black;Black;14;1;[HideInInspector];Create;True;0;0;0;True;0;False;a57d1830013fe844194854109d699647;a57d1830013fe844194854109d699647;False;black;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;29;-733.739,-1094.569;Float;False;Property;_Large_Tiling;Large_Tiling;12;0;Create;True;0;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;28;-819.877,-1319.83;Float;True;Property;_Large_Grunge;Large_Grunge;3;0;Create;True;0;0;0;True;0;False;e8f0dce8b25b7a340bab74754e3c5b3b;7dc473f15a719614394ea14366f72ecc;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TexturePropertyNode;2;-1313.094,-710.6437;Float;True;Property;_Grunge;Grunge;2;0;Create;True;0;0;0;True;0;False;e8f0dce8b25b7a340bab74754e3c5b3b;4408bed58ddfa1440b1ad8bdbe0d86e8;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;30;-769.887,-995.1501;Float;False;Property;_Large_FallOff;Large_FallOff;13;0;Create;True;0;0;0;False;0;False;14.11765;14.11765;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-275.6937,440.8105;Float;False;Property;_Dust_Amount;Dust_Amount;7;0;Create;True;0;0;0;False;0;False;1;0;0;1.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-730.3198,-453.8427;Float;False;Property;_Dirt_Amount;Dirt_Amount;9;0;Create;True;0;0;0;False;0;False;1;0;0;1.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-111.0139,-868.5096;Float;False;Property;_Large_DirtAmount;Large_DirtAmount;15;0;Create;True;0;0;0;False;0;False;1;1;0;1.2;0;1;FLOAT;0
Node;AmplifyShaderEditor.TriplanarNode;17;-539.311,223.4735;Inherit;True;Cylindrical;World;False;Top Texture 3;_TopTexture3;white;-1;None;Mid Texture 3;_MidTexture3;white;1;None;Bot Texture 3;_BotTexture3;white;0;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT3;1,1,1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;27;-376.0063,-1149.861;Inherit;True;Cylindrical;World;False;Top Texture 0;_TopTexture0;white;-1;None;Mid Texture 0;_MidTexture0;white;-1;None;Bot Texture 0;_BotTexture0;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT3;1,1,1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TriplanarNode;16;-853.2233,-731.675;Inherit;True;Cylindrical;World;False;Top Texture 1;_TopTexture1;white;-1;None;Mid Texture 1;_MidTexture1;white;-1;None;Bot Texture 1;_BotTexture1;white;-1;None;Triplanar Sampler;Tangent;10;0;SAMPLER2D;;False;5;FLOAT;1;False;1;SAMPLER2D;;False;6;FLOAT;0;False;2;SAMPLER2D;;False;7;FLOAT;0;False;9;FLOAT3;0,0,0;False;8;FLOAT3;1,1,1;False;3;FLOAT2;1,1;False;4;FLOAT;1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;31;187.2601,-1085.195;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;7;-410.0459,-645.5282;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;1,1,1,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;6;-749.9958,-287.422;Inherit;True;Property;_Texture;Texture;0;0;Create;True;0;0;0;False;0;False;-1;180f637da4b840b46bc641a43ebbe31a;b937161d11f6ae540b8464b983edfac9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-716.0892,-40.55235;Inherit;False;Property;_Dust_Colour;Dust_Colour;6;0;Create;True;0;0;0;False;0;False;0.4558824,0.4196799,0.3653763,0;0.4558824,0.4196799,0.3653763,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;22;-72.55693,224.9776;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;34;130.1555,-632.9691;Inherit;False;Constant;_Int1;Int 1;18;0;Create;True;0;0;0;False;0;False;1;0;False;0;1;INT;0
Node;AmplifyShaderEditor.LerpOp;33;416.2227,-840.7526;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.LerpOp;18;132.2767,-315.7751;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;26;-333.3938,-170.3296;Inherit;True;Property;_Normal_Map;Normal_Map;1;0;Create;True;0;0;0;False;0;False;-1;180f637da4b840b46bc641a43ebbe31a;71be0b74c606f3448ba347f7b1422170;True;0;False;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;25;154.743,186.5478;Inherit;False;Property;_SmoothnessSpec;Smoothness(Spec);4;0;Create;True;0;0;0;False;0;False;0.1962713;0.1962713;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;538.6368,-556.7639;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;156.5736,341.4676;Inherit;False;Property;_Metallic;Metallic;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;569.7997,-129.9;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SyntyStudios/GrungeTriplanar;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;8;0
WireConnection;17;1;11;0
WireConnection;17;2;11;0
WireConnection;17;4;12;0
WireConnection;27;0;28;0
WireConnection;27;1;28;0
WireConnection;27;2;28;0
WireConnection;27;3;29;0
WireConnection;27;4;30;0
WireConnection;16;0;2;0
WireConnection;16;1;2;0
WireConnection;16;2;2;0
WireConnection;16;3;1;0
WireConnection;16;4;3;0
WireConnection;31;0;27;0
WireConnection;31;1;32;0
WireConnection;7;0;16;0
WireConnection;7;1;4;0
WireConnection;22;0;17;4
WireConnection;22;2;23;0
WireConnection;33;0;7;0
WireConnection;33;1;34;0
WireConnection;33;2;31;0
WireConnection;18;0;6;0
WireConnection;18;1;9;0
WireConnection;18;2;22;0
WireConnection;14;0;18;0
WireConnection;14;1;33;0
WireConnection;0;0;14;0
WireConnection;0;1;26;0
WireConnection;0;3;24;0
WireConnection;0;4;25;0
ASEEND*/
//CHKSM=6757938602C13D4539926B5C441E13720EEB7BB1