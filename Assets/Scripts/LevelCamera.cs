using UnityEngine;
using System.Collections;

public class LevelCamera : MonoBehaviour {

    public GameObject player;
    RaycastHit hit;

    void LateUpdate()
    {
        SafeFrameCheck();
    }

    public void MoveForward()
    {
        transform.position += Time.deltaTime * transform.forward;

    }
    public void MoveBackward()
    {
        transform.position -= Time.deltaTime * transform.forward;
    }

    bool SafeFrameCheck()
    {
        Vector3 screenPos = transform.GetChild(0).camera.WorldToScreenPoint(player.transform.position);
        float ratio = screenPos.x / transform.GetChild(0).camera.pixelWidth;
        if (ratio > 0.2f)
        {
            MoveForward();
            return false;
        }
        return true;
    }
}
