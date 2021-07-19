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
    double Tanh(float x){
        float a=0.1f;
        float abs=Math.Abs(x);
        int code=x>=0?1:-1;
        var res=Math.Tanh(code*a*Math.Sqrt(abs));
        if(Math.Abs(res)>1f){
            Debug.Log("error");
            Debug.Log($"x {code*a*Math.Sqrt(abs)}");
            Debug.Log($"res {res}");
        }
        return res;
    }
    public void Bond(Vector2Int key){
            Random random=new Random();
            foreach (var adjacent in getAdjacentVector(key))
            {
                if(atomPosDictionary.Keys.Contains(adjacent)){
                    var start=atomPosDictionary[key];
                    var end=atomPosDictionary[adjacent];
                    //Debug.Log($"Bond {key} {adjacent}");
                    bool done=false;
                    
                    if(start.bondOrder>=1&&end.bondOrder>=1){
                        int basePercentage=50;
                        IReactiveAtom sender,receiver;
                        //if(start.suctionPower*end.injectionPower>start.injectionPower*end.suctionPower){
                        //}
                        //else{
                        sender=start;receiver=end;//ループにより両方向でTryBond操作が行われるからこれで十分。
                        //}
                        
                        int reactionPower=calcReationPower(sender.injectionPower,receiver.suctionPower);
                        int resPercentage=(int)(basePercentage*(1f+Tanh(reactionPower)));
                        int threshold=random.Next()%100;
                        if(resPercentage>threshold){
                            //Debug.Log($"Bond {resPercentage}%");
                            done=start.Bond(end,Atom.Injection);
                        }
                    }
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
    int calcReationPower(int injectionPower,int suctionPower){
        int reactionPower=injectionPower*suctionPower;
        if(injectionPower<0&&suctionPower<0){
            reactionPower=-reactionPower*reactionPower;
        }
        return reactionPower;
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
            var start=atomPosDictionary[key];
            var end=linkedAtom;
            var sender=start;
            var receiver=end;
            int basePercentage=50;
            int reactionPower=calcReationPower(sender.injectionPower,receiver.suctionPower);
            int resPercentage=(int)(basePercentage*(1f-Tanh(reactionPower)));
            int threshold=random.Next()%1000;
            if(resPercentage>threshold){
                
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