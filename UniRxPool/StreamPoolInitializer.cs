
using System.Collections.Generic;
using UniRxPool;

    static class StreamPoolInitializer{
    static bool registered = false;

            [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
            public static void Register()
            {
                if(registered)
                    return;
                    registered=true;
                    StreamPool.Instance.RegisterStream<InitData>("initData",
                    typeof(BlocksGenerator),typeof(TurnManager)
                    );
                    StreamPool.Instance.RegisterStream<LineRenderingAction>("LineRenderingAction",
                    typeof(ReactionManager),typeof(LineRederingManager)
                    );
                }
    }