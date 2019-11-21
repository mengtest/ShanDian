using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    Transform player;
    Terrain terr;
    LineRenderer lineR;
    public ParticleSystem effect;
    int nodeCount = 10; //闪电节点
    public LayerMask layer;
    private void OnEnable()
    {
        StartCoroutine(ReleaseLine());
    }
    public void Init(Transform _player, Terrain _terr) //初始化
    {
        player = _player;
        terr = _terr;

        lineR = GetComponent<LineRenderer>();
    }
    IEnumerator ReleaseLine() //释放闪电
    {
        Vector3 pos = player.position + Vector3.down / 2; //发射点位置在控制器下方一点
        //闪电起始点位置
        lineR.positionCount = 1;
        lineR.SetPosition(0, pos);

        float inter = pos.y / (nodeCount - 1); //节点之间的位置间隔

        for (int i = 1; i < nodeCount; i++) //为每个节点赋值
        {
            lineR.positionCount++;
            if (i == nodeCount - 1) //如果是最后一个节点
            {
                lineR.SetPosition(i, new Vector3(pos.x, pos.y - i * inter, pos.z)); //xz轴位置和起始节点一样            
                terr.center = lineR.GetPosition(i); //确定打击点
                terr.recovery = false;
                //特效
                effect.transform.position = terr.center;
                effect.Play();

                CheakEnemy(terr.center);
                break;
            }
            //除首尾两个节点外,其它节点在xz上会随机偏移一定位置
            lineR.SetPosition(i, new Vector3(pos.x + Random.Range(-0.5f, 0.5f), pos.y - i * inter, pos.z + Random.Range(-0.5f, 0.5f)));
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.05f);
        gameObject.SetActive(false);
    }
    void CheakEnemy(Vector3 hitPos) //胶囊射线检测零件
    {
        Collider[] col = Physics.OverlapCapsule(hitPos + Vector3.down, hitPos + Vector3.up, 0.5f, layer);
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].GetComponent<DisPlayCenter>().HitFly(hitPos)) //每次击落一个零件
                break;
        }
    }
}
