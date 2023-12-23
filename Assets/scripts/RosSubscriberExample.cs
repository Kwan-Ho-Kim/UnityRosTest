using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosColor = RosMessageTypes.UnityRoboticsDemo.UnityColorMsg;
using RosMessageTypes.Geometry;

public class RosSubscriberExample : MonoBehaviour
{
    ROSConnection ros;

    public GameObject cube;
    public Rigidbody sedan_car;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<RosColor>("color", ColorChange);
        ros.Subscribe<TwistMsg>("sedan_vel", VelControl);
    }

    void ColorChange(RosColor colorMessage)
    {
        cube.GetComponent<Renderer>().material.color = new Color32((byte)colorMessage.r, (byte)colorMessage.g, (byte)colorMessage.b, (byte)colorMessage.a);
    }

    void VelControl(TwistMsg vel_msg)
    {
        sedan_car.velocity = new Vector3((float)vel_msg.linear.x, (float)vel_msg.linear.y, (float)vel_msg.linear.z);
        
    }
}