using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRxPool;
using UnityEngine;
using Random=System.Random;
using UniRx;
public class BlocksGenerator: MonoBehaviour
{
    //[SerializeField]GameObject block;
    [SerializeField]Vector2Int blockSize ;
    [SerializeField]int generateCount;
    [SerializeField]Canvas parent;
    [SerializeField]GameObject oxygen;
    [SerializeField]GameObject hydrogen;
    [SerializeField]int xNumber;
    [SerializeField]int yNumber;
    void Start()
    {
        Generate();
    }
    List<IAtom> atomList=new List<IAtom>();
    Dictionary<Vector2Int,bool> popDictionary=new Dictionary<Vector2Int, bool>(); 
    void Generate(){
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        Dictionary<Type,GameObject> atomDictionary=new Dictionary<Type, GameObject>(){
        {typeof(Oxygen),oxygen},
        {typeof(Hydrogen),hydrogen},
        };
        Vector3 basePos=Vector3.zero;
        Vector2Int maxRange=new Vector2Int(xNumber/2,yNumber/2);
        Vector2Int minRange=new Vector2Int(-xNumber/2,-yNumber/2);
        Random random=new Random();
        for (int n = 0; n < generateCount; n++)
        {
                int i,j;
                do{
                i=random.Next()%(int)(maxRange.x-minRange.x)+(int)minRange.x;
                j=random.Next()%(int)(maxRange.y-minRange.y)+(int)minRange.y;
                }
                while(popDictionary.ContainsKey(new Vector2Int(i,j)));
                popDictionary[new Vector2Int(i,j)]=true;
                Vector3 addPos=new Vector3((i+1/2f)*blockSize.x,(j+1/2f)*blockSize.y,0);
                //var blockObj=GameObject.Instantiate(block,basePos+addPos,Quaternion.identity);
                //blockObj.transform.parent=parent.transform;
                int mode=random.Next()%2;
                Type atomType=getAtomType(mode);
                var atom=atomDictionary[atomType];//getAtomPrefab(atomType);
                var atomObj=GameObject.Instantiate(atom,basePos+addPos,Quaternion.identity);
                atomObj.transform.parent=parent.transform;
                OverrideSorting(atomObj);
                Vector2Int startPosition=new Vector2Int(i,j);
                //Debug.Log(startPosition);
                IAtom atomInstance=null;
                switch(mode){
                    case 0:atomInstance=new Oxygen(atomObj.transform,blockSize,startPosition);break;
                    case 1:atomInstance=new Hydrogen(atomObj.transform,blockSize,startPosition);break;
                    default:throw new Exception();
                }
                atomList.Add(atomInstance);
        }
        sw.Stop();
        Debug.Log(sw.ElapsedMilliseconds + "ms");
        StreamPool.Instance.FindObserver<InitData>("initData").OnNext(
            new InitData(atomList,minRange,maxRange)
        );
    }
    async UniTask OverrideSorting(GameObject obj){
        await UniTask.Delay(200);
        obj.GetComponent<Canvas>().overrideSorting=true;
    }
    Type getAtomType(int mode){
        switch(mode){
                    case 0:return typeof(Oxygen);
                    case 1:return typeof(Hydrogen);
                    default:break;
                }
        return null;
    }
}