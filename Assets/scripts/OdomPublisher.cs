using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Tf2;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Nav;
using UnityEditor.Callbacks;
using UnityEngine.TextCore;

public class OdomPublisher : MonoBehaviour
{
    public Rigidbody base_link_rb;
    public float Frequency = 0.1f;

    private float timeElapsed = 0;
    private ROSConnection ros;
    private string topic_name = "odom";

    private OdometryMsg odom_msg = new OdometryMsg();
    private bool is_tf = false;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<TFMessageMsg>("tf", tf_CB);
        ros.RegisterPublisher<OdometryMsg>(topic_name);

        odom_msg.child_frame_id = "map";
        odom_msg.header.frame_id = "map";
    }

    void tf_CB(TFMessageMsg msg)
    {
        foreach (TransformStampedMsg tf in msg.transforms)
        {
            if (tf.child_frame_id == "base_link")
            {
                // pose
                odom_msg.pose.pose.position.x = tf.transform.translation.x;
                odom_msg.pose.pose.position.y = tf.transform.translation.y;
                odom_msg.pose.pose.position.z = tf.transform.translation.z;

                odom_msg.pose.pose.orientation.x = tf.transform.rotation.x;
                odom_msg.pose.pose.orientation.y = tf.transform.rotation.y;
                odom_msg.pose.pose.orientation.z = tf.transform.rotation.z;
                odom_msg.pose.pose.orientation.w = tf.transform.rotation.w;
            }
        }

        is_tf = true;
    }


    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > Frequency && is_tf)
        {
            odom_msg.twist.twist.linear = base_link_rb.velocity.To<FLU>();
            odom_msg.twist.twist.angular = base_link_rb.angularVelocity.To<FLU>();

            ros.Publish(topic_name, odom_msg);

            timeElapsed = 0;
        }

        Resources.UnloadUnusedAssets();
            
    }
}
