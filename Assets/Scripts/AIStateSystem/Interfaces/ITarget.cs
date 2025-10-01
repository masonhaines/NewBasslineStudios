using UnityEngine;

public interface ITarget
{
    // bool bHasReachedTarget { get; set; }
    void NewTargetLocation(Vector2 moveToTargetLocation);
}