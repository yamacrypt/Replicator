using System.Collections.Generic;
using UnityEngine;

public class InitData:IMovingInitData,IReactiveInitData{
    public Vector2Int minRange{get;}
    public Vector2Int maxRange{get;}
    public List<IAtom> atomList{get;}

    IReadOnlyList<IReactiveAtom> IReactiveInitData.atomList => atomList;

    IReadOnlyList<IMovingAtom> IMovingInitData.atomList => atomList;

    public InitData(List<IAtom> atomList, Vector2Int minRange, Vector2Int maxRange)
    {
        this.atomList = atomList;
        this.minRange = minRange;
        this.maxRange = maxRange;
    }
}
public interface IMovingInitData{

    Vector2Int minRange{get;}    
    Vector2Int maxRange{get;}    
    IReadOnlyList<IMovingAtom> atomList{get;}
}
public interface IReactiveInitData{

    //Vector2Int minRange{get;}    
    //Vector2Int maxRange{get;}  
    IReadOnlyList<IReactiveAtom> atomList{get;}
}