using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LinkIOcsharp.model
{
    public class Event
    {
        private String type;
        private Boolean me;
        private JObject root;
        private string obj;

        public Event(JObject jsonObj)
        {
            try
            {
                root = jsonObj;
                this.type = (String) jsonObj.SelectToken("type");
                this.obj = jsonObj.SelectToken("data").ToObject<string>();

                /*data = new Dictionary<String, JToken>();

                JToken ds = jsonObj.SelectToken("data");
                if (ds.Type == JTokenType.Object)
                {
                    JObject obj = (JObject)ds;
                    foreach (KeyValuePair<String, JToken> j in obj)
                    {
                        data.Add(j.Key, j.Value);
                    }
                }
                else
                    this.obj = jsonObj.SelectToken("data");*/
                
            }
            catch (Exception e)
            {
                //e.printStackTrace();
            }
        }

        /*public T get<T>(String s)
        {
            if (s == null || s == "")
                return get<T>();
            else
                return data[s].ToObject<T>();
        }*/

        public T get<T>()
        {
            return (T)deserializeObject(obj);
        }

        public Boolean isMe()
        {
            return me;
        }

        public void setMe(Boolean me)
        {
            this.me = me;
        }

        public String getType()
        {
            return type;
        }

        public void setType(String type)
        {
            this.type = type;
        }

        public static object deserializeObject(string str)
        {
            byte[] bytes = Convert.FromBase64String(str);

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }
    }

}
