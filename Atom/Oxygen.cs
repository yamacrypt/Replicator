using UnityEngine;

public class Oxygen : Atom
{
    public Oxygen(Transform transform, Vector2Int blockSize, Vector2Int startPosition) : base(transform, blockSize, startPosition)
    {
    }

    public override int fieldPower => -2;

    protected override int bondOrderBaseValue => 2;

    protected override int suctionPowerBaseValue => -2;

    public override int reactivity(IReactiveAtom target)
    {
        throw new System.NotImplementedException();
    }
}