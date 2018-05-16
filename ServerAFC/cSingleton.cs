using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServerAFC
{
    public class cSingleton<T> where T : class
    {
        private static object _Lock = new object();
        private static volatile T _Instance = null;

        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    CreateInstance();
                }

                return _Instance;
            }
        }

        private static void CreateInstance()
        {
            lock (_Lock)
            {
                if (_Instance == null)
                {
                    Type t = typeof(T);

                    //// Ensure there are no public constructors...  
                    //ConstructorInfo[] ctors = t.GetConstructors();  
                    //if (ctors.Length > 0)  
                    //{  
                    //   throw new InvalidOperationException(String.Format("{0} has at least one accesible ctor making it impossible to enforce singleton behaviour", t.Name));  
                    //}  

                    // Create an instance via the private constructor  
                    HttpContext.Current.Application[t.ToString()] = (T)Activator.CreateInstance(t, true);
                    _Instance = (T)HttpContext.Current.Application[t.ToString()];
                }
            }
        }
    }
}
