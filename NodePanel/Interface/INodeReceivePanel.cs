using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace PanelNode
{
    public interface INodeReceivePanel
    {
        bool CanOpen();
        void HandleOpenData(object data);
        bool CloseAble();
    }
}