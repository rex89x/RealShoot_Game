using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class RealShoot : MonoBehaviour
{
    private Camera _camera;
    public SerialPort sp = new SerialPort("COM3", 9600);
    float x, y;
    int getShoot = 0;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private Transform playerBody;

    private float xAxisClamp;

    void Start()
    {
        _camera = GetComponent<Camera>();
        sp.Open();
        sp.ReadTimeout = 1000;
    }
    void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }

    private void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;
    }


    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        CameraRotation();
        if (sp.IsOpen)
        {
            char[] delimiterChars = { ',' };
            string param = sp.ReadLine();
            string[] words = param.Split(delimiterChars);
            for (int i = 0; i <= 2; i++)
            {
                if (i == 0) { x = float.Parse(words[i]); }
                if (i == 2) { y = float.Parse(words[i]); }
            }
            if (int.Parse(words[3]) == 1)
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
                    }
                    else
                    {
                        StartCoroutine(F(hit.point));//用協程造子彈，因為要編寫子彈要1s後消失的效果
                    }
                }
            }
            else
            {

            }
        }
    }

    private void CameraRotation()
    {
        //y軸控制
        if (y < -50) { y = 3; }
        else if (y < -30 && y >= -50) { y = 2; }
        else if (y < -15 && y >= -30) { y = 1; }
        else if (y > 50) { y = -3; }
        else if (y > 30 && y <= 50) { y = -2; }
        else if (y > 15 && y >= 30) { y = -1; }
        else { y = 0; }
        //x軸控制
        if (x > 60) { x = -4; }
        else if (x > 40 && x <= 60) { x = -3; }
        else if (x > 25 && x <= 40) { x = -2; }
        else if (x > 10 && x <= 25) { x = -1; }
        else if (x < -10 && x >= -25) { x = 1; }
        else if (x < -25 && x >= -40) { x = 2; }
        else if (x < -40 && x >= -60) { x = 3; }
        else if (x < -60) { x = 4; }
        else { x = 0; }


        float mouseX = x * mouseSensitivity * Time.deltaTime;
        float mouseY = y * mouseSensitivity * Time.deltaTime;

        xAxisClamp += mouseY;

        if (xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if (xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }
    private IEnumerator F(Vector3 pos)//協程
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;
        yield return new WaitForSeconds(0.001f);
        Destroy(sphere);//1s後消失
    }
}
