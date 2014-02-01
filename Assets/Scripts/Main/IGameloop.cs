using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IGameloop
{
    void OnAwake();
    void OnStart();
    void OnUpdate();
    void OnFixedUpdate();
    void OnLateUpdate();
    void OnDestroy();
}
