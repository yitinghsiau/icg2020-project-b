using UnityEngine;

public class ControllerCameraEntity : MonoBehaviour
{

    //Intiatialize controller camera (in the cabin) 
    Vector3 m_MousePosition;
    float m_HorizontalAngle = 0;
    float m_VerticalAngle = 0;


    void Start()
    {
        m_MousePosition = Input.mousePosition;
    }

    void Update()
    {

        //Direction tracked by mouse position
        if (Input.GetMouseButtonDown(0))
        {
            m_MousePosition = Input.mousePosition;

        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 mouseDeltaPosition = m_MousePosition - Input.mousePosition;

            m_HorizontalAngle -= -mouseDeltaPosition.x * 0.1f;
            m_VerticalAngle = Mathf.Clamp(m_VerticalAngle - mouseDeltaPosition.y * 0.1f, -89f, 89f);

            this.transform.localEulerAngles = new Vector3(m_VerticalAngle, m_HorizontalAngle, 0f);

            m_MousePosition = Input.mousePosition;
        }


    }
}
