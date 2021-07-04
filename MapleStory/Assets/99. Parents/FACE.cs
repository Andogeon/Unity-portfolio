using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FACE : PART
{
    public override Vector3 IDLE()
    {
        return Vector3.zero;
    }
    public override Vector3 RUN()
    {
        return Vector3.zero;
    }

    public override Sprite GetSprite()
    {
        return null;
    }
}
