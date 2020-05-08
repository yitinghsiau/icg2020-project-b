using UnityEngine;

public class TowerControl : MonoBehaviour
{    
    //Assign models to the script
    [SerializeField] GameObject m_Jib;  
    [SerializeField] GameObject m_Hook;

    //Assign models and joint connection points to the script (for line rendering)
    [SerializeField] GameObject m_Trolley;
    [SerializeField] GameObject m_TrolleyJointBody_1;
    [SerializeField] GameObject m_TrolleyJointBody_2;

    //Assign models to the script
    [SerializeField] GameObject m_HookJointBody;

    //Assign temporate gameobject for joint connection
    GameObject m_DetectedObject;
    GameObject m_AttachedObject;

    //-----------------------------------------------------------------------------//

    //Generate configurable joints of trolley-hook
    ConfigurableJoint m_TrolleyJoint_1;
    ConfigurableJoint m_TrolleyJoint_2;

    //Generate configurable joints of hook-container
    Joint m_JointForObject_1;
    Joint m_JointForObject_2;
    Joint m_JointForObject_3;
    Joint m_JointForObject_4;

    //-----------------------------------------------------------------------------//

    //Assign trolley moving speed and jib rotating speed
    const float MOVE_SPEED = 2f;
    const float ROTATE_SPEED = 10f;

    //Assign the maximum detection distance
    const float ATTACH_DISTANCE = 1.5f;

    //Initialize cable length (the joint of trolley-hook)
    float CableLimit = 2f;

    //Detect the moving distance of trolley (for control limit)
    float TrolleyDistance;

    //-----------------------------------------------------------------------------//

    //Assign line renenderer to the script (joints of trolley-hook)
    [SerializeField] LineRenderer m_TrolleyCable_1;
    [SerializeField] LineRenderer m_TrolleyCable_2;

    //Assign line renenderer to the script (joints of hook-container)
    [SerializeField] LineRenderer m_Cable_1;
    [SerializeField] LineRenderer m_Cable_2;
    [SerializeField] LineRenderer m_Cable_3;
    [SerializeField] LineRenderer m_Cable_4;



    void Start()
    {       
        //Assign the properties of trolley-hook joint (1st cable)
        var joint_1 = m_Trolley.AddComponent<ConfigurableJoint>();
        joint_1.xMotion = ConfigurableJointMotion.Limited;
        joint_1.yMotion = ConfigurableJointMotion.Limited;
        joint_1.zMotion = ConfigurableJointMotion.Limited;
        joint_1.angularXMotion = ConfigurableJointMotion.Free;
        joint_1.angularYMotion = ConfigurableJointMotion.Free;
        joint_1.angularZMotion = ConfigurableJointMotion.Free;

        var limit_1 = joint_1.linearLimit;
        limit_1.limit = 2f;
        joint_1.linearLimit = limit_1;

        //Assign the anchor point of trolley-hook joint (1st cable)
        joint_1.autoConfigureConnectedAnchor = false;
        joint_1.connectedAnchor = new Vector3(0f, 0.1f, -0.15f);
        joint_1.anchor = new Vector3(0f, 0f, 2.3f);

        //Assign the connect body of trolley-hook joint (1st cable)
        joint_1.connectedBody = m_Hook.GetComponent<Rigidbody>();
        m_TrolleyJoint_1 = joint_1;

        //-----------------------------------------------------------------------------//

        //Assign the properties of trolley-hook joint (2nd cable)
        var joint_2 = m_Trolley.AddComponent<ConfigurableJoint>();
        joint_2.xMotion = ConfigurableJointMotion.Limited;
        joint_2.yMotion = ConfigurableJointMotion.Limited;
        joint_2.zMotion = ConfigurableJointMotion.Limited;
        joint_2.angularXMotion = ConfigurableJointMotion.Free;
        joint_2.angularYMotion = ConfigurableJointMotion.Free;
        joint_2.angularZMotion = ConfigurableJointMotion.Free;

        var limit_2 = joint_2.linearLimit;
        limit_2.limit = 2f;
        joint_2.linearLimit = limit_2;

        //Assign the anchor point of trolley-hook joint (2nd cable)
        joint_2.autoConfigureConnectedAnchor = false;
        joint_2.connectedAnchor = new Vector3(0f, 0.1f, 0.15f);
        joint_2.anchor = new Vector3(0f, 0f, 1.7f);

        //Assign the connect body of trolley-hook joint (2nd cable)
        joint_2.connectedBody = m_Hook.GetComponent<Rigidbody>();
        m_TrolleyJoint_2 = joint_2;


    }

