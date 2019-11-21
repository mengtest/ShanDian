Shader "Custom/NewSurfaceShader"
{
	Properties
	{
	  _MainTex("MainTex",2D) = ""{} //主纹理
	  _Tex2("Tex2",2D) = ""{} //纹理2
	  _R1("R1",range(0,0.1)) = 0.01 //波动幅度1
	  _R2("R2",float) = 10 //波动幅度2
	  _Speed("Speed",float) = 5 //波动速度
	}
	SubShader
	{
	   pass
	   {
		  //colormask r 只会输出r值(红色)
		  //colormask rg 只会输出rg值(红色和绿色)
		  CGPROGRAM
		  #pragma vertex vert
		  #pragma fragment frag
		  #include "unitycg.cginc"
		  sampler2D _MainTex;
		  fixed4 _MainTex_ST;
		  sampler2D _Tex2;
		  fixed _R1;
		  fixed _R2;
		  fixed _Speed;
		  struct v2f
		  {
				fixed4 pos : SV_POSITION;
				fixed2 uv : TEXCOORD0;
		  };
		  v2f vert(appdata_full v)
		  {
			 v2f o;
			 o.pos = UnityObjectToClipPos(v.vertex);
			 o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
			 return o;
		  }
		  fixed4 frag(v2f vf) :COLOR
		  {
			 fixed4 mainTex = tex2D(_MainTex, vf.uv); //主纹理采样

			 fixed offset = _R1 * sin(vf.uv*_R2 + _Time.x*_Speed); //计算偏移
			 fixed2 uv = vf.uv + offset;
			 uv.y -= 0.5; //采样偏移
			 fixed4 tex2 = tex2D(_Tex2, uv); //偏移的纹理2

			 uv = vf.uv - offset; //反向偏移	   
			 uv.y -= 0.5; //采样偏移
			 tex2 += tex2D(_Tex2, uv); //纹理2自加反向偏移

			 return  mainTex + tex2; //主纹理叠加纹理2
		   }
		   ENDCG
	   }
    }
}
