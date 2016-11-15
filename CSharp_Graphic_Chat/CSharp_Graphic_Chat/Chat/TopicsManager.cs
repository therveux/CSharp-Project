﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CSharp_Graphic_Chat.Chat
{
    namespace Chat
    {
        class TopicsManager
        {
            private static Hashtable _topics = new Hashtable();

            public TopicsManager()
            {
            }

            public Hashtable topics
            {
                get
                {
                    return _topics;
                }
                set
                {
                    _topics = value;
                }
            }

            public Chatroom joinTopic(String topic)
            {
                if (_topics.Contains(topic))
                    return (Chatroom)_topics[topic];
                else
                    return null;
            }

            public void createTopic(String topic)
            {
                if (_topics.Contains(topic))
                    return;
                else
                    _topics.Add(topic,new Chatroom(topic));
            }

            public List<string> getRooms()
            {
                List<string> res = new List<string>();
                foreach (string s in _topics.Keys)
                {
                    res.Add(s);
                }
                return res;
            }
        }
    }
}
