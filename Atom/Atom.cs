using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
public abstract class Atom : IAtom
{
    Vector2Int  blockSize;
    Transform transform;
    public Vector2Int position =>_position;
    public Vector2 actualPosition =>transform.position;
    Vector2Int _position;
    HashSet<IReactiveAtom> _linkedAtoms;
    public int id=>transform.GetInstanceID();
    public IReadOnlyCollection<IReactiveAtom> linkedAtoms => _linkedAtoms;
    private Subject<Vector2> _subject = new Subject<Vector2>();
    
    //Subjectのうち、IObservableだけを公開し、処理を登録できるように
    public IObservable<Vector2> Observable{
    get { return _subject; }
    }

    IReadOnlyCollection<IMovingAtom> IMovingAtom.linkedAtoms => _linkedAtoms;

    public abstract int fieldPower{get;}
    public  int bondOrder=>_bondOrder;
    public  int suctionPower=>_suctionPower;
    protected abstract int bondOrderBaseValue{get;}
    protected abstract int suctionPowerBaseValue{get;}

    int _bondOrder;
    int _suctionPower;
    protected Atom(Transform transform,Vector2Int blockSize,Vector2Int startPosition)
    {
        this.transform=transform;
        this._position=startPosition;
        this.blockSize=blockSize;
        _bondOrder=bondOrderBaseValue;
        _suctionPower=suctionPowerBaseValue;
        _linkedAtoms=new HashSet<IReactiveAtom>();
    }

    public abstract int reactivity(IReactiveAtom target);

    public bool Bond(IReactiveAtom atom, int number = 1)
    {
        if(_linkedAtoms.Contains(atom))
            return false;
        _linkedAtoms.Add(atom);
        _bondOrder-=1;
        atom.Bond(this,number);
        return true;

    }
    public void CutOff(IReactiveAtom atom){
        if(!_linkedAtoms.Contains(atom))
            return;
        _linkedAtoms.Remove(atom);
        _bondOrder=1;
        atom.CutOff(this);
    }

    public void Move(int x, int y)
    {
            _position=_position+new Vector2Int(x,y);
            transform.position=transform.position+new Vector3(x*blockSize.x,y*blockSize.y,0);
            _subject.OnNext(actualPosition);
    }
    public void Mutation()
    {
        throw new System.NotImplementedException();
    }
}
public interface IAtom:IMovingAtom,IReactiveAtom{
   
}
public interface IMovingAtom:IBaseAtom{
    //void Move(int x,int y);
    IReadOnlyCollection<IMovingAtom> linkedAtoms{get;}
}
public interface IReactiveAtom:IMovingAtom{
    bool Bond(IReactiveAtom atom,int number=1);
    void CutOff(IReactiveAtom atom);
    void Mutation();
    int reactivity(IReactiveAtom atom);
    new IReadOnlyCollection<IReactiveAtom> linkedAtoms{get;}
}
public interface IBaseAtom{
    void Move(int x,int y);
    Vector2Int position{get;}
     Vector2 actualPosition{get;}
     
     int id{get;}
    int fieldPower{get;}
    int bondOrder{get;}
    int suctionPower{get;}
     IObservable<Vector2> Observable{get;}
}
