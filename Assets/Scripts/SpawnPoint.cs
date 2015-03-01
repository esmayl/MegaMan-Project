using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public GameObject player;
    public GameObject camera;
    public int cameraRange = 8;

	void Start () {
        GameObject playerInstance = new GameObject();

        if (!player.GetComponent<CharacterController>()) { player.AddComponent<CharacterController>();}
        if (player) { playerInstance  = Instantiate(player, transform.position, Quaternion.identity)as GameObject;}
        if (camera) { camera.GetComponent<LevelCamera>().player = playerInstance; Instantiate(camera,new Vector3(cameraRange, player.transform.position.y *2f,  player.transform.position.z/2), Quaternion.identity); }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
