using UnityEngine;

public class Hydrogen : Atom
{
    public Hydrogen(Transform transform, Vector2Int blockSize, Vector2Int startPosition) : base(transform, blockSize, startPosition)
    {
    }

    public override int fieldPower => 1;

    protected override int bondOrderBaseValue => 1;

    protected override int suctionPowerBaseValue => 1;

    public override int reactivity(IReactiveAtom target)
    {
        throw new System.NotImplementedException();
    }
}