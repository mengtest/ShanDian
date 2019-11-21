using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Terrain terr;
    public Lightning line;
    public int speed;
    public LayerMask layer;
    void Start()
    {
        line.Init(transform, terr);
    }

    void Update()
    {
        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * speed);//移动
        CreateLine(Input.GetMouseButtonDown(0) && terr.recovery);

        DisplayCenter();
    }

    void CreateLine(bool key) //发射线启用闪电
    {
        if (key)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 10))
            {
                //如果闪电是禁用状态,则启用
                if (line.gameObject.activeInHierarchy == false)
                {
                    line.gameObject.SetActive(true);
                }
            }
        }
    }

    void DisplayCenter() //显示准心
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, layer)) //发射射线
        {
            Collider[] col = Physics.OverlapSphere(hit.point, 5, layer); //在射线点再发射球型射线检测地形和零件
            for (int i = 0; i < col.Length; i++)
            {
                col[i].GetComponent<DisPlayCenter>().GetCenter(hit.point, true);
            }
        }
    }
}
