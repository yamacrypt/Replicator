using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Random=System.Random;
public class ReactionManager{
    IReactiveInitData initData;
    IReadOnlyDictionary<Vector2Int,IAtom> atomPosDictionary;

    public ReactionManager(IReactiveInitData initData, IReadOnlyDictionary<Vector2Int, IAtom> atomPosDictionary = null)
    {
        this.initData = initData;
        this.atomPosDictionary = atomPosDictionary;
    }
    bool isAdjacent(Vector2Int pos1,Vector2Int pos2){
        var diff=(Math.Abs(pos1.x-pos2.x)+Math.Abs(pos1.y-pos2.y));
        return diff<2;
    }
    HashSet<Vector2Int> getAdjacentVector(Vector2Int target){
        return new HashSet<Vector2Int>(){
            target+new Vector2Int(1,0),
            target+new Vector2Int(-1,0),
            target+new Vector2Int(0,1),
            target+new Vector2Int(0,-1),
        };
    }
    public void React(){
        foreach(var key in atomPosDictionary.Keys){
            Bond(key);
            //CutOff(key);
            Mutation(key);
        }
    }
    public void Bond(Vector2Int key){
            foreach (var adjacent in getAdjacentVector(key))
            {
                if(atomPosDictionary.Keys.Contains(adjacent)){
                    var start=atomPosDictionary[key];
                    var end=atomPosDictionary[adjacent];
                    Debug.Log($"Bond {key} {adjacent}");
                    bool done=false;
                    if(start.bondOrder>=1&&end.bondOrder>=1)
                        done=start.Bond(end);
                    if(done){
                    UniRxPool.StreamPool.Instance.FindObserver<LineRenderingAction>("LineRenderingAction").OnNext(
                        new LineRenderingAction(
                            start,end
                        )
                    );
                    }
                }                
        }
    }
    public void CutOff(Vector2Int key){
            foreach (var linkedAtom in atomPosDictionary[key].linkedAtoms.ToArray())
            {
                var start=atomPosDictionary[key];
                var end=linkedAtom;
                if(!isAdjacent(start.position,end.position)){
                    //Debug.Log($"CutOff {key} {end.position}");
                    start.CutOff(end);
                    UniRxPool.StreamPool.Instance.FindObserver<LineRenderingAction>("LineRenderingAction").OnNext(
                        new LineRenderingAction(
                            start,end,
                            false
                        )
                    );
                }                
            }
    }
    public void Mutation(Vector2Int key){
        Random random=new Random();
        foreach(var linkedAtom in atomPosDictionary[key].linkedAtoms.ToArray())
        {
            int val=random.Next()%1000;
            bool occur=val<2;
            if(occur){
                var start=atomPosDictionary[key];
                var end=linkedAtom;
                start.CutOff(end);
                    UniRxPool.StreamPool.Instance.FindObserver<LineRenderingAction>("LineRenderingAction").OnNext(
                        new LineRenderingAction(
                            start,end,
                            false
                        )
                    );
            }
        }
    }
}