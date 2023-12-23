using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.Nav;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using System;
using UnityEngine.UIElements;
using UnityEditor;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class PathSubscriber : MonoBehaviour
{
    ROSConnection ros;

    private List<Vector3> path = new List<Vector3>();

    private bool is_path = false;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.Subscribe<PathMsg>("/global_path", path_CB);
        
    }

    // Update is called once per frame
    void path_CB(PathMsg msg)
    {
        is_path = false;
        path.Clear();
        foreach (PoseStampedMsg pose in msg.poses)
        {
            Vector3 position = pose.pose.position.From<FLU>();
            
            path.Add(position);
        }
        is_path = true;
    }

    void Update()
    {
        if (is_path)
        {
            int i = 0;
            foreach(Vector3 pose in path)
            {
                if (i < path.Count-1)
                {
                    Debug.DrawLine(pose, path[i+1]);
                
                }

                i++;
                
            }
            
        }

    }
}
