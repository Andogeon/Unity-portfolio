using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQuest : QUEST
{
    [SerializeField] private NPC _NPC = null;

    public NPC AccessNPC
    {
        get { return _NPC; }

        set { _NPC = value; }
    }
}
