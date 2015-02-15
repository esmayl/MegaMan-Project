using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelCamera : MonoBehaviour {

    public GameObject player;
    public Image hpBar;
    public Image barForeground;
    public Image chargeBar;


    Image[] hp = new Image[10];
    Image[] charges = new Image[10];
    RaycastHit hit;

    void Start()
    {
        //if (player.GetComponent<PlayerMovement>().hp > 0)
        //{
        //    for (int i = 0; i < player.GetComponent<PlayerMovement>().hp / 10; i++)
        //    {
        //        Image overlay = Image.Instantiate(barForeground) as Image;
        //        overlay.rectTransform.position = hpBar.rectTransform.position;
        //
        //        
        //    }
        //}
    }

    void LateUpdate()
    {
        SafeFrameCheck();


    }

    public void MoveForward()
    {
        transform.position += Time.deltaTime * transform.forward*2;

    }
    public void MoveBackward()
    {
        transform.position -= Time.deltaTime * transform.forward*2;
    }

    bool SafeFrameCheck()
    {
        Vector3 screenPos = transform.GetChild(0).camera.WorldToScreenPoint(player.transform.position);
        float ratio = screenPos.x / transform.GetChild(0).camera.pixelWidth;
        if (ratio > 0.6f)
        {
            MoveForward();
            return false;
        }
        if (ratio < 0.19f)
        {
            MoveBackward();
            return false;
        }
        return true;
    }
}
