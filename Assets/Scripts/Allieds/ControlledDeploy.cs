using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deploy 1", menuName ="Custom/Deployments")]
public class ControlledDeploy : DeploySequence
{
    //Variables para controlar la secuencia del despliegue

    public override IEnumerator SequenceCoroutine(MonoBehaviour runner)
    {
        yield return new WaitForSeconds(runner.GetComponent<AlliedManager>().allied.deployTime);
    }

}
