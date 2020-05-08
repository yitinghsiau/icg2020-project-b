using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardTargetRerionEntity : MonoBehaviour
{
    //Reset number of containers in the HARD target region
    int Counter_Hard = 0;

    [SerializeField] GameObject m_TargetHard;


    void Update()
    {
        //Change the color of target region to green, and finish game after 2 seconds.
        if (Counter_Hard >= 2)
        {

            m_TargetHard.GetComponent<MeshRenderer>().material.color = Color.green;
            this.Invoke("Finish", 2f);
        }

    }



    void OnCollisionEnter(Collision collision)
    {
        //When the obejct "touches" the target region, increase the number of counter and debug at console.
        Counter_Hard++;

        string Announcement = string.Format("There are {0} containers at the target region (Hard)", Counter_Hard);
        Debug.Log(Announcement);

        if (Counter_Hard == 2)
        {
            //While the counter reaches the assigned number, debug at console.
            Debug.Log("You Win!");
        }

    }

    void OnCollisionExit(Collision collision)
    {
        //When the obejct "leaves" the target region, decrease the number of counter and debug at console.
        Counter_Hard--;

        string Announcement = string.Format("There are {0} containers at the target region (Hard)", Counter_Hard);
        Debug.Log(Announcement);
    }

    //Function to end the game.
    void Finish()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
