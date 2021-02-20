using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    // 选中的轮廓组件
    //public _2DC_ShaderLerpDemo outline;

    // 选中目标
    public GameObject target;

    private Graphic _line;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (target && target.GetComponentInChildren<Graphic>())
            {
                target.GetComponentInChildren<Graphic>().material = null;
                if (_line)
                {
                    _line.enabled = false;
                }
            }

            var point = new PointerEventData(EventSystem.current);
            point.position = Input.mousePosition;
            var raycasters = Resources.FindObjectsOfTypeAll<GraphicRaycaster>();
            foreach (var raycaster in raycasters)
            {
                var results = new List<RaycastResult>();
                raycaster.Raycast(point, results);
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject.tag == "Component")
                    {
                        target = results[i].gameObject;
                    }
                }
            }

            if (!target)
            {
                return;
            }

            Debug.Log(target.name);
            if (!target.GetComponentInChildren<Graphic>())
            {
                _line = target.AddComponent<Image>();
                //_line.material = outline.mat;
                _line.enabled = true;
            }
            else
            {
                //target.GetComponentInChildren<Graphic>().material = outline.mat;
            }
        }
    }

    private void OnEnable()
    {
        //outline = GetComponent<_2DC_ShaderLerpDemo>();
    }
}