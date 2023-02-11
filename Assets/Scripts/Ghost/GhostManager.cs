using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : Singleton<GhostManager>
{
    Transform ghostHome;

    public override bool ShouldDestoryOnLoad()
    {
        return true;
    }
}
