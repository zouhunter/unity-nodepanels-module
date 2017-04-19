using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace NodePanels
{
    public sealed class NodePanel : MonoBehaviour
    {
        public Button closeBtn;
        public OpenType openType;
        public List<NodePanelPair> relatedPanels;
        private UnityAction onClose;
        private void Awake()
        {
            if (openType == OpenType.ByButton){
                if(closeBtn) closeBtn.onClick.AddListener(Close);
            }
            for (int i = 0; i < relatedPanels.Count; i++)
            {
                NodePanelPair pair = relatedPanels[i];
                if (pair.nodePanel.openType == OpenType.ByButton)
                {
                    if (pair.openBtn != null) pair.openBtn.onClick.AddListener(() =>
                    {
                        if (pair.hideSelf)
                        {
                            if (pair.nodePanel.Open(() => { UnHide();}))
                            {
                                Hide();
                            }
                        }
                        else
                        {
                            pair.nodePanel.UnHide();
                        }
                    });
                }
                if ((pair.nodePanel.openType & OpenType.ByToggle) == OpenType.ByToggle)
                {
                    if (pair.openTog != null) pair.openTog.onValueChanged.AddListener((x) =>
                    {
                        if (x)
                        {
                            pair.nodePanel.UnHide();
                        }
                        else
                        {
                            pair.nodePanel.Close();
                        }
                    });
                }
            }
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void UnHide()
        {
            gameObject.SetActive(true);
        }

        public bool Open(UnityAction onClose)
        {
            IOpenAble panel = gameObject.GetComponent<IOpenAble>();
            if ((panel != null && panel.OpenAble()) || panel == null)
            {
                UnHide();
                this.onClose = onClose;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Close()
        {
            ICloseAble panel = gameObject.GetComponent<ICloseAble>();
            if ((panel != null && panel.CloseAble()) || panel == null)
            {
                for (int i = 0; i < relatedPanels.Count; i++){
                    relatedPanels[i].nodePanel.Hide();
                }
                if (onClose != null) onClose.Invoke();
                Hide();
            }
        }
    }
}