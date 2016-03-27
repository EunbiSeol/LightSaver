using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class gy521send_personalized : MonoBehaviour {
    SerialPort sp = new SerialPort("COM3", 9600);  //set Serial port
    int check;
    static int size = sizeof(float) * 3 + 1;
    byte[] received = new byte[size];
    int buttonCheck=0;
    float x, y, z;
    int[] angle = new int[2];

    // Use this for initialization
    void Start()
    {
        sp.Open();  //Serial port open
        sp.ReadTimeout = 1;  //set Serial timeout
    }

    // Update is called once per frame
    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                sp.Write("s");  //send start data
                check = sp.ReadByte();  //read a byte
                if (check == 0xff)
                {
                        //check start byte
                        for (int i = 0; i < size; i++)
                        {
                            received[i] = (byte)sp.ReadByte();
                        }

                        x = System.BitConverter.ToSingle(received, 0);
                        x = -x;
                        y = System.BitConverter.ToSingle(received, (size-1)/3);
                        z = System.BitConverter.ToSingle(received, (size - 1) / 3 * 2);

                        //transform.Rotate(x, y, z);
                        //transform.RotateAround(this.transform.FindChild("main").position, Vector3.up, val[0]);

                        transform.rotation = Quaternion.Euler(x, y, z);  //rotate object
                    if(received[size-1] == 0xff)
                    {
                        buttonCheck += 1;
                        if(buttonCheck % 2 == 1)
                        {
                            this.transform.GetChild(0).gameObject.SetActive(true);
                        }
                        else
                        {
                            this.transform.GetChild(0).gameObject.SetActive(false);
                        }
                        
                    }
                     
                    
                }
            }
            catch (System.Exception) { }
        }
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }
}
