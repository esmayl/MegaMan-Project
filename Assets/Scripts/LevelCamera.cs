using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelCamera : MonoBehaviour {

    public GameObject player;
    public Image hpBar;
    public Image barForeground;
    public Image chargeBar;

    float moveSpeed = 2.5f;
    Image[] hp = new Image[9];
    Image[] mp = new Image[9];
    RaycastHit hit;

    void Start()
    {
        if (player.GetComponent<PlayerMovement>().hp > 0)
        {
            for (int i = 1; i < 9; i++)
            {
                if (i == 1) { hp[0] = (Image)Image.Instantiate(barForeground); hp[0].rectTransform.position = barForeground.rectTransform.position; }
                Image temp = (Image)Image.Instantiate(barForeground);
                temp.rectTransform.SetParent(hpBar.transform,false);
                temp.rectTransform.localScale = new Vector3(1, 1, 1);
                temp.rectTransform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                Vector3 tempLocation = temp.rectTransform.position+(transform.up*(0.1125f * i));
                temp.rectTransform.position = tempLocation;
                hp[i] = temp;
            }
            
        }

        if(player.GetComponent<PlayerMovement>().mp >0)
        {
            for (int i = 1; i < 9; i++)
            {
                if (i == 1) { mp[0] = (Image)Image.Instantiate(barForeground); mp[0].rectTransform.position = barForeground.rectTransform.position; }
                Image temp = (Image)Image.Instantiate(barForeground);
                temp.rectTransform.SetParent(chargeBar.transform, false);
                temp.rectTransform.localScale = new Vector3(1, 1, 1);
                temp.rectTransform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                Vector3 tempLocation = temp.rectTransform.position + (transform.up * (0.1125f * i));
                temp.rectTransform.position = tempLocation;
                mp[i] = temp;
            }
        }

        barForeground.gameObject.SetActive(false);

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

    void RemoveMP()
    {
        if (player.GetComponent<PlayerMovement>().mp <= 0) { return; }
        if (player.GetComponent<PlayerMovement>().mp < 100)
        {
            for (int i = (int)player.GetComponent<PlayerMovement>().mp / 10; i < 9; i++)
            {
                Destroy(mp[i]);
            }
        }
    }

    void Update()
    {
        SafeFrameCheck();
        RemoveHP();
        RemoveMP();
    }

    public void MoveForward()
    {
        transform.position += Time.deltaTime * transform.forward*moveSpeed*2;
    }
    public void MoveBackward()
    {
        transform.position -= Time.deltaTime * transform.forward * moveSpeed*2;
    }
    public void MoveDown()
    {
        transform.position -= Time.deltaTime * transform.up * moveSpeed*3.5f;
    }
    public void MoveUp()
    {
        transform.position += Time.deltaTime * transform.up * moveSpeed*1.5f;
    }

    bool SafeFrameCheck()
    {

            Vector3 screenPos = transform.GetChild(0).camera.WorldToScreenPoint(player.transform.position);
            float ratio = screenPos.x / transform.GetChild(0).camera.pixelWidth;
            float ratioY = screenPos.y / transform.GetChild(0).camera.pixelHeight;
            if (ratioY > 0.37f)
            {
                MoveUp();
                return false;
            }
            else if (ratioY < 0.2f)
            {
                MoveDown();
                return false;
            }
            else if (ratio > 0.65f)
            {
                MoveForward();
                return false;
            }
            else if (ratio < 0.45f)
            {
                MoveBackward();
                return false;
            }
            return true;

    }
}
