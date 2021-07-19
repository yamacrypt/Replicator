using UnityEngine;

public class Carbon : Atom
{
    public Carbon(Transform transform, Vector2Int blockSize, Vector2Int startPosition) : base(transform, blockSize, startPosition)
    {
    }

    public override int fieldPower => 1;

    protected override int bondOrderBaseValue => 4;

    protected override int suctionPowerBaseValue => 1;
    protected override int injectionPowerBaseValue => 1;

    public override int reactivity(IReactiveAtom target)
    {
        throw new System.NotImplementedException();
    }
}