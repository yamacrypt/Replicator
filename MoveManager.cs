using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random=System.Random;
public class MoveManager{
    void isMovedDictionaryReset(){
        foreach(var ele in isMovedDictionary.Keys.ToArray())
            isMovedDictionary[ele]=false;

    }
    Dictionary<Vector2Int,int> fieldPowerDictionary;
    public void RandomWalk(){
        
        Random random=new Random();
        foreach(var atom in initData.atomList){
            int direction=random.Next()%5;
            //var weights=getAdjacentWeights(atom);
            switch(direction){
                case Stay:break;
                case Up:Move(atom,1,0);break;
                case Down:Move(atom,-1,0);break;
                case Right:Move(atom,0,1);break;
                case Left:Move(atom,0,-1);break;
            }
        }
        isMovedDictionaryReset();
    }
    void Move(IMovingAtom atom,int x,int y){
        if(MoveCheckAll(atom,x,y)){
            foreach(var ele in movingAtomsList){
                var beforePos=new Vector2Int(ele.position.x,ele.position.y);
                //foreach(var nextPos in getAdjacentVector(atom.position))
                //    fieldPowerDictionary[nextPos]=fieldPowerDictionary.GetOrDefault<Vector2Int, int>(nextPos)-atom.fieldPower;
                ele.Move(x,y);
                //foreach(var nextPos in getAdjacentVector(atom.position))
                //    fieldPowerDictionary[nextPos]=fieldPowerDictionary.GetOrDefault<Vector2Int, int>(nextPos)+atom.fieldPower;
        
                atomPosDictionary[ele.position]=atomPosDictionary[beforePos];
                atomPosDictionary.Remove(beforePos);
            }
        }
    }
    List<Vector2Int> getAdjacentWeights(Vector2Int target){
        return new List<Vector2Int>(){
            target,
            target+new Vector2Int(1,0),
            target+new Vector2Int(-1,0),
            target+new Vector2Int(0,1),
            target+new Vector2Int(0,-1),
        };
    }
    HashSet<IMovingAtom> movingAtomsList;
    bool MoveCheckAll(IMovingAtom atom,int x,int y){
        bool flag=true;
        Queue<IMovingAtom> queue=new Queue<IMovingAtom>();
        queue.Enqueue(atom);
        movingAtomsList=new HashSet<IMovingAtom>();
        while(queue.Count>0){
            var res=queue.Dequeue();
            if(movingAtomsList.Contains(res))
                continue;
            movingAtomsList.Add(res);
            if(flag&&!MoveCheck(res,x,y)){
                flag=false;
            }
            isMovedDictionary[res.id]=true;
            foreach(var ele in res.linkedAtoms)
                queue.Enqueue(ele);
        }
        return flag;
    }
    bool MoveCheck(IMovingAtom atom,int x,int y){
        if(isMovedDictionary[atom.id])
            return false;
        Vector2Int minRange=initData.minRange;
        Vector2Int maxRange=initData.maxRange;
        var transitionPos=atom.position+new Vector2Int(x,y);
        if(!(transitionPos.x>=minRange.x&&transitionPos.x<maxRange.x&&transitionPos.y>=minRange.y&&transitionPos.y<maxRange.y))
            return false;
        if(atomPosDictionary.ContainsKey(transitionPos)&&!movingAtomsList.Contains(atomPosDictionary[transitionPos]))    
            return false;
       
        return true;
    }
    HashSet<Vector2Int> getAdjacentVector(Vector2Int target){
        return new HashSet<Vector2Int>(){
            target+new Vector2Int(1,0),
            target+new Vector2Int(-1,0),
            target+new Vector2Int(0,1),
            target+new Vector2Int(0,-1),
        };
    }
    const int Stay=0;
    const int Up=1;
    const int Down=2;
    const int Left=3;
    const int Right=4;
    IMovingInitData initData;
    IDictionary<Vector2Int,IAtom> atomPosDictionary;
    Dictionary<int,bool> isMovedDictionary;

    public MoveManager(IMovingInitData initData, IDictionary<Vector2Int, IAtom> atomPosDictionary)
    {
        this.initData = initData;
        this.atomPosDictionary = atomPosDictionary;
        //fieldPowerDictionary=new Dictionary<Vector2Int, int>();
        //foreach(var atom in atomPosDictionary.Values)
        //    foreach(var nextPos in getAdjacentVector(atom.position))
        //        fieldPowerDictionary[nextPos]=fieldPowerDictionary.GetOrDefault<Vector2Int, int>(nextPos)+atom.fieldPower;
        isMovedDictionary=new Dictionary<int, bool>();
        foreach(var atom in atomPosDictionary.Values)
           isMovedDictionary[atom.id]=false;
    }
}