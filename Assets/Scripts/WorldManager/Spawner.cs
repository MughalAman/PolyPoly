using UnityEngine;
using Photon.Pun;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;
    public GameObject CameraGO;
    public bool Debug = false;
    //On start spawn player using Photon
    void Start()
    {
        //deactivate standby camera, spawn player and then activate it's components to fix some bugs (íf you don't do this then the player cameras and inputs don't match your player)
        Camera standbyCamera = CameraGO.GetComponent<Camera>();
        GameObject myPlayerGO = (GameObject)PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-181, 88),20, Random.Range(-51, 160.8f)), Quaternion.identity);
        standbyCamera.enabled = false;
        ((MonoBehaviour)myPlayerGO.GetComponent("Movement")).enabled = true;
        ((MonoBehaviour)myPlayerGO.GetComponent("Weapon")).enabled = true;
        ((MonoBehaviour)myPlayerGO.GetComponent("MouseLook")).enabled = true;

        myPlayerGO.transform.Find("Main Camera").gameObject.SetActive(true);
    }
}
