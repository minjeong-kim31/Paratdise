using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 작성자 : 조영민
/// 최초작성일 : 2022/03/28
/// 최종수정일 : 
/// 설명 : 
/// 
/// 캐릭터의 타입과 해금 여부 데이터 
/// </summary>
namespace YM
{
    [System.Serializable]
    public struct CharacterData
    {
        public CharacterType type;
        public bool isAvailable;
    }

    [System.Serializable]
    public enum CharacterType
    {
        None,
        Mise,
        Laila,
        ggabirilldjo,
        Eily
    }
}
