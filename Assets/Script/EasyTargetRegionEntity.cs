using UnityEngine;

public class EasyTargetRegionEntity : MonoBehaviour
{
    //Reset number of containers in the EASY target region
    int Counter_Easy = 0;

    [SerializeField] GameObject m_TargetEasy;


    void Update()
    {
        //Change the color of target region to green, and finish game after 2 seconds.
        if (Counter_Easy >= 3)
        {
            
            m_TargetEasy.GetComponent<MeshRenderer>().material.color = Color.green;           
            this.Invoke("Finish", 2f);
        }

    }


    
    void OnCollisionEnter(Collision collision)
    {
        //When the obejct "touches" the target region, increase the number of counter and debug at console.
        Counter_Easy++;

        string Announcement = string.Format("There are {0} containers at the target region (Easy)", Counter_Easy);
        Debug.Log(Announcement);

        if (Counter_Easy == 3)
        {
            //While the counter reaches the assigned number, debug at console.
            Debug.Log("You Win!");
        }

    }

    void OnCollisionExit(Collision collision)
    {
        //When the obejct "leaves" the target region, decrease the number of counter and debug at console.
        Counter_Easy--;

        string Announcement = string.Format("There are {0} containers at the target region (Easy)", Counter_Easy);
        Debug.Log(Announcement);
    }

    //Function to end the game.
    void Finish()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
