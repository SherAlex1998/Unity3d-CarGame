using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public GameObject player;
    public GameObject[] massObj;
    private GameObject[] massObjOnScene;
    public float sizeofx,sizeofy;
    private int countForward, countBackward,current;
    public int forward, backward;
    int setindex,delindex;
    private bool GenDone;
    // Start is called before the first frame update
    void Start()
    {
        massObjOnScene = new GameObject[backward+forward+1];
        OneStepGen();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GenDone) return;
        //if (!(countForward >= current + forward)) return;
        //Debug.Log(player.transform.position.x);
        current = (int) (player.transform.position.x / sizeofx);
        OneStepGen();
        DeleteBackward();
        }
    void OneStepGen()
    {
        GenDone = false;
        int index;
        while (countForward <=  current + forward)
        {
           index = countForward % massObjOnScene.Length;
            //Debug.Log(index);
            massObjOnScene[index] = PoolManager.getGameObjectFromPool(massObj[Random.Range(0, massObj.Length)]);
            massObjOnScene[index].transform.position = new Vector3(sizeofx*countForward, sizeofy,0);
            countForward++;
        }
        GenDone = true;
    }

    void DeleteBackward()
    {
        int index;
        while (countBackward  <=current - backward)
        {   index = countBackward % massObjOnScene.Length;
            PoolManager.putGameObjectToPool(massObjOnScene[index]);
            countBackward++;
        }
    }
}
