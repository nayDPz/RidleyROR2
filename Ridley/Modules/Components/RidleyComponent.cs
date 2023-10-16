using UnityEngine;
using System.Collections;
using RoR2;
public class RidleyComponent : MonoBehaviour
{
    float leftFootPositionWeight;
    float leftFootRotationWeight;
    Transform leftFootObj;


    private Animator animator;
    private ModelLocator modelLocator;
    private Transform modelTransform;
    public Transform weaponBase;
    private Transform weaponExtender;
    public Transform[] tailBones;

    private bool overrideRotation;
    private Vector3 weaponPointOverride;

    public Vector3 vector = new Vector3(90f, 0f, 0f);
    void Start()
    {
        this.tailBones = new Transform[6];
        this.modelLocator = base.GetComponent<ModelLocator>();
        this.modelTransform = this.modelLocator.modelTransform;
        this.GetTransforms();
    }


    private void GetTransforms()
    {
        if (this.modelTransform)
        {
            Transform tail = modelTransform.Find("model-armature/Trans/Rot/Hip/Tail/Tail1/Tail2/Tail3");
            if (tail) tailBones[0] = tail;
            tail = modelTransform.Find("model-armature/Trans/Rot/Hip/Tail/Tail1/Tail2/Tail3/Tail4");
            if (tail) tailBones[1] = tail;
            tail = modelTransform.Find("model-armature/Trans/Rot/Hip/Tail/Tail1/Tail2/Tail3/Tail4/Tail5");
            if (tail) tailBones[2] = tail;
            tail = modelTransform.Find("model-armature/Trans/Rot/Hip/Tail/Tail1/Tail2/Tail3/Tail4/Tail5/Tail6");
            if (tail) tailBones[3] = tail;
            tail = modelTransform.Find("model-armature/Trans/Rot/Hip/Tail/Tail1/Tail2/Tail3/Tail4/Tail5/Tail6/Tail7");
            if (tail) tailBones[4] = tail;
            tail = modelTransform.Find("model-armature/Trans/Rot/Hip/Tail/Tail1/Tail2/Tail3/Tail4/Tail5/Tail6/Tail7/Tail8");
            if (tail) tailBones[5] = tail;
        }
    }

    public void RotationOverride(Vector3 pointToHit)
    {
        Vector3 basePosition = tailBones[0].position;
        Vector3 vector = pointToHit - basePosition;
        Vector3 direction = vector.normalized;
        float distanceBetweenBones = vector.magnitude / tailBones.Length;
        for (int i = 0; i < tailBones.Length; i++)
        {
            if (tailBones[i])
            {
                tailBones[i].position = direction * (distanceBetweenBones * (i));
                tailBones[i].forward = direction;
                tailBones[i].localScale = new Vector3(3, 3, 3);
            }
                
        }

        tailBones[5].position = pointToHit;
        tailBones[5].localScale = new Vector3(8, 3, 8);
        this.overrideRotation = true;
        this.weaponPointOverride = pointToHit;
    }

    public void StopRotationOverride()
    {
        for (int i = 0; i < tailBones.Length; i++)
        {
            if (tailBones[i])
            {
                tailBones[i].localScale = new Vector3(1, 1, 1);
            }

        }
        this.overrideRotation = false;
        this.weaponPointOverride = Vector3.zero;
    }

    void LateUpdate()
    {
        if (this.overrideRotation)
        {
            Vector3 basePosition = tailBones[0].position;
            Vector3 vector = this.weaponPointOverride - basePosition;
            Vector3 direction = vector.normalized;
            float distanceBetweenBones = vector.magnitude / tailBones.Length;
            for (int i = 0; i < tailBones.Length; i++)
            {
                if (tailBones[i])
                {
                    tailBones[i].position = direction * (distanceBetweenBones * i);
                    tailBones[i].forward = direction;
                    tailBones[i].localScale = new Vector3(3, 3, 3);
                }

            }

            tailBones[5].position = this.weaponPointOverride;
            tailBones[5].localScale = new Vector3(8, 3, 8);
        }
    }

}