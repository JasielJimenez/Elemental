using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParticleEffect
{
    public string ParticleName;
    public GameObject Effect;
    public ParticleSpawnType SpawnType;
    public List<string> BoneSpawnPoints;
    public Vector3 OffsetPosition;
    public Quaternion OffsetRotation;
    //public Vector3 StartScale;
    
    //See if this particle effect should be spawned for multiple ranges
    public bool ForSeveralRanges;
    //Lists which child ranges should get this particle effect
    public List<int> RangesWithParticle;
    
}
