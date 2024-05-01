using PsychoticLab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterComponentLister : MonoBehaviour
{
    [Header("Component Lists")]
    public CharacterComponentGroup lightComponents; 
    public CharacterComponentGroup mediumComponents;
    public CharacterComponentGroup heavyComponents;
    [System.Serializable]
    public class PreBuiltHead
    {
        public GameObject head;
        public GameObject eyebrow;
        public GameObject facialHair;
        public GameObject accessory;
    }
    [System.Serializable]
    public class PreBuiltChest
    {
        public GameObject torso;
        public GameObject armUpperRight;
        public GameObject armUpperLeft;
        public GameObject armLowerRight;
        public GameObject armLowerLeft;
        public GameObject handRight;
        public GameObject handLeft;
        public GameObject shoulderRight;
        public GameObject shoulderLeft;
        public GameObject elbowRight;
        public GameObject elbowLeft;
        public GameObject backAttachment;
    }
    [System.Serializable]
    public class PreBuiltLegs
    {
        public GameObject hips;
        public GameObject legRight;
        public GameObject legLeft;
        public GameObject kneeAttachmentRight;
        public GameObject kneeAttachmentLeft;
        public GameObject hipsAttachment;
    }
    [System.Serializable]
    public class CharacterComponentGroup
    {
        public List<PreBuiltHead> head;
        public List<GameObject> hair;
        public List<PreBuiltChest> chest;
        public List<PreBuiltLegs> legs;
    }

    public Color[] skinColours;
    public Color[] bodyArtColours;
    public Color[] hairColours;
    public Color[] primaryColours;
    public Color[] secondaryColours;
    public Color[] metalDarkColours;
    public Color[] leatherColours;
    public Color[] leatherSecondaryColours;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
