using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    MeshFilter[] meshs; //地形所有网格组件
    [HideInInspector] public Vector3 center; //打击点
    float wave; //冲击波半径
    public bool recovery; //地形是否恢复
    public float r; //最大波及半径
    public float waveSpeed; //冲击波扩散速度
    void Start()
    {
        meshs = GetComponentsInChildren<MeshFilter>(); //拿到所有网格组件
        wave = 0.5f; //冲击波初始半径
        recovery = true;
    }
    void Update()
    {
        SetMehs();
    }
    void SetMehs()
    {
        if (!recovery) 
        {
            wave += Time.deltaTime * waveSpeed; //冲击波扩散
            if (wave >= r) //扩散到一定范围,地形恢复
                recovery = !recovery;
        }
        else
            wave = 0.5f; 


        for (int i = 0; i < meshs.Length; i++) //遍历所有网格组件
        {
            Vector3[] verts = meshs[i].mesh.vertices;
            for (int j = 0; j < verts.Length; j++) //遍历每个网格组件的所有顶点
            {
                //获取每个顶点到打击点的距离
                Vector3 pos = meshs[i].transform.TransformPoint(new Vector3(verts[j].x, 0, verts[j].z));
                float dis = Vector3.Distance(center, pos);

                if (Mathf.Abs(wave - dis) < 0.2 && !recovery) //冲击波波及处,地面隆起
                    verts[j].y += Time.deltaTime * Mathf.Abs(r - dis) * waveSpeed;
                else if (verts[j].y > 0) //冲击波过后,隆起的地面下降
                    verts[j].y -= Time.deltaTime * Mathf.Abs(r - dis) * waveSpeed;
                else
                    verts[j].y = 0; //其他地面保持在水平位置
            }
            meshs[i].mesh.vertices = verts;
            meshs[i].GetComponent<MeshCollider>().sharedMesh = meshs[i].mesh; //网格碰撞器同步
        }

    }
}
