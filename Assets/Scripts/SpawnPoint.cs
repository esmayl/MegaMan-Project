using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

    public GameObject player;
    public GameObject camera;

	void Start () {
        GameObject playerInstance = new GameObject();

        if (!player.GetComponent<CharacterController>()) { player.AddComponent<CharacterController>();}
        if (player) { playerInstance  = Instantiate(player, transform.position, Quaternion.identity)as GameObject;}
        if (camera) { camera.GetComponent<LevelCamera>().player = playerInstance; Instantiate(camera, transform.position + new Vector3(11, 0, 0), Quaternion.identity); }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
