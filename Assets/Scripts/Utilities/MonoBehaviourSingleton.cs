using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
{

	static T m_instance;
	static bool m_wasCreated = false;

	public static T Instance
	{
		get
		{	
			if (m_instance == null) {
				if (!m_wasCreated) {
					Debug.LogError ("intentando usar una instancia de " + typeof(T).Name + " antes que haya sido creado!");
				} else {
					Debug.LogWarning ("intentando usar una instancia de " + typeof(T).Name + " pero ya fue destruida!");
				}
			}
			return m_instance;
		}
	}

	protected virtual void SingletonAwake() { }

	protected virtual void SingletonDestroy() { }

	void Awake ()
	{
		Debug.Log("Creando singleton: " + typeof(T).Name);
		if (m_instance == null)
		{
			m_instance = this as T;
			m_wasCreated = true;
			SingletonAwake();
		}
		else
		{
			Debug.Log("Se intento crear una segunda instancia de " + typeof(T).Name + ", destruyendo el gameobject llamado "+ gameObject.name);
			Destroy(gameObject);
		}
	}

	void OnDestroy()
	{
		Debug.Log("Destroying " + typeof(T).Name);
		if (Object.ReferenceEquals(m_instance, this))
		{
			SingletonDestroy();
			m_instance = null;
		}
	}
}
