using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueDataSQ", menuName = "Scriptable Objects/DialogueDataSQ")]
public class DialogueDataSQ : ScriptableObject
{
    [Header("캐릭터 정보")]
    public string characterName = "캐릭터";
    public Sprite characterImage;

    [Header("대화내용")]
    [TextArea(3,10)]
    public List<string> dialogueLines = new List<string>();

}
