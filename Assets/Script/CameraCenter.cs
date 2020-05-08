using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    //Initialize camera center
    [SerializeField] Camera[] m_Cameras = new Camera[3];
    [SerializeField] Camera m_ControlAidCamera = new Camera();

    int m_SelectedIndex = 0;


    void Start()
    {
        SelectedCamera(m_SelectedIndex);
    }


    void Update()
    {
        //Press N to change camera
        if (Input.GetKeyDown (KeyCode.N))
        {
            NextCamera();
        }

        //When selected to controller comera (index=1), activate assistant camera (top view camera attached to the trolley)
        if (m_SelectedIndex == 1)
        {
            m_ControlAidCamera.enabled = true;
        }
        //Deactivate assistant camera otherwise
        else
        {
            m_ControlAidCamera.enabled = false;
        }

    }

    void NextCamera()
    {
        m_SelectedIndex++;
        if (m_SelectedIndex >= m_Cameras.Length)
        {
            m_SelectedIndex = 0;
        }
        SelectedCamera(m_SelectedIndex);
    }

    void SelectedCamera (int index)
    {
        for (int i = 0; i < m_Cameras.Length; i++)
        {
            m_Cameras[i].enabled = i == index;
        }
    }

}
