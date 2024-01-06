
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;

public class HelloWorldSubscriber : MonoBehaviour
{
    ROSConnection ros;

    public string topic_name = "hello_unity";

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<StringMsg>(topic_name, msg_CB);
    }

    // Update is called once per frame
    void msg_CB(StringMsg msg)
    {
        Debug.Log(msg.data);
    }
}
