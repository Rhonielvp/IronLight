// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader " "
{
	Properties
	{
		_Blending("Blending", Range( 0 , 1)) = 0
		_Distortion("Distortion", 2D) = "bump" {}
		_DistortionScale("Distortion Scale", Range( 0 , 1)) = 0
		_RippleScale("Ripple Scale", Range( 0 , 20)) = 0
		_RippleSpeed("Ripple Speed", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 screenPos;
		};

		uniform sampler2D _GrabTexture;
		uniform sampler2D _Distortion;
		uniform float _RippleSpeed;
		uniform float _RippleScale;
		uniform float _DistortionScale;
		uniform float _Blending;
		uniform float _Smoothness;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = float3(0,0,0);
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor14 = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD( ( float4( ( UnpackNormal( tex2D( _Distortion, ( (( ( _Time.y * _RippleSpeed ) + ase_grabScreenPosNorm )).xy * _RippleScale ) ) ) * _DistortionScale ) , 0.0 ) + ase_grabScreenPosNorm ) ) );
			float4 temp_cast_1 = (1.0).xxxx;
			float4 lerpResult3 = lerp( screenColor14 , temp_cast_1 , _Blending);
			o.Emission = lerpResult3.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=16400
856;249;1279;722;2700.119;1100.391;2.490816;True;True
Node;AmplifyShaderEditor.RangedFloatNode;29;-1864.357,-410.8204;Float;False;Property;_RippleSpeed;Ripple Speed;4;0;Create;True;0;0;False;0;0;0.187;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;23;-1805.156,-581.4716;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GrabScreenPosition;7;-1495.778,65.83643;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1589.357,-489.8204;Float;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-1436.633,-372.3728;Float;False;2;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-1326.236,-251.7471;Float;False;Property;_RippleScale;Ripple Scale;3;0;Create;True;0;0;False;0;0;4.5;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwizzleNode;24;-1204.546,-344.8682;Float;False;FLOAT2;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1042.235,-319.747;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-883.8117,-193.8637;Float;False;Property;_DistortionScale;Distortion Scale;2;0;Create;True;0;0;False;0;0;0.342;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-883.558,-419.3701;Float;True;Property;_Distortion;Distortion;1;0;Create;True;0;0;False;0;dd2fd2df93418444c8e280f1d34deeb5;dd2fd2df93418444c8e280f1d34deeb5;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-562.2634,-250.1585;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-386.1619,-134.0374;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-345.6258,194.4731;Float;False;Property;_Blending;Blending;0;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-212.726,72.3189;Float;False;Constant;_White;White;0;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;14;-236.8995,-134.41;Float;False;Global;_GrabScreen1;Grab Screen 1;2;0;Create;True;0;0;True;0;Object;-1;False;True;1;0;FLOAT4;0,0,0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector3Node;1;-19.60051,-507.2949;Float;False;Constant;_Vector0;Vector 0;0;0;Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;30;-42.84546,-2.485962;Float;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;0;0.791;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;3;-33.07546,-138.249;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;379.5475,-194.0013;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard; ;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;28;0;23;2
WireConnection;28;1;29;0
WireConnection;27;0;28;0
WireConnection;27;1;7;0
WireConnection;24;0;27;0
WireConnection;25;0;24;0
WireConnection;25;1;26;0
WireConnection;6;1;25;0
WireConnection;19;0;6;0
WireConnection;19;1;20;0
WireConnection;18;0;19;0
WireConnection;18;1;7;0
WireConnection;14;0;18;0
WireConnection;3;0;14;0
WireConnection;3;1;4;0
WireConnection;3;2;5;0
WireConnection;0;0;1;0
WireConnection;0;2;3;0
WireConnection;0;4;30;0
ASEEND*/
//CHKSM=B470AA14C69C0C53087005690E477F0AC3129A66