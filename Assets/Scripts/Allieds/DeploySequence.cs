using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeploySequence : ScriptableObject
{
    public abstract IEnumerator SequenceCoroutine(MonoBehaviour runner);
}
