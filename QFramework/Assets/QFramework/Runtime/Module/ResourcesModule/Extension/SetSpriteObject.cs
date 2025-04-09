using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework
{
    [SerializeField]
    public class SetSpriteObject : ISetAssetObject
    {
        enum SetSpriteType
        {
            None,
            Image,
            SpriteRender,
        }

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private SetSpriteType m_setType;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private Image m_image;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private SpriteRenderer m_spriteRenderer;

#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private Sprite m_sprite;

        private bool m_setNativeSize = false;
        private CancellationToken m_cancellationToken;

        public string Location { get; private set; }

        public void SetAsset(Object asset)
        {
            m_sprite = asset as Sprite;

            if (m_cancellationToken.IsCancellationRequested)
            {
                return;
            }

            if (m_image != null)
            {
                m_image.sprite = m_sprite;
                if (m_setNativeSize)
                {
                    m_image.SetNativeSize();
                }
            }
            else if(m_spriteRenderer != null)
            {
                m_spriteRenderer.sprite = m_sprite;
            }
        }

        public bool CanRelease()
        {
            if (m_setType == SetSpriteType.Image)
            {
                return m_image == null || m_image.sprite == null
                    || (m_sprite != null && m_image.sprite != m_sprite);
            }
            else if(m_setType == SetSpriteType.SpriteRender)
            {
                return m_spriteRenderer == null || m_spriteRenderer.sprite == null
                    || (m_sprite != null && m_spriteRenderer.sprite != m_sprite);
            }

            return true;
        }

        public void OnRelease()
        {
            m_spriteRenderer = null;
            m_image = null;
            m_sprite = null;
            m_setNativeSize = false;
            m_setType = SetSpriteType.None;
            Location = null;
        }

        public static SetSpriteObject Create(Image image, string location, bool setNativeSize = false, CancellationToken cancelToken = default)
        {
            SetSpriteObject setSpriteObject = MemoryPoolMgr.Get<SetSpriteObject>();
            setSpriteObject.Location = location;
            setSpriteObject.m_image = image;
            setSpriteObject.m_cancellationToken = cancelToken;
            setSpriteObject.m_setNativeSize = setNativeSize;
            setSpriteObject.m_setType = SetSpriteType.Image;
            return setSpriteObject;
        }

        public static SetSpriteObject Create(SpriteRenderer spriteRenderer, string location, bool setNativeSize = false, CancellationToken cancelToken = default)
        {
            SetSpriteObject setSpriteObject = MemoryPoolMgr.Get<SetSpriteObject>();
            setSpriteObject.Location = location;
            setSpriteObject.m_spriteRenderer = spriteRenderer;
            setSpriteObject.m_cancellationToken = cancelToken;
            setSpriteObject.m_setNativeSize = setNativeSize;
            setSpriteObject.m_setType = SetSpriteType.SpriteRender;
            return setSpriteObject;
        }
    }
}