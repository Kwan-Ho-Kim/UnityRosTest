using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;

public class HelloWorldPublisher : MonoBehaviour
{
    ROSConnection ros;
    
    public string topic_name = "hello_ros";

    public float freq = 1f;
    private float TimeElapse = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<StringMsg>(topic_name);
    }

    // Update is called once per frame
    void Update()
    {
        TimeElapse += Time.deltaTime;
        if (TimeElapse > freq)
        {
            StringMsg msg = new StringMsg();
            msg.data = "Hello, ROS";
            ros.Publish(topic_name, msg);

            TimeElapse = 0;

        }
    }

    
}
