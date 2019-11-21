Shader "Custom/NewSurfaceShader" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		//LOD 200
		Cull off //关闭背面剔除(显示背面)

		CGPROGRAM
		#pragma surface surf Standard vertex:vert //定义顶点函数vert
		#pragma target 3.0

		sampler2D _MainTex;
		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		fixed4 _WorldCenter; //准心的世界坐标

		struct Input 
		{
			float2 uv_MainTex;
			float4 objPos; //保存顶点模型坐标
		};

		void vert(inout appdata_full v, out Input o) 
		{
			o.uv_MainTex = v.texcoord.xy;
			o.objPos = v.vertex; //获取顶点模型坐标
		}
		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			fixed4 worldPos = mul(unity_ObjectToWorld, IN.objPos); //获取顶点的世界坐标
			if (distance(worldPos.xz, _WorldCenter.xz) < 0.1) //在鼠标位置周围0.5范围内显示红色
				o.Albedo += fixed4(1, 1, 1, 1);
		}		
		ENDCG
	
	}
	FallBack "Diffuse"
}
