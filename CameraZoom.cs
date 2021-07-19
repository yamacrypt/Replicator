using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
  Transform tf; //Main CameraのTransform
  Camera cam; //Main CameraのCamera
  float defaultSize;
  void Start()
  {
    tf = this.gameObject.GetComponent<Transform>(); //Main CameraのTransformを取得する。
    cam = this.gameObject.GetComponent<Camera>(); //Main CameraのCameraを取得する。
    defaultSize=cam.orthographicSize;
  }
    [SerializeField]float zoomSpeed=100f;
    [SerializeField]float basetransitSpeed=0.1f;
  void Update()
  {
    var scroll = Input.mouseScrollDelta.y;
    cam.orthographicSize -= cam.orthographicSize * scroll * zoomSpeed;
    var transitSpeed=basetransitSpeed*cam.orthographicSize/defaultSize;
    if(Input.GetKey(KeyCode.UpArrow)) //上キーが押されていれば
    {
      tf.position = tf.position + new Vector3(0.0f,1.0f*transitSpeed,0.0f); //カメラを上へ移動。
    }
    else if(Input.GetKey(KeyCode.DownArrow)) //下キーが押されていれば
    {
      tf.position = tf.position + new Vector3(0.0f,-1.0f*transitSpeed,0.0f); //カメラを下へ移動。
    }
    if(Input.GetKey(KeyCode.LeftArrow)) //左キーが押されていれば
    {
      tf.position = tf.position + new Vector3(-1.0f*transitSpeed,0.0f,0.0f); //カメラを左へ移動。
    }
    else if(Input.GetKey(KeyCode.RightArrow)) //右キーが押されていれば
    {
      tf.position = tf.position + new Vector3(1.0f*transitSpeed,0.0f,0.0f); //カメラを右へ移動。
    }
  }
}