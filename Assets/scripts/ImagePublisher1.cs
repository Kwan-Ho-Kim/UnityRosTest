using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using UnityEngine;
using System;

public class ImagePublisher1 : MonoBehaviour
{

    ROSConnection ros;

    private RenderTexture cam;
    private Camera cam_component;
    public int img_width = 640;
    public int img_height = 480;



    public string compressed_image_topic = "compressed_image";
    // public string image_topic = "image";

    public string frame_id = "camera";

    // Publish the cube's position and rotation every N seconds
    public float ImageFrequency = 0.05f;
    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed = 0;

    private string camera_info_topic = "camera_info";


    // Start is called before the first frame update
    void Start()
    {

        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<CompressedImageMsg>(compressed_image_topic);

        cam_component = GetComponent<Camera>();

        cam = new RenderTexture(img_width, img_height, 3);
        cam.name = "cam_1";

        cam_component.targetTexture = cam;
    
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > ImageFrequency)
        {
            CompressedImageMsg compimg_msg = new CompressedImageMsg();

            compimg_msg.header.frame_id = frame_id;
            compimg_msg.header.stamp.sec = (Int32)Time.time;
            compimg_msg.header.stamp.nanosec = (UInt32)((Time.time - compimg_msg.header.stamp.sec)*1e+9);

            Texture2D img_tex = RenderToTexture2D(cam);
            compimg_msg.format = "png";
            compimg_msg.data = img_tex.EncodeToPNG();
            ros.Publish(compressed_image_topic, compimg_msg);

            timeElapsed = 0;
        }

        Resources.UnloadUnusedAssets();
    }

    Texture2D RenderToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(img_width, img_height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new UnityEngine.Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }


}
