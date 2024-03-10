using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackParticles : MonoBehaviour
{
    public List<ParticleEffect> Particles;
    public GameObject User;
    public List<GameObject> Targets;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUser(GameObject user)
    {
        User = user;
    }

    public void SpawnParticle(string particleName)
    {
        for(int i = 0; i < Particles.Count; i++)
        {
            if(particleName == Particles[i].ParticleName)
            {
                var particle = Particles[i];
                if(particle.ForSeveralRanges)
                {
                    for(int j = 0; j < particle.RangesWithParticle.Count; j++)
                    {
                        CreateParticle(particle, particle.RangesWithParticle[j]);
                    }
                }
                else
                {
                    CreateParticle(particle, 0);
                }
            }
        }
    }

    private void CreateParticle(ParticleEffect particle, int hitboxNumber)
    {
        //For particles spawned from the user
        if(particle.SpawnType == ParticleSpawnType.FromUserPos)
        {
            //Move effect to specific bone (maybe should be listed in ParticleEffect.cs)
            var system = Instantiate(particle.Effect, User.transform.position + particle.OffsetPosition, User.transform.rotation * particle.OffsetRotation); 
            //Set parent to specific bone
            system.transform.SetParent(this.transform.parent);
        }
        //For particles spawned on the target
        else if(particle.SpawnType == ParticleSpawnType.OnTargetPos)
        {
            Debug.Log("OnTargetPos Particles not implemented yet");
        }
        else if(particle.SpawnType == ParticleSpawnType.FromUserBones)
        {
            var armature = User?.transform.GetChild(4)?.Find("Armature");
            if(armature == null)
            {
                return;
            }
            for(int i = 0; i < particle.BoneSpawnPoints.Count; i++)
            {
                Transform spawnPoint = RecursiveFindChild(armature, particle.BoneSpawnPoints[i]);
                if(spawnPoint != null)
                {
                    var system = Instantiate(particle.Effect, spawnPoint.position + particle.OffsetPosition, spawnPoint.rotation * particle.OffsetRotation); 
                    //Set parent to specific bone?
                    system.transform.SetParent(spawnPoint);
                }
            }
        }
        else if(particle.SpawnType == ParticleSpawnType.OnTargetBones)
        {
            Debug.Log("OnTargetBones Particles not implemented yet");
        }
    }

    Transform RecursiveFindChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if(child.name == childName)
            {
                return child;
            }
            else
            {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null)
                {
                    return found;
                }
            }
        }
    return null;
    }


}
