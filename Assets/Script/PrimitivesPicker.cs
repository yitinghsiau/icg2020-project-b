﻿using UnityEngine;

public class PrimitivesPicker : MonoBehaviour
{

    GameObject m_ClickedObject;


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            FireRaycast();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            RecoverClickedObject();
        }
    }


    void FireRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();

            if (renderer != null)
            {
                renderer.material.color = Color.red;

                m_ClickedObject = hit.collider.gameObject;
            }
        }

    }

    void RecoverClickedObject()
    {
        if (m_ClickedObject != null)
        {
            m_ClickedObject.GetComponent<MeshRenderer>().material.color = Color.white;
            m_ClickedObject = null;
        }
    }


}
