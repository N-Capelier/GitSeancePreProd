using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.CameraManagement
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class CameraShake : MonoBehaviour
    {
        private bool _isShaking = false;

        public IEnumerator Shake(float duration, float xMagnitude, float yMagnitude, float zMagnitude)
        {
            if(!_isShaking)
            {
                _isShaking = true;

                Vector3 originalPos = transform.localPosition;
                float elapsed = 0f;

                while(elapsed < duration)
                {
                    float xDelta = Random.Range(-1f, 1f) * xMagnitude;
                    float yDelta = Random.Range(-1f, 1f) * yMagnitude;
                    float zDelta = Random.Range(-1f, 1f) * zMagnitude;

                    transform.position = new Vector3(originalPos.x + xDelta, originalPos.y + yDelta, originalPos.z + zDelta);
                    elapsed += Time.deltaTime;
                    yield return 0;
                }
                transform.position = originalPos;

                _isShaking = false;
            }
        }
    }
}
