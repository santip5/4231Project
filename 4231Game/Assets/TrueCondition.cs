using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "True", story: "[I] am [me]", category: "Conditions", id: "3e04cce849ca087f61ecc42621f5efe5")]
public partial class TrueCondition : Condition
{
    [SerializeReference] public BlackboardVariable<int> I;
    [SerializeReference] public BlackboardVariable<int> Me;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
