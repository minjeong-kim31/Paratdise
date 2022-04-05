using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 작성자 : 조영민
/// 최초작성일 : 2022/04/04
/// 최종수정일 : 
/// 설명 : 
/// 
/// 추격스테이지의 추격자
/// </summary>
namespace YM
{
    public class Tracer : MonoBehaviour
    {
        [SerializeField] private float speed = 0.5f;
        [SerializeField] private float delayTime = 2f;
        Transform tr;
        private void Awake()
        {
            tr = transform;
        }

        public void StartMove()
        {
            StartCoroutine(E_Move());
        }

        IEnumerator E_Move()
        {   
            yield return new WaitForSeconds(delayTime);
            while (true)
            {
                tr.Translate(Vector3.up * speed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
