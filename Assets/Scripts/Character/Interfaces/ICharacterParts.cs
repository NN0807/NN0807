using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterPart
{
    void Initialize(CharacterManager manager);
    void UpdatePart(CharacterManager manager);
}

public interface NetWork_ICharacterPart
{
    void Initialize(NetWork_CharacterManager manager);
    void UpdatePart(NetWork_CharacterManager manager);
}