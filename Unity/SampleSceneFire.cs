using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SampleSceneFire : MonoBehaviour
{
    int count = 16;
    //GUI
    // public
    public int windowWidth = 400;
    public int windowHight = 150;

    // private
    Rect windowRect;
    int windowSwitch = 0;
    float alpha = 0;

    void GUIAlphaColor_0_To_1()
    {
        if (alpha < 1)
        {
            alpha += Time.deltaTime;
            GUI.color = new Color(0, 0, 0);
        }
    }

    // Init
    void Awake()
    {
        windowRect = new Rect(
            (Screen.width - windowWidth) / 2,
            (Screen.height - windowHight) / 2,
            windowWidth,
            windowHight);
    }
    public AudioSource fire;
    void Update()
    {

        //響應左鍵
        if (Input.GetMouseButton(0) == true)
        {
            Vector3 point = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
            Ray ray = _camera.ScreenPointToRay(point);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))//hit就是被擊中的目標
            {
                // Debug.Log(point);
                //獲取打到的物體
                GameObject hitobj = hit.transform.gameObject;
                yDie RT = hitobj.GetComponent<yDie>();
                if (RT != null)
                {
                    RT.Reacttohit();//呼叫敵人的被擊反饋
                    count--;
                }
                else
                {
                    StartCoroutine(F(hit.point));//用協程造子彈，因為要編寫子彈要1s後消失的效果
                }
                Debug.Log(count);
            }
            fire.Play();
        }
        if (count == 0 && windowSwitch == 0)
        {
            windowSwitch = 1;
            alpha = 0; // Init Window Alpha Color
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            count++;
        }
    }

    void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
        if (windowSwitch == 1)
        {
            GUIAlphaColor_0_To_1();
            windowRect = GUI.Window(0, windowRect, QuitWindow, "");

        }
    }

    void QuitWindow(int windowID)
    {
        GUIStyle gStyle = new GUIStyle(GUI.skin.textArea);
        gStyle.fontSize = 30;

        GUI.Label(new Rect(137, 50, 125, 40), "Goodjob", gStyle);



        if (GUI.Button(new Rect(220, 110, 100, 20), "Next"))
        {
            windowSwitch = 0;
            Time.timeScale = 1;
            Application.LoadLevel("ice2");
            
        }
        if (GUI.Button(new Rect(80, 110, 100, 20), "Menu"))
        {
            windowSwitch = 0;
            Application.LoadLevel("game");
            Time.timeScale = 1;
        }


        GUI.DragWindow();
    }
    private Camera _camera;
    // Use this for initialization
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
   
    private IEnumerator F(Vector3 pos)//協程
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        yield return new WaitForSeconds(0.001f);
        Destroy(sphere);//1s後消失
    }
}
