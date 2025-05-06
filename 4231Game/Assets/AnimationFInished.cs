using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationFInished")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "AnimationFInished", message: "[Animation] has finished", category: "Action/Animation", id: "d9b45a4a818ab77299c1084bedd158bd")]
public partial class AnimationFInished : EventChannelBase
{
    public delegate void AnimationFInishedEventHandler(GameObject Animation);
    public event AnimationFInishedEventHandler Event; 

    public void SendEventMessage(GameObject Animation)
    {
        Event?.Invoke(Animation);
    }

    public override void SendEventMessage(BlackboardVariable[] messageData)
    {
        BlackboardVariable<GameObject> AnimationBlackboardVariable = messageData[0] as BlackboardVariable<GameObject>;
        var Animation = AnimationBlackboardVariable != null ? AnimationBlackboardVariable.Value : default(GameObject);

        Event?.Invoke(Animation);
    }

    public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
    {
        AnimationFInishedEventHandler del = (Animation) =>
        {
            BlackboardVariable<GameObject> var0 = vars[0] as BlackboardVariable<GameObject>;
            if(var0 != null)
                var0.Value = Animation;

            callback();
        };
        return del;
    }

    public override void RegisterListener(Delegate del)
    {
        Event += del as AnimationFInishedEventHandler;
    }

    public override void UnregisterListener(Delegate del)
    {
        Event -= del as AnimationFInishedEventHandler;
    }
}