    //Update cable length
    void FixedUpdate()
    {      
        //cable 1 of trolley-hook joint
        var limit_1 = m_TrolleyJoint_1.linearLimit;
        limit_1.limit = CableLimit;
        m_TrolleyJoint_1.linearLimit = limit_1;

        //cable 2 of trolley-hook joint
        var limit_2 = m_TrolleyJoint_2.linearLimit;
        limit_2.limit = CableLimit;
        m_TrolleyJoint_2.linearLimit = limit_2;
    }
    

    void Update()
    {
        float DeltaTime = Time.deltaTime;

        //Control the rotation of jib
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            
            m_Jib.transform.Rotate(0, -ROTATE_SPEED * DeltaTime, 0);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            m_Jib.transform.Rotate(0, ROTATE_SPEED * DeltaTime, 0);

        }

        //-----------------------------------------------------------------------------//

        //Control the movement of trolley
        //Calculate the moving distance of trolley (for control limit)
        Vector3 Origin = new Vector3(0, 16.4f, 0);
        Vector3 RelativePosition = m_Trolley.transform.position - Origin;
        TrolleyDistance = RelativePosition.magnitude;
        //Move the trolley while within the contol limit
        if (TrolleyDistance >= 0.05 && TrolleyDistance <= 15)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                m_Trolley.transform.Translate(0, 0, MOVE_SPEED * DeltaTime);

            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                m_Trolley.transform.Translate(0, 0, -MOVE_SPEED * DeltaTime);


            }
        }
        //Avoid trolley from moving outside the control limit by force moving back
        else if(TrolleyDistance < 0.05)
        {
            m_Trolley.transform.Translate(0, 0, 0.3f * MOVE_SPEED * DeltaTime);
        }
        else if(TrolleyDistance > 15)
        {
            m_Trolley.transform.Translate(0, 0, 0.3f * -MOVE_SPEED * DeltaTime);
        }

        //-----------------------------------------------------------------------------//

        //Control the hook height by assigning joint linear limit
        if (Input.GetKey(KeyCode.Q))
        {
            CableLimit = Mathf.Clamp(CableLimit + MOVE_SPEED * DeltaTime, 2f, 14f);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            CableLimit = Mathf.Clamp(CableLimit - MOVE_SPEED * DeltaTime, 2f, 14f);
        }

        //-----------------------------------------------------------------------------//

        //Detect object
        if (m_JointForObject_1 == null)
        {
            DetectObjects();
        }

        //-----------------------------------------------------------------------------//

        //Attach or detach object
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttachOrDetachObject();

        }

        //-----------------------------------------------------------------------------//

        UpdateCable();

    }

   
    void DetectObjects()
    {
        Ray ray = new Ray(m_Hook.transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, ATTACH_DISTANCE))
        {
            if (m_DetectedObject == hit.collider.gameObject)
            {
                return;
            }

            RecoverDetectedObject();

            MeshRenderer renender = hit.collider.GetComponent<MeshRenderer>();

            if (renender != null)
            {
                renender.material.color = Color.yellow;
                m_DetectedObject = hit.collider.gameObject;
            }
            
        }
        else
        {
            RecoverDetectedObject();
        }
    }


    void RecoverDetectedObject()
    {
        if (m_DetectedObject != null)
        {
            m_DetectedObject.GetComponent<MeshRenderer>().material.color = Color.white;
            m_DetectedObject = null;
        }
    }


    void AttachOrDetachObject()
    {
        if (m_JointForObject_1 == null)
        {
            if (m_DetectedObject != null)
            {
                float Hook_CableLimit = 2.3f;

                //Assign the properties of hook-object joint (1st corner of container joint)
                var hook_joint_1 = m_Hook.AddComponent<ConfigurableJoint>();
                hook_joint_1.xMotion = ConfigurableJointMotion.Limited;
                hook_joint_1.yMotion = ConfigurableJointMotion.Limited;
                hook_joint_1.zMotion = ConfigurableJointMotion.Limited;
                hook_joint_1.angularXMotion = ConfigurableJointMotion.Free;
                hook_joint_1.angularYMotion = ConfigurableJointMotion.Free;
                hook_joint_1.angularZMotion = ConfigurableJointMotion.Free;

                var hook_joint_limit_1 = hook_joint_1.linearLimit;
                hook_joint_limit_1.limit = Hook_CableLimit;
                hook_joint_1.linearLimit = hook_joint_limit_1;

                //Assign the anchor point of hook-object joint (1st corner of container joint)
                hook_joint_1.autoConfigureConnectedAnchor = false;
                hook_joint_1.connectedAnchor = new Vector3(1.15f, 2.3f, 4.7f);
                hook_joint_1.anchor = new Vector3(0f, -0.7f, 0f);

                hook_joint_1.connectedBody = m_DetectedObject.GetComponent<Rigidbody>();

                m_JointForObject_1 = hook_joint_1;

                //----------Container Joint 2------------//
                var hook_joint_2 = m_Hook.AddComponent<ConfigurableJoint>();
                hook_joint_2.xMotion = ConfigurableJointMotion.Limited;
                hook_joint_2.yMotion = ConfigurableJointMotion.Limited;
                hook_joint_2.zMotion = ConfigurableJointMotion.Limited;
                hook_joint_2.angularXMotion = ConfigurableJointMotion.Free;
                hook_joint_2.angularYMotion = ConfigurableJointMotion.Free;
                hook_joint_2.angularZMotion = ConfigurableJointMotion.Free;

                var hook_joint_limit_2 = hook_joint_2.linearLimit;
                hook_joint_limit_2.limit = Hook_CableLimit;

                hook_joint_2.linearLimit = hook_joint_limit_2;

                hook_joint_2.autoConfigureConnectedAnchor = false;
                hook_joint_2.connectedAnchor = new Vector3(1.15f, 2.3f, -4.7f);
                hook_joint_2.anchor = new Vector3(0f, -0.3f, 0f);

                hook_joint_2.connectedBody = m_DetectedObject.GetComponent<Rigidbody>();

                m_JointForObject_2 = hook_joint_2;

                //----------Container Joint 3------------//
                var hook_joint_3 = m_Hook.AddComponent<ConfigurableJoint>();
                hook_joint_3.xMotion = ConfigurableJointMotion.Limited;
                hook_joint_3.yMotion = ConfigurableJointMotion.Limited;
                hook_joint_3.zMotion = ConfigurableJointMotion.Limited;
                hook_joint_3.angularXMotion = ConfigurableJointMotion.Free;
                hook_joint_3.angularYMotion = ConfigurableJointMotion.Free;
                hook_joint_3.angularZMotion = ConfigurableJointMotion.Free;

                var hook_joint_limit_3 = hook_joint_3.linearLimit;
                hook_joint_limit_3.limit = Hook_CableLimit;

                hook_joint_3.linearLimit = hook_joint_limit_3;

                hook_joint_3.autoConfigureConnectedAnchor = false;
                hook_joint_3.connectedAnchor = new Vector3(-1.15f, 2.3f, 4.7f);
                hook_joint_3.anchor = new Vector3(0f, -0.3f, 0f);

                hook_joint_3.connectedBody = m_DetectedObject.GetComponent<Rigidbody>();

                m_JointForObject_3 = hook_joint_3;

                //----------Container Joint 4------------//
                var hook_joint_4 = m_Hook.AddComponent<ConfigurableJoint>();
                hook_joint_4.xMotion = ConfigurableJointMotion.Limited;
                hook_joint_4.yMotion = ConfigurableJointMotion.Limited;
                hook_joint_4.zMotion = ConfigurableJointMotion.Limited;
                hook_joint_4.angularXMotion = ConfigurableJointMotion.Free;
                hook_joint_4.angularYMotion = ConfigurableJointMotion.Free;
                hook_joint_4.angularZMotion = ConfigurableJointMotion.Free;

                var hook_joint_limit_4 = hook_joint_4.linearLimit;
                hook_joint_limit_4.limit = Hook_CableLimit;

                hook_joint_4.linearLimit = hook_joint_limit_4;

                hook_joint_4.autoConfigureConnectedAnchor = false;
                hook_joint_4.connectedAnchor = new Vector3(-1.15f, 2.3f, -4.7f);
                hook_joint_4.anchor = new Vector3(0f, -0.3f, 0f);

                hook_joint_4.connectedBody = m_DetectedObject.GetComponent<Rigidbody>();

                m_JointForObject_4 = hook_joint_4;

                //Change the color of attached object to red
                m_DetectedObject.GetComponent<MeshRenderer>().material.color = Color.red;

                //Assign contents to temporarily storage (for color reset purpose)
                m_AttachedObject = m_DetectedObject;

                //Clear the content to avoid dual operation
                m_DetectedObject = null;
            }

        }

        else
        {
            //Reset attached object's color
            m_AttachedObject.GetComponent<MeshRenderer>().material.color = Color.white;

            //Release cable
            GameObject.Destroy(m_JointForObject_1);
            GameObject.Destroy(m_JointForObject_2);
            GameObject.Destroy(m_JointForObject_3);
            GameObject.Destroy(m_JointForObject_4);

            //Reset parameters
            m_JointForObject_1 = null;
            m_JointForObject_2 = null;
            m_JointForObject_3 = null;
            m_JointForObject_4 = null;

            m_AttachedObject = null;
        }
    }





    
    void UpdateCable()
    {
        
        var connectedBodyTransform_Hook = m_TrolleyJoint_1.connectedBody.transform;

        //Assign cable points' position (trolley-hook joint)
        m_TrolleyCable_1.enabled = true;
        m_TrolleyCable_1.SetPosition(0, m_TrolleyJointBody_1.transform.position);
        m_TrolleyCable_1.SetPosition(1, connectedBodyTransform_Hook.TransformPoint(m_TrolleyJoint_1.connectedAnchor));
        m_TrolleyCable_1.GetComponent<LineRenderer>().material.color = Color.black;

        m_TrolleyCable_2.enabled = true;
        m_TrolleyCable_2.SetPosition(0, m_TrolleyJointBody_2.transform.position);
        m_TrolleyCable_2.SetPosition(1, connectedBodyTransform_Hook.TransformPoint(m_TrolleyJoint_2.connectedAnchor));
        m_TrolleyCable_2.GetComponent<LineRenderer>().material.color = Color.black;


        if (m_JointForObject_1 != null)
        {
            //Assign cable points' position (hook-object joint)
            var connectedBodyTransform_Container_1 = m_JointForObject_1.connectedBody.transform;
            m_Cable_1.enabled = true;
            m_Cable_1.SetPosition(0, m_HookJointBody.transform.position);     
            m_Cable_1.SetPosition(1, connectedBodyTransform_Container_1.TransformPoint(m_JointForObject_1.connectedAnchor));
            m_Cable_1.GetComponent<LineRenderer>().material.color = Color.black;

            var connectedBodyTransform_Container_2 = m_JointForObject_2.connectedBody.transform;
            m_Cable_2.enabled = true;
            m_Cable_2.SetPosition(0, m_HookJointBody.transform.position);
            m_Cable_2.SetPosition(1, connectedBodyTransform_Container_2.TransformPoint(m_JointForObject_2.connectedAnchor));
            m_Cable_2.GetComponent<LineRenderer>().material.color = Color.black;

            var connectedBodyTransform_Container_3 = m_JointForObject_3.connectedBody.transform;
            m_Cable_3.enabled = true;
            m_Cable_3.SetPosition(0, m_HookJointBody.transform.position);
            m_Cable_3.SetPosition(1, connectedBodyTransform_Container_3.TransformPoint(m_JointForObject_3.connectedAnchor));
            m_Cable_3.GetComponent<LineRenderer>().material.color = Color.black;

            var connectedBodyTransform_Container_4 = m_JointForObject_4.connectedBody.transform;
            m_Cable_4.enabled = true;
            m_Cable_4.SetPosition(0, m_HookJointBody.transform.position);
            m_Cable_4.SetPosition(1, connectedBodyTransform_Container_4.TransformPoint(m_JointForObject_4.connectedAnchor));
            m_Cable_4.GetComponent<LineRenderer>().material.color = Color.black;

        }
        else
        {
            //Release cable rendering when disconnect the joint
            m_Cable_1.enabled = false;
            m_Cable_2.enabled = false;
            m_Cable_3.enabled = false;
            m_Cable_4.enabled = false;

        }       
    }  
}
