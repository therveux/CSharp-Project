﻿using chatLibrary;
using CSharp_server.Authentification.Authentification;
using CSharp_server.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp_server.Chat
{
    namespace Chat
    {
        class Chatroom
        {
            private string _topic;
            private List<User> _chatters = new List<User>();

            public Chatroom(String topic)
            {
                _topic = topic;
            }

            public String topic
            {
                get;
                set;
            }
            public List<User> chatters
            {
                get;
                set;
            }

            public User getChatter(User s)
            {
                foreach (User u in _chatters)
                {
                    if (u.Equals(s))
                        return u;
                }
                return null;
            }
            public List<string> getChatters()
            {
                List<string> chatters = new List<string>();
                foreach (User u in _chatters)
                    chatters.Add(u.login);
                return chatters;
            }

            public void post(String msg, User u)
            {
                //Console.WriteLine(this._topic +" : "+ u.chatter.alias + " : " + msg);
                foreach (User user in this._chatters)
                {
                    //Console.WriteLine("Sending message : " + msg + " from user : " + u.chatter.alias + " in chatroom : " + this._topic + " to user : " + user.chatter.alias);
                    MessageBroadcastPacket mbp = new MessageBroadcastPacket(u.login, msg, this._topic);
                    Packet.Send(mbp, user.ns);
                }
            }
            public void whisper(String msg, User u, User t)
            {
                User chatter = this.getChatter(t);
                if (chatter != null)
                {
                    WhisperMessagePacket wbp = new WhisperMessagePacket(u.login, this._topic, msg, chatter.login);
                    Packet.Send(wbp, chatter.ns);
                }
                else
                    Console.WriteLine("[CHATROOM]Error while sending the whisp " + u.login + "/" + t.login + " message : " + msg + " in chatroom" + this._topic);
            }

            public void quit(User c)
            {
                c.ns.Flush();
                _chatters.Remove(c);
                Console.WriteLine("[CHATROOM]" + c.chatter.alias + " has left the chat" + this.topic);
            }

            public bool join(User c)
            {
                if (_chatters.Contains(c))
                    return false;
                else
                {
                    _chatters.Add(c);
                    Console.WriteLine("[CHATROOM]" + c.chatter + " joined the chat" + this.topic);
                    return true;
                }
            }
        }
    }
}
