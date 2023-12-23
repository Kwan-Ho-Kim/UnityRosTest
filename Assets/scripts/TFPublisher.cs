using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Tf2;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class TFPublisher : MonoBehaviour
{
    private ROSConnection ros;
    private string topic_name = "tf";

    public GameObject base_link;
    public List<GameObject> sensors = new List<GameObject>();

    public float Frequency = 0.1f;
    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed = 0;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TFMessageMsg>(topic_name);
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > Frequency)
        {
            List<TransformStampedMsg> tfMessageList = new List<TransformStampedMsg>();

            TransformStampedMsg transform_stamped = new TransformStampedMsg(header_make("map"), "base_link", new TransformMsg(base_link.transform.localPosition.To<FLU>(),base_link.transform.localRotation.To<FLU>()));

            tfMessageList.Add(transform_stamped);

            foreach(GameObject sensor in sensors)
            {
                if (sensor.tag == "Untagged")
                {
                    transform_stamped = new TransformStampedMsg(header_make("base_link"), sensor.name, new TransformMsg(sensor.transform.localPosition.To<FLU>(), sensor.transform.localRotation.To<FLU>()));
                }
                else
                {
                    transform_stamped = new TransformStampedMsg(header_make("base_link"), sensor.tag, new TransformMsg(sensor.transform.localPosition.To<FLU>(), sensor.transform.localRotation.To<FLU>()));
                }                

                tfMessageList.Add(transform_stamped);
            }

            var tfMessage = new TFMessageMsg(tfMessageList.ToArray());

            ros.Publish(topic_name, tfMessage);

            timeElapsed = 0;
        }

        Resources.UnloadUnusedAssets();
    }

    HeaderMsg header_make(string frame_id)
    {
        
        HeaderMsg header = new HeaderMsg();
        header.frame_id = frame_id;
        header.stamp.sec = (Int32)Time.time;
        header.stamp.nanosec = (UInt32)((Time.time - header.stamp.sec)*1e+9);

        return header;
    }


}
