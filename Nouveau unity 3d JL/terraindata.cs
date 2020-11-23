using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
//using battleia;

public class terraindata : MonoBehaviour
{
    public GameObject mur;
    public GameObject terrain;
    public GameObject robot,explosion;
    public GameObject energie;
    public GameObject centreterrain;
    public int lenght = 10;
    public int width = 10;
    public byte[] mapinfo;
    public bool[] isempty;
    public GameObject[] typeobjs;
    private bool isSelected;
    private bool done = false;
    //private int previousw, previousl;
    private GameObject[] murs;
    // Start is called before the first frame update
    void Start()
    {
        
    }
        // Update is called once per frame
    void Update()
    {
        
        while (!terrainwebsocket.isConnect) return;
        while (!terrainwebsocket.receivedmapinfo)
            {
            done = false;
            return;
            
            }
        if(!done)
            {
            done = true;
            lenght = terrainwebsocket.height;
            width = terrainwebsocket.width;

            
            GenerateTerrain();
            Debug.LogWarning("terminé");
            }

        
        
        updateTerrain();
        
    }
        void GenerateTerrain()
        {
            for (int x = 0; x < lenght; x++)
            {
                for (int z = 0; z < width; z++)
                {

                    // Vector3 positionmur = new Vector3(x, 0, z);
                    Vector3 positionterrain = new Vector3(x, 0.5f, z);
                    //Vector3 rot = new Vector3(0, 0, 0);
                    //GameObject newmur = Instantiate(mur, positionmur, Quaternion.identity) as GameObject;
                    GameObject newterrain = Instantiate(terrain, positionterrain, Quaternion.identity) as GameObject;
                }
            }
            for (int x = -1; x < lenght + 1; x++)
            {
                for (int z = -1; z < width + 1; z++)
                {
                    if (x == -1 || x == lenght || z == -1 || z == width)
                    {


                        Vector3 positionmur = new Vector3(x, 1, z);
                        Vector3 positionmur2 = new Vector3(x, 2, z);
                        //Vector3 positionterrain = new Vector3(x - 10, 0, z - 10);
                        //Vector3 rot = new Vector3(0, 0, 0);
                        GameObject newmur = Instantiate(mur, positionmur, Quaternion.identity) as GameObject;
                        GameObject newmur2 = Instantiate(mur, positionmur2, Quaternion.identity) as GameObject;
                        //GameObject newterrain = Instantiate(terrain, positionterrain, Quaternion.identity) as GameObject;
                    }
                }
            }
        isempty = new bool[lenght * width];
        mapinfo = new byte[lenght * width];
        mapinfo = terrainwebsocket.mapInfos;
        typeobjs = new GameObject[lenght * width];
        centreterrain.transform.position = new Vector3(lenght / 2 - 0.5f, 1, width / 2 - 0.5f);

        Debug.Log("nouveauterrain");
        for (int i = 0; i <= mapinfo.Length; i++)
        {
          
            if (mapinfo[i] == 4 )
            {
                float posobjx = i / width;
                posobjx = Mathf.Floor(posobjx);

                int posobjz = (i % width);

                Vector3 positionobj = new Vector3(posobjx, 1, posobjz);
                //Vector3 positionobj = new Vector3(terrainwebsocket.bx2, 1, terrainwebsocket.by2);
                GameObject bot = Instantiate(robot, positionobj, Quaternion.identity) as GameObject;
                typeobjs[i] = bot;
                isempty[i] = false;


            }
            if (mapinfo[i] == (byte)2 )
            {

                float posobjx = i / width;
                posobjx = Mathf.Floor(posobjx);

                int posobjz = (i % width);

                Vector3 positionobj = new Vector3(posobjx, 1.25f, posobjz);

                GameObject newmur = Instantiate(mur, positionobj, Quaternion.identity) as GameObject;
                typeobjs[i] = newmur;
                isempty[i] = false;
            }
            if (mapinfo[i] == (byte)3 )
            {
                float posobjx = i / width;
                posobjx = Mathf.Floor(posobjx);

                int posobjz = (i % width);
                Vector3 positionobj = new Vector3(posobjx, 0.75f, posobjz);
                GameObject ene = Instantiate(energie, positionobj, Quaternion.identity) as GameObject;
                isempty[i] = false;
                typeobjs[i] = ene;
            }


        }


    }
        void updateTerrain()
        {
            clearcase();

            moveBot();

            addEnergiy();
            
            removePlayer();
            
            


        for (int i = 0; i <= width*lenght; i++)
        {
                if (mapinfo[i] == 0 )
                {
                    isempty[i] = true;
                    if (typeobjs[i] != null)
                    {
                        DestroyImmediate(typeobjs[i]);
                    }
                }
                if (mapinfo[i] == 4 && isempty[i])
                {
                /*calcul coord avant
                    int j = (terrainwebsocket.by1 * width) + terrainwebsocket.bx1;

                    float posobjxav = j / width;
                    posobjxav = Mathf.Floor(posobjxav);

                    int posobjzav = (j % width);
                    Vector3 positionobjav = new Vector3(posobjxav, 1, posobjzav);
                //nouvelle coord
                float posobjx = i / width;
                    posobjx = Mathf.Floor(posobjx);

                    int posobjz = (i % width);

                    Vector3 positionobj = new Vector3(posobjx, 1, posobjz);
                    //Vector3 positionobj = new Vector3(terrainwebsocket.bx2, 1, terrainwebsocket.by2);
                    if (!terrainwebsocket.newbot)
                    {
                     
                    typeobjs[j].transform.position = Vector3.Lerp(positionobjav, positionobj, 1);

                    typeobjs[i] = typeobjs[j];
                    typeobjs[j] = null;
                    isempty[j] = true;
                    isempty[i] = false;
                    mapinfo[j] = 0;
                   // Debug.Log("je translate un  bot");
                    }
                    else
                    {
                    GameObject bot = Instantiate(robot, positionobjav, Quaternion.identity) as GameObject;
                    typeobjs[j] = bot;
                    isempty[j] = false;
                    terrainwebsocket.newbot = false;
                    mapinfo[j] = 4;
                    //Debug.Log("je créé un  bot");
                }  */
                    float posobjx = i / width;
                    posobjx = Mathf.Floor(posobjx);

                    int posobjz = (i % width);

                    Vector3 positionobj = new Vector3(posobjx, 1, posobjz);

                GameObject bot = Instantiate(robot, positionobj, Quaternion.identity) as GameObject;
                typeobjs[i] = bot;
                isempty[i] = false;
                


            }
                if (mapinfo[i] == (byte)2 && isempty[i])
                {

                    float posobjx = i / width;
                    posobjx = Mathf.Floor(posobjx);

                    int posobjz = (i % width);

                    Vector3 positionobj = new Vector3(posobjx, 1.25f, posobjz);

                    GameObject newmur = Instantiate(mur, positionobj, Quaternion.identity) as GameObject;
                    typeobjs[i] = newmur;
                    isempty[i] = false;
                }
                if (mapinfo[i] == (byte)3 && isempty[i])
                {

                /* float posobjx = i / width;
                 posobjx = Mathf.Floor(posobjx);
                 int posobjz = (i % width);
                 Debug.Log(posobjx);
                 Debug.Log(posobjz);
                 Vector3 positionobj = new Vector3(posobjx, 0.5f, posobjz);

                 GameObject ene = Instantiate(energie, positionobj, Quaternion.identity) as GameObject;
                 typeobjs[i] = ene;
                 isempty[i] = false;*/
                //Vector3 positionobj = new Vector3(terrainwebsocket.ey1,0.75f, terrainwebsocket.ex1);
                float posobjx = i / width;
                posobjx = Mathf.Floor(posobjx);
                int posobjz = (i % width);
                
                Vector3 positionobj = new Vector3(posobjx, 0.5f, posobjz);
                GameObject ene = Instantiate(energie, positionobj, Quaternion.identity) as GameObject;
                

                
                isempty[i] = false;
                typeobjs[i] = ene;
                }


            }
        }

