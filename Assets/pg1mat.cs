using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class pg1mat : MonoBehaviour
{
    public GameObject parking_Garage;
    public Material material1;
    public Material material2;
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer[] meshRenderers = parking_Garage.GetComponentsInChildren<MeshRenderer>();

        MeshRenderer concrete1 = meshRenderers[0];
        MeshRenderer concrete2 = meshRenderers[1];
        MeshRenderer concrete3 = meshRenderers[2];
        MeshRenderer concrete4 = meshRenderers[3];
        MeshRenderer concrete5 = meshRenderers[4];
        MeshRenderer concrete6 = meshRenderers[5];
        MeshRenderer concrete7 = meshRenderers[6];
        MeshRenderer concrete8 = meshRenderers[7];



        foreach (MeshRenderer meshRenderers1 in meshRenderers)
        {
            meshRenderers1.material = material2;
            meshRenderers1.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
