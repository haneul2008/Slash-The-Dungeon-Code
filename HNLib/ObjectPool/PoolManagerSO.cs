
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HN.HNLib.ObjectPool
{
    [CreateAssetMenu(fileName = "PoolManager", menuName = "SO/PoolManager", order = 0)]
    public class PoolManagerSO : ScriptableObject
    {
        [SerializeField] private string prefabPath;
        
        public List<MonoBehaviour> poolList;
        private Dictionary<Type, Pool> _poolPairs;
        private Dictionary<Type, List<MethodInfo>> _resetItemPairs;
        private Dictionary<string, Pool> _poolPairsWithName;
        private List<Type> _duplicationTable;
        
        public void Initialize(Transform parent)
        {
            _poolPairs = new Dictionary<Type, Pool>();
            _resetItemPairs = new Dictionary<Type, List<MethodInfo>>();
            _poolPairsWithName = new Dictionary<string, Pool>();
            _duplicationTable = new List<Type>();

            foreach (MonoBehaviour monoBehaviour in poolList)
            {
                Type type = monoBehaviour.GetType();
                AddPool(parent, type, monoBehaviour);
                AddResetItem(type);
            }
        }

        private void AddResetItem(Type type)
        {
            MethodInfo[] methodInfos =
                type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (MethodInfo methodInfo in methodInfos)
            {
                if(methodInfo.GetParameters().Length > 0) continue;
                
                if (Attribute.IsDefined(methodInfo, typeof(ResetItemAttribute)))
                {
                    if (_resetItemPairs.TryGetValue(type, out List<MethodInfo> infos))
                    {
                        infos.Add(methodInfo);
                    }
                    else
                    {
                        _resetItemPairs.Add(type, new List<MethodInfo>(){methodInfo});
                    }
                }
            }
        }

        private void AddPool(Transform parent, Type type, MonoBehaviour monoBehaviour)
        {
            PoolableAttribute attribute = Attribute.GetCustomAttribute(type, typeof(PoolableAttribute)) as PoolableAttribute;

            if (attribute == null)
            {
                Debug.LogWarning($"{type} class attribute is null");
                return;
            }
            
            Pool pool = new Pool(monoBehaviour, parent, attribute.Count);
            if (_poolPairs.TryAdd(type, pool) == false && _duplicationTable.Contains(type) == false)
                _duplicationTable.Add(type);

            if (attribute.PoolName is not null)
            {
                _poolPairsWithName.Add(attribute.PoolName, pool);
            }
            else
            {
                string key = monoBehaviour.gameObject.name;
                if (_poolPairsWithName.TryAdd(key, pool) == false)
                {
                    Debug.LogWarning("gameObject name is exist");
                }
            }
        }

        public void Push(MonoBehaviour mono)
        {
            Type type = mono.GetType();

            if (_duplicationTable.Contains(type))
            {
                if (_poolPairsWithName.TryGetValue(mono.gameObject.name, out Pool pool))
                {
                    pool.Push(mono);
                }
            }
            else if (_poolPairs.TryGetValue(type, out Pool pool))
            {
                pool.Push(mono);
            }
        }

        public T Pop<T>() where T : MonoBehaviour
        {
            if (_poolPairs.TryGetValue(typeof(T), out Pool pool))
            {
                MonoBehaviour item = pool.Pop();
                CallResetItem(item);

                return item as T;
            }

            return null;
        }
        
        public T Pop<T>(string poolName) where T : MonoBehaviour
        {
            if (_poolPairsWithName.TryGetValue(poolName, out Pool pool))
            {
                MonoBehaviour item = pool.Pop();
                CallResetItem(item);
                return item as T;
            }

            return null;
        }

        private void CallResetItem(MonoBehaviour item)
        {
            Type type = item.GetType();
            if (_resetItemPairs.TryGetValue(type, out List<MethodInfo> methodInfos))
            {
                foreach (MethodInfo methodInfo in methodInfos)
                {
                    methodInfo.Invoke(item, null);
                }
            }
        }

        [ContextMenu("Print pairs")]
        public void PrintPairs()
        {
            foreach (var pair in _poolPairsWithName)
            {
                Debug.Log($"<color=red>----------{pair.Key}-----------");
                Pool pool = (Pool)pair.Value;
                pool.DebugAll();
            }
        }

#if UNITY_EDITOR
        [ContextMenu("GeneratePoolList")]
        private void GeneratePoolList()
        {
            poolList.Clear();
            
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { prefabPath });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                MonoBehaviour[] monoBehaviours = gameObject.GetComponents<MonoBehaviour>();

                foreach (MonoBehaviour mono in monoBehaviours)
                {
                    Type type = mono.GetType();

                    if (Attribute.IsDefined(type, typeof(PoolableAttribute)))
                    {
                        poolList.Add(mono);
                        break;
                    }
                }
            }
        }
#endif
    }
}