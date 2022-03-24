using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Megumin.GameFramework.Sensor
{
    public partial class AISensor<T> : MonoBehaviour
    {
        [ProtectedInInspector]
        public HearingSensor HearingSensor;
        [ProtectedInInspector]
        public SightSensor SightSensor;

        [ProtectedInInspector]
        public List<T> Ignore = new List<T>();

        [Range(0, 5)]
        public float checkDelta = 0.5f;

        public void Awake()
        {
            FindC();
        }

        private void Reset()
        {
            FindC();
        }

        void FindC()
        {
            if (!HearingSensor)
            {
                HearingSensor = GetComponentInChildren<HearingSensor>();
            }

            if (!SightSensor)
            {
                SightSensor = GetComponentInChildren<SightSensor>();
            }

            var self = GetComponentInParent<T>();
            if (self != null)
            {
                if (!Ignore.Contains(self))
                {
                    Ignore.Add(self);
                }
            }
        }

        [ReadOnlyInInspector]
        public List<T> SightTarget = new List<T>();
        [ReadOnlyInInspector]
        public List<T> HearingTarget = new List<T>();
        [ReadOnlyInInspector]
        public HashSet<T> InSensor = new HashSet<T>();

        private float nextCheckStamp;
        private void Update()
        {
            if (Time.time < nextCheckStamp)
            {
                return;
            }
            nextCheckStamp = Time.time + checkDelta;

            var mixR = Mathf.Max(HearingSensor.Radius, SightSensor.Radius);

            var collidersInRadius =
                Physics.OverlapSphere(transform.position, mixR);

            SightTarget.Clear();
            HearingTarget.Clear();

            HashSet<T> currentIn = new HashSet<T>();
            foreach (var item in collidersInRadius)
            {
                var tarC = item.GetComponentInParent<T>();
                if (tarC == null)
                {
                    continue;
                }

                if (Ignore.Contains(tarC))
                {
                    continue;
                }

                if (tarC is MonoBehaviour behaviour)
                {
                    if (SightSensor.Check(behaviour, item))
                    {
                        //���Ӿ���Χ��
                        SightTarget.Add(tarC);
                        currentIn.Add(tarC);
                    }

                    if (HearingSensor.Check(behaviour))
                    {
                        HearingTarget.Add(tarC);
                        currentIn.Add(tarC);
                    }
                }
            }

            ///����Ѿ��ڸ�֪��Χ�ڵģ������ǲ����뿪��֪��Χ
            foreach (var item in InSensor)
            {
                if (item is MonoBehaviour behaviour)
                {
                    if (SightSensor.Check(behaviour, null))
                    {
                        //���Ӿ���Χ��
                        SightTarget.Add(item);
                        currentIn.Add(item);
                    }

                    if (HearingSensor.Check(behaviour))
                    {
                        HearingTarget.Add(item);
                        currentIn.Add(item);
                    }
                }
            }

            foreach (var item in InSensor)
            {
                if (currentIn.Contains(item))
                {

                }
                else
                {
                    //ʧȥ��֪
                    OnLostTarget(item);
                }
            }

            foreach (var item in currentIn)
            {
                if (InSensor.Contains(item))
                {

                }
                else
                {
                    //�¸�֪
                    OnFindTarget(item);
                }
            }

            InSensor.Clear();
            foreach (var item in currentIn)
            {
                InSensor.Add(item);
            }
        }

        public virtual void OnFindTarget(T target)
        {
            Debug.Log($"��֪ģ�� ������Ŀ��");
        }

        public virtual void OnLostTarget(T target)
        {
            Debug.Log($"��֪ģ�� ʧȥĿ��");
        }
    }
}



