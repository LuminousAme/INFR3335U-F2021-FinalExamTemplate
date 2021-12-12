using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject cameraPrefab;

    [SerializeField] private Vector3 min, max;

    // Start is called before the first frame update
    void Start()
    {
        //generate a random position
        Vector3 randPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        //create a new player at that position
        GameObject NewPlayer = PhotonNetwork.Instantiate(playerPrefab.name, randPos, Quaternion.identity);

        if (NewPlayer.GetComponent<PhotonView>().IsMine)
        {
            //create the camera
            GameObject mainCam = Instantiate(cameraPrefab, randPos, Quaternion.identity);
            //grab a reference to the character controller
            CharacterMovement pc = NewPlayer.GetComponent<CharacterMovement>();

            //the move joystick is the the first child of the hud, which is the second child of the camera
            pc.moveJoyStick = mainCam.transform.GetChild(1).GetChild(0).GetComponent<Joystick>();
            //the look joystick is the the second child of the hud, which is the second child of the camera
            pc.lookJoyStick = mainCam.transform.GetChild(1).GetChild(1).GetComponent<Joystick>();

            //the cimena machine brain is the first child of the camera
            CinemachineVirtualCamera vcam = mainCam.transform.GetChild(0).GetComponent<CinemachineVirtualCamera>();
            vcam.LookAt = pc.LookAtTarget; //the lookat is the first child of the player's first child
            vcam.Follow = NewPlayer.transform; //the follow is simply the player's find child
        }
    }
}