    void clearcase()
    {
        int i = (terrainwebsocket.cx1 * width) + terrainwebsocket.cy1;
        //mapinfo[i] = 0;
        //DestroyImmediate(typeobjs[i]);
        //typeobjs[i] = null;
        //isempty[i] = true;
    }
    void moveBot()
    {
        if (terrainwebsocket.needtomovebot)
        {
            int j = (terrainwebsocket.by1 * width) + terrainwebsocket.bx1;

            //Vector3 positionobj = new Vector3(terrainwebsocket.bx2, 1, terrainwebsocket.by2);
            //GameObject bot = Instantiate(robot, positionobj, Quaternion.identity) as GameObject;
            int i = (terrainwebsocket.by2 * width) + terrainwebsocket.bx2;
            //int j = (terrainwebsocket.by1 * width) + terrainwebsocket.bx1;
            
            mapinfo[i] = 4;
            mapinfo[j] = 0;
            DestroyImmediate(typeobjs[j]);
            //isempty[i] = false;
            // typeobjs[i] = bot;
            Debug.LogWarning("bot action bouge en i : " + i);
            terrainwebsocket.needtomovebot = false;
        }
        
    }
    void addEnergiy()
    {
        
       
        int i = (terrainwebsocket.ey1 * width) + terrainwebsocket.ex1;

        mapinfo[i] = 3;
        Debug.LogWarning("enrgie en : " + i);
        mapinfo[0] = 0;
    }
    public void removePlayer()
    {
        if (terrainwebsocket.needtoremove)
        {


            int i = (terrainwebsocket.ry1 * width) + terrainwebsocket.rx1;
            //Debug.LogWarning("bot doit dégagé en : " + i);
            mapinfo[i] = 0;
            float posobjx = i / width;
            posobjx = Mathf.Floor(posobjx);
            int posobjz = (i % width);
            Vector3 positionobj = new Vector3(posobjx, 1, posobjz);
            GameObject boum = Instantiate(explosion, positionobj, Quaternion.identity) as GameObject;
            terrainwebsocket.needtoremove = false;
        }
    }
}
