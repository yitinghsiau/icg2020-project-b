using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraEntity : MonoBehaviour
{
    float VELOCITY;

    //Intiatialize Free camera (on the ground)
    Vector3 m_MousePosition;
    float m_HorizontalAngle = 0;
    float m_VerticalAngle = 0;

    void Start()
    {
        m_MousePosition = Input.mousePosition;
    }

    void Update()
    {

        //Moving Forward/Backward
        if (Input.GetKey (KeyCode.W))
        {
            VELOCITY = 3;
            this.transform.Translate(0, 0, VELOCITY * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            VELOCITY = 3;
            this.transform.Translate(0, 0, -VELOCITY * Time.deltaTime);
        }

        //Moving Left/Right
        if (Input.GetKey(KeyCode.A))
        {
            VELOCITY = 3;
            this.transform.Translate(-VELOCITY * Time.deltaTime, 0, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            VELOCITY = 3;
            this.transform.Translate(VELOCITY * Time.deltaTime,0 ,0);
        }

        //Moving Up/Down
        if (Input.GetKey(KeyCode.Z))
        {
            VELOCITY = 3;
            this.transform.Translate(0, VELOCITY * Time.deltaTime, 0);           
        }
        else if (Input.GetKey (KeyCode.C))
        {
            VELOCITY = 3;
            this.transform.Translate(0, -VELOCITY * Time.deltaTime, 0);
        }
        
        //Direction tracked by mouse position
        if (Input.GetMouseButtonDown(0))
        {
            m_MousePosition = Input.mousePosition;

        }else if (Input.GetMouseButton(0))
        {
            Vector3 mouseDeltaPosition = m_MousePosition - Input.mousePosition;

            m_HorizontalAngle -= -mouseDeltaPosition.x * 0.1f;
            m_VerticalAngle = Mathf.Clamp(m_VerticalAngle - mouseDeltaPosition.y * 0.1f, -89f, 89f);

            this.transform.localEulerAngles = new Vector3(m_VerticalAngle, m_HorizontalAngle, 0f);

            m_MousePosition = Input.mousePosition;
        }
        

    }
}
