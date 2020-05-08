using UnityEngine;

public class ContainerGenerator : MonoBehaviour
{
    [SerializeField] GameObject m_ContainerPrefab;

    //Assgin the region for randomly generated container
    [SerializeField] Vector2 m_Dimension = new Vector2(20,20);
    //Offset the origin of container generation region from global origin.
    Vector2 offsetDistance = new Vector2(10, 5);


    //-------------Generate the Initial Primitives---------------//
    void Start()
    {
        //Randomly generate containers amount between 4~7
        GeneratePrimitives(m_ContainerPrefab, Random.Range(4,7));

    }

    //--------------Primitives Generation FUnction--------------//
    void GeneratePrimitives(GameObject primitive, int count)
    {
        for (int i = 0; i < count; i++)
        {
            var primitiveIns = GameObject.Instantiate(primitive);
            primitiveIns.transform.localPosition = new Vector3(
                    Random.Range(-m_Dimension.x, m_Dimension.x) + offsetDistance.x,
                    1.5f,  //generated height
                    Random.Range(-m_Dimension.y, m_Dimension.y) + offsetDistance.y
                );

        }
    }


}
