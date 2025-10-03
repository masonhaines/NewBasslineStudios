using UnityEngine;

public interface ITarget
{
    public event System.Action OnTargetReachedCaller;
    bool bHasReachedTarget { get; set; }
    void NewTargetLocation(Vector2 moveToTargetLocation);
    void OnTick();
}