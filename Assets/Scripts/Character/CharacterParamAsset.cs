using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character,Data", menuName = "ScriptableObjects/CreateCharacterParamAsset")]
public class CharacterParamAsset : ScriptableObject
{
    //public List<CharacterParam> CharacterParamList = new List<CharacterParam>();

    // –¼‘O(String•Ï”‚Íã‚É‘‚¢‚½•û‚ª—Ç‚¢)
    public string CharacterName = "Temp";

    // ˆÚ“®‘¬“x
    [SerializeField]
    public float MoveSpeed;

    // ‰ñ“]‘¬“x
    [SerializeField]
    public float RotateSpeed;

    // UŒ‚—Í
    [SerializeField]
    public float Attack;
}

[System.Serializable]
public class CharacterParam
{
    
}