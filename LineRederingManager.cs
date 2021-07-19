using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class LineRederingManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]GameObject lineObj;
    Dictionary<int,GameObject> rendererDictionary=new Dictionary<int, GameObject>();
    void Start()
    {
       
        UniRxPool.StreamPool.Instance.FindObservable<LineRenderingAction>("LineRenderingAction").Subscribe(
            (action)=>{
                if(action.writeMode){
                    var obj=GameObject.Instantiate(lineObj);
                    var render=obj.GetComponent<LineRenderer>();
                    //Debug.Log($"Bond {action.start.position} {action.end.position}");
                    render.startWidth=10f;
                    render.endWidth=10f;
                    render.SetPosition(0,toVisible(action.start.actualPosition));
                    render.SetPosition(1,toVisible(action.end.actualPosition));
                    action.start.Observable.Subscribe(ele=>{
                            render.SetPosition(0,toVisible(ele));
                    }).AddTo(render);
                    
                    action.end.Observable.Subscribe(ele=>{

                            render.SetPosition(1,toVisible(ele));
                    }).AddTo(render);
                    int[] ids=new int[]{action.start.id,action.end.id};
                    var start=action.start.id;
                    var end=action.end.id;
                    rendererDictionary[Hash(start,end)]=obj;
                }
                else{
                    int[] ids=new int[]{action.start.id,action.end.id};
                    var start=action.start.id;
                    var end=action.end.id;
                    var obj=rendererDictionary[Hash(start,end)];
                    Destroy(obj);
                    

                }
            }
        );
    }
    int Hash(int i ,int j){
        //return HashFunction.CoodinateHash(new Vector2(i,j));
        int mod1=47713;
        return (i%mod1+1)*(j%mod1+1);
    }
    Vector3 toVisible(Vector2 target){
      return new Vector3(target.x,target.y,-1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
