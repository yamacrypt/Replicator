using System.Linq;
using UnityEngine;

public class LineRenderingAction{
    public IBaseAtom start{get;}
    public IBaseAtom end{get;}
    public LineRenderingAction(IBaseAtom start, IBaseAtom end,bool writeMode=true)
    {
        
        this.writeMode = writeMode;
        this.start = start;
        this.end = end;
    }

    public bool writeMode{get;}
}