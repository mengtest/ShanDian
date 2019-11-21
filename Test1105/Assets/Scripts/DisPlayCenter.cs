using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisPlayCenter : MonoBehaviour
{
    Material mat;
    bool show;
    Vector3 center;
    Rigidbody rig;
    
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    void Update()
    {
        if (show)
            mat.SetVector("_WorldCenter", center);
        else
            mat.SetVector("_WorldCenter", Vector3.forward * 100);
        show = false;
    }
    public void GetCenter(Vector3 _center, bool _show) //获取准心
    {
        center = _center;
        show = _show;
    }

    public bool HitFly(Vector3 hitPos) //击飞
    {
        if (transform.childCount == 0 && transform.parent != null) //如果没有子物体且有父物体
        {
            transform.SetParent(null); //先脱离父物体
            rig = gameObject.AddComponent<Rigidbody>(); //添加刚体
            return true; //如果被击落返回true
        }
        else if (transform.parent == null) //如果脱离了父物体则会被击飞
        {
            hitPos = new Vector3(hitPos.x, transform.position.y, hitPos.z); //重塑打击点位置

            Vector3 dir = (transform.position - hitPos).normalized; //击飞方向
            rig.AddForce((dir + Vector3.up) * 2, ForceMode.Impulse);
        }
        return false;
    }
}
