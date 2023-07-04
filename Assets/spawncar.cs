using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class spawncar : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject car_blue;
    public GameObject parking_Garage;
    public Material material1;
    public Material material2;

    public enum Option { _0, _25, _50, _75, _90, _100 };
    [Tooltip("Select type of simulation")]
    public Option occupancy;

    void Start()
    {
        Vector3[] vectorList1 = new Vector3[34];
        Vector3[] vectorList2 = new Vector3[4];
        Vector3[] vectorList3 = new Vector3[5];
        Vector3[] vectorList4 = new Vector3[10];
        Vector3[] vectorList5 = new Vector3[3];
        Vector3[] vectorList6 = new Vector3[4];
        Vector3[] vectorList7 = new Vector3[34];
        Vector3[] vectorList8 = new Vector3[4];
        Vector3[] vectorList9 = new Vector3[3];
        Vector3[] vectorList10 = new Vector3[10];
        Vector3[] vectorList11 = new Vector3[5];
        Vector3[] vectorList12 = new Vector3[4];

        for (var i = 1; i < 34; i++)
        {
            vectorList1[i] = new Vector3(36.8f - (2.5f * i), -2.0f, 50.13f);
        }
        for (var i = 1; i < 4; i++)
        {
            vectorList2[i] = new Vector3(36.8f - (2.5f * i), -2.0f, 60.99f);
        }
        for (var i = 1; i < 5; i++)
        {
            vectorList3[i] = new Vector3(21.83f - (2.5f * i), -2.0f, 60.99f);
        }
        for (var i = 1; i < 10; i++)
        {
            vectorList4[i] = new Vector3(4.31f - (2.5f * i), -2.0f, 60.99f);
        }
        for (var i = 1; i < 3; i++)
        {
            vectorList5[i] = new Vector3(-25.93f - (2.5f * i), -2.0f, 60.99f);
        }
        for (var i = 1; i < 4; i++)
        {
            vectorList6[i] = new Vector3(-38.32f - (2.5f * i), -2.0f, 60.99f);
        }
        for (var i = 1; i < 34; i++)
        {
            vectorList7[i] = new Vector3(-48.27f + (2.5f * i), -2.0f, 84.88f);
        }
        for (var i = 1; i < 4; i++)
        {
            vectorList8[i] = new Vector3(-48.27f + (2.5f * i), -2.0f, 74.180f);
        }
        for (var i = 1; i < 3; i++)
        {
            vectorList9[i] = new Vector3(-33.49f + (2.5f * i), -2.0f, 74.180f);
        }
        for (var i = 1; i < 10; i++)
        {
            vectorList10[i] = new Vector3(-20.7f + (2.5f * i), -2.0f, 74.180f);
        }
        for (var i = 1; i < 5; i++)
        {
            vectorList11[i] = new Vector3(9.45f + (2.5f * i), -2.0f, 74.180f);
        }
        for (var i = 1; i < 4; i++)
        {
            vectorList12[i] = new Vector3(26.91f + (2.5f * i), -2.0f, 74.180f);
        }

        Vector3[] pg_pos_temp = vectorList1.Concat(vectorList2).Concat(vectorList3).Concat(vectorList4).Concat(vectorList5).Concat(vectorList6).Concat(vectorList7).Concat(vectorList8).Concat(vectorList9).Concat(vectorList10).Concat(vectorList11).Concat(vectorList12).ToArray();
        Vector3[] pg_pos = pg_pos_temp.Where(v => v != Vector3.zero).ToArray();

        //foreach (Vector3 v in pg_pos)
        //{
        //    Debug.Log("pg_pos = "+v);
        // }

        MeshRenderer[] pg = parking_Garage.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer pg1 in pg)
        {
            pg1.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
        }

        if (occupancy == Option._0)
        {

        }
        else if (occupancy == Option._25)
        {
            Vector3[] randomValues = new Vector3[27];

            for (int i = 0; i < randomValues.Length; i++)
            {
                Vector3 randomValue = pg_pos[Random.Range(0, pg_pos.Length)];

                while (randomValues.Contains(randomValue))
                {
                    randomValue = pg_pos[Random.Range(0, pg_pos.Length)];
                }

                randomValues[i] = randomValue;
            }

            foreach (Vector3 position in randomValues)
            {
                GameObject go = Instantiate(car_blue, position, Quaternion.identity);
                MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
                //MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                //foreach (MeshRenderer renderer in meshRenderers)
                //{
                //    Debug.Log(renderer);
                //}

                // Access the mesh renderer of the first child game object
                MeshRenderer bbody = meshRenderers[0];
                Material[] bbodyMaterials = bbody.materials;

                // Apply the material to the second element in the materials array of bbody
                if (bbodyMaterials.Length > 1)
                {
                    bbodyMaterials[1] = material1;
                    bbody.materials = bbodyMaterials;
                }
                // Access the mesh renderer of the second child game object
                MeshRenderer sspoiler = meshRenderers[1];

                // Access the mesh renderer of the third child game object
                MeshRenderer flw = meshRenderers[2];
                MeshRenderer frw = meshRenderers[3];
                MeshRenderer rlw = meshRenderers[4];
                MeshRenderer rrw = meshRenderers[5];



                bbody.material = material1;
                sspoiler.material = material1;
                flw.material = material2;
                frw.material = material2;
                rlw.material = material2;
                rrw.material = material2;

                bbody.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                sspoiler.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                flw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                frw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rlw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rrw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                
            }
        }
        else if (occupancy == Option._50)
        {
            Vector3[] randomValues = new Vector3[54];

            for (int i = 0; i < randomValues.Length; i++)
            {
                Vector3 randomValue = pg_pos[Random.Range(0, pg_pos.Length)];

                while (randomValues.Contains(randomValue))
                {
                    randomValue = pg_pos[Random.Range(0, pg_pos.Length)];
                }

                randomValues[i] = randomValue;
            }

            foreach (Vector3 position in randomValues)
            {
                GameObject go = Instantiate(car_blue, position, Quaternion.identity);
                MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
                //MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                //foreach (MeshRenderer renderer in meshRenderers)
                //{
                //    Debug.Log(renderer);
                //}

                // Access the mesh renderer of the first child game object
                MeshRenderer bbody = meshRenderers[0];
                Material[] bbodyMaterials = bbody.materials;

                // Apply the material to the second element in the materials array of bbody
                if (bbodyMaterials.Length > 1)
                {
                    bbodyMaterials[1] = material1;
                    bbody.materials = bbodyMaterials;
                }
                // Access the mesh renderer of the second child game object
                MeshRenderer sspoiler = meshRenderers[1];

                // Access the mesh renderer of the third child game object
                MeshRenderer flw = meshRenderers[2];
                MeshRenderer frw = meshRenderers[3];
                MeshRenderer rlw = meshRenderers[4];
                MeshRenderer rrw = meshRenderers[5];



                bbody.material = material1;
                sspoiler.material = material1;
                flw.material = material2;
                frw.material = material2;
                rlw.material = material2;
                rrw.material = material2;

                bbody.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                sspoiler.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                flw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                frw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rlw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rrw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
            }
        }
        else if (occupancy == Option._75)
        {
            Vector3[] randomValues = new Vector3[81];

            for (int i = 0; i < randomValues.Length; i++)
            {
                Vector3 randomValue = pg_pos[Random.Range(0, pg_pos.Length)];

                while (randomValues.Contains(randomValue))
                {
                    randomValue = pg_pos[Random.Range(0, pg_pos.Length)];
                }

                randomValues[i] = randomValue;
            }

            foreach (Vector3 position in randomValues)
            {
                GameObject go = Instantiate(car_blue, position, Quaternion.identity);
                MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
                //MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                //foreach (MeshRenderer renderer in meshRenderers)
                //{
                //    Debug.Log(renderer);
                //}

                // Access the mesh renderer of the first child game object
                MeshRenderer bbody = meshRenderers[0];
                Material[] bbodyMaterials = bbody.materials;

                // Apply the material to the second element in the materials array of bbody
                if (bbodyMaterials.Length > 1)
                {
                    bbodyMaterials[1] = material1;
                    bbody.materials = bbodyMaterials;
                }
                

                // Access the mesh renderer of the second child game object
                MeshRenderer sspoiler = meshRenderers[1];

                // Access the mesh renderer of the third child game object
                MeshRenderer flw = meshRenderers[2];
                MeshRenderer frw = meshRenderers[3];
                MeshRenderer rlw = meshRenderers[4];
                MeshRenderer rrw = meshRenderers[5];



                bbody.material = material1;
                sspoiler.material = material1;
                flw.material = material2;
                frw.material = material2;
                rlw.material = material2;
                rrw.material = material2;

                bbody.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                sspoiler.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                flw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                frw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rlw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rrw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
            }
        }
        else if (occupancy == Option._90)
        {
            Vector3[] randomValues = new Vector3[96];

            for (int i = 0; i < randomValues.Length; i++)
            {
                Vector3 randomValue = pg_pos[Random.Range(0, pg_pos.Length)];

                while (randomValues.Contains(randomValue))
                {
                    randomValue = pg_pos[Random.Range(0, pg_pos.Length)];
                }

                randomValues[i] = randomValue;
            }

            foreach (Vector3 position in randomValues) 
            {
                GameObject go = Instantiate(car_blue, position, Quaternion.identity);
                MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
                //MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                //foreach (MeshRenderer renderer in meshRenderers)
                //{
                //    Debug.Log(renderer);
                //}

                // Access the mesh renderer of the first child game object
                MeshRenderer bbody = meshRenderers[0];
                Material[] bbodyMaterials = bbody.materials;

                // Apply the material to the second element in the materials array of bbody
                if (bbodyMaterials.Length > 1)
                {
                    bbodyMaterials[1] = material1;
                    bbody.materials = bbodyMaterials;
                }
                // Access the mesh renderer of the second child game object
                MeshRenderer sspoiler = meshRenderers[1];

                // Access the mesh renderer of the third child game object
                MeshRenderer flw = meshRenderers[2];
                MeshRenderer frw = meshRenderers[3];
                MeshRenderer rlw = meshRenderers[4];
                MeshRenderer rrw = meshRenderers[5];



                bbody.material = material1;
                sspoiler.material = material1;
                flw.material = material2;
                frw.material = material2;
                rlw.material = material2;
                rrw.material = material2;

                bbody.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                sspoiler.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                flw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                frw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rlw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rrw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
            }
        }
        else if (occupancy == Option._100)
        {
            foreach (Vector3 position in pg_pos)
            {
                GameObject go = Instantiate(car_blue, position, Quaternion.identity);
                MeshRenderer[] meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
                //MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
                //foreach (MeshRenderer renderer in meshRenderers)
                //{
                //    Debug.Log(renderer);
                //}

                // Access the mesh renderer of the first child game object
                MeshRenderer bbody = meshRenderers[0];
                Material[] bbodyMaterials = bbody.materials;

                // Apply the material to the second element in the materials array of bbody
                if (bbodyMaterials.Length > 1)
                {
                    bbodyMaterials[1] = material1;
                    bbody.materials = bbodyMaterials;
                }
                // Access the mesh renderer of the second child game object
                MeshRenderer sspoiler = meshRenderers[1];

                // Access the mesh renderer of the third child game object
                MeshRenderer flw = meshRenderers[2];
                MeshRenderer frw = meshRenderers[3];
                MeshRenderer rlw = meshRenderers[4];
                MeshRenderer rrw = meshRenderers[5];



                bbody.material = material1;
                sspoiler.material = material1;
                flw.material = material2;
                frw.material = material2;
                rlw.material = material2;
                rrw.material = material2;

                bbody.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                sspoiler.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                flw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                frw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rlw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
                rrw.renderingLayerMask = (uint)(1 << 5) | (uint)(1 << 6);
            }

            
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}