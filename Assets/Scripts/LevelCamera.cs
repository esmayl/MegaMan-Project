using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelCamera : MonoBehaviour {

    public GameObject player;
    public Image hpBar;
    public Image barForeground;
    public Image chargeBar;


    Image[] hp = new Image[9];
    Image[] charges = new Image[9];
    RaycastHit hit;

    void Start()
    {
        if (player.GetComponent<PlayerMovement>().hp > 0)
        {
            for (int i = 1; i < 9; i++)
            {
                if (i == 1) { hp[0] = barForeground; }
                Image temp = (Image)Image.Instantiate(barForeground);
                temp.rectTransform.SetParent(hpBar.transform,false);
                temp.rectTransform.position = barForeground.rectTransform.position;
                temp.rectTransform.localScale = new Vector3(1, 1, 1);
                temp.rectTransform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                Vector3 tempLocation = temp.rectTransform.position+(new Vector3(0,1,0)*(0.047f * i));
                temp.rectTransform.position = tempLocation;
                hp[i] = temp;
            }
        }
    }

    void RemoveHP()
    {
        if (player.GetComponent<PlayerMovement>().hp < 100)
        {
            for (int i = (int)player.GetComponent<PlayerMovement>().hp/10; i < 9; i++)
            {
                Destroy(hp[i]);
            }
        }
    }

    void LateUpdate()
    {
        SafeFrameCheck();
        RemoveHP();

    }

    public void MoveForward()
    {
        transform.position += Time.deltaTime * transform.forward*2;

    }
    public void MoveBackward()
    {
        transform.position -= Time.deltaTime * transform.forward*2;
    }
    public void MoveDown()
    {
        transform.position += Time.deltaTime * -transform.up * 2;
    }
    public void MoveUp()
    {
        transform.position += Time.deltaTime * transform.up * 2;
    }

    bool SafeFrameCheck()
    {
        Vector3 screenPos = transform.GetChild(0).camera.WorldToScreenPoint(player.transform.position);
        float ratio = screenPos.x / transform.GetChild(0).camera.pixelWidth;
        float ratioY = screenPos.y / transform.GetChild(0).camera.pixelHeight;
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
        if (ratioY > 0.75f)
        {
            MoveUp();
            return false;
        }
        if (ratioY < 0.15f)
        {
            MoveDown();
            return false;
        }
        return true;
    }
}
