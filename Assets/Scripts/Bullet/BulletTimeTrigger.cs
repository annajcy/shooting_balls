using Framework.Event;
using UnityEngine;
using EventType = Framework.Event.EventType;

namespace Bullet
{
    public class BulletTimeTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Tower"))
                EventCenter.Instance.EventTrigger(EventType.HitBulletTimeStart);
        }
    }
}
