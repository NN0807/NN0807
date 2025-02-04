using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterPart
{
    void Initialize(CharacterManager manager);
    void UpdatePart(CharacterManager manager);
}