using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ViewJoint : MonoBehaviour
{
    // Start is called before the first frame update
    public TCPServerScript tcpServerScript;
    public GameObject sphere_prefab;
    private GameObject[] sphere = new GameObject[21];
    private int i = 0;


    // Update is called once per frame
    void Update()
    {
        if (i == 0){
            for (int i = 0; i < 21; i++){
                sphere[i] = Instantiate(sphere_prefab);
            }
        }
        ItemList itemList = tcpServerScript.getHnadPose();
        if (itemList != null){ 
            // Debug.Log(itemList.items[0].x);
            for (int i = 0; i < itemList.items.Length; i++){
                sphere[i].transform.position = new Vector3(-itemList.items[i].x, itemList.items[i].y, itemList.items[i].z);
            }
            // foreach(Item item in itemList.items){
            //     Instantiate(sphere_prefab, new Vector3(item.x,item.y,item.z), Quaternion.identity);
            // // スクリプトでオブジェクトを生成しようとしたらnullになって失敗した
            // // sphere_prefab = Resources.Load<GameObject>("hand_joint");
            // // GameObject sphere = Instantiate(sphere_prefab);
            // // sphere.transform.position = new Vector3(item.x, item.y, item.z);
            // }
    }
    i++;
}
}
