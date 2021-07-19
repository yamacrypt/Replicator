using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;

namespace UniRxPool{
public class StreamPool 
{
    static StreamPool instance;
    public static StreamPool Instance{
        get{
        if(instance==null)
            instance=new StreamPool();
        return instance;
        }
    }
    private Dictionary<string, object> streams = new Dictionary<string, object>();
    class Stream<T>{
        public Dictionary<Type,(bool sendable,bool receivable)> accesessableDictionary{get;private set;}
        public readonly Subject<T> subject;
        public readonly bool isStrict;

            public Stream(Subject<T> subject, Type[] sender, Type[] receiver, bool isStrict)
            {
                accesessableDictionary=new Dictionary<Type, (bool sendable, bool receivable)>();
                this.subject = subject;
                this.isStrict = isStrict;
                foreach (Type item in sender)
                    {
                        if(accesessableDictionary.ContainsKey(item)){
                            accesessableDictionary[item]=(true,accesessableDictionary[item].receivable);
                        }
                        else{
                            accesessableDictionary[item]=(true,false);
                        }
                    }
                    foreach (Type item in receiver)
                    {
                        if(accesessableDictionary.ContainsKey(item)){
                            accesessableDictionary[item]=(accesessableDictionary[item].sendable,true);
                        }
                        else{
                            accesessableDictionary[item]=(false,true);
                        }
                    }
            }


        }

    public ISubject<T> RegisterStream<T>(string key ,Type[] sender,Type[] receiver,bool isStrict=true){
        var subject = new Subject<T>();
        streams.Add(key, new Stream<T>(subject,sender,receiver,isStrict));
        return subject;
    }
    public ISubject<T> RegisterStream<T>(string key ,Type sender,Type[] receiver,bool isStrict=true){
        return RegisterStream<T>(key,new Type[]{sender},receiver,isStrict);
    }
    public ISubject<T> RegisterStream<T>(string key ,Type[] sender,Type receiver,bool isStrict=true){
        return RegisterStream<T>(key,sender,new Type[]{receiver},isStrict);
    }
    public ISubject<T> RegisterStream<T>(string key ,Type sender,Type receiver,bool isStrict=true){
        return RegisterStream<T>(key,new Type[]{sender},new Type[]{receiver},isStrict);
    }
    public IObserver<T> FindObserver<T>(string key){
        object obj;
        Type caller=new StackTrace().GetFrame(1).GetMethod().ReflectedType;
         caller=TypeUtils.StringToType(TypeUtils.TypeToString(caller));
        if (streams.TryGetValue(key, out obj))
        {
            Stream<T> stream=obj as Stream<T>;
            if(stream.accesessableDictionary[caller].sendable)
                return stream.subject as IObserver<T>;
        }
        throw new KeyNotFoundException();
    }

    public IObservable<T> FindObservable<T>(string key)
    {
        object obj;
        //if(caller==null)
        Type  caller=new StackTrace().GetFrame(1).GetMethod().ReflectedType;
         caller=TypeUtils.StringToType(TypeUtils.TypeToString(caller));
        if (streams.TryGetValue(key, out obj))
        {
            Stream<T> stream=obj as Stream<T>;
            if(stream.accesessableDictionary[caller].receivable)
                return stream.subject as IObservable<T>;
            //else if(stream.isStrict&&stream.receiver is )
        }
        throw new KeyNotFoundException();
    }

    public bool RemoveSubject(string key)
    {
        return streams.Remove(key);
    }
     public class TypeUtils{
        public static  string TypeToString(Type type){
            //return type.ToString();
            if(!type.IsGenericType)
                return $"{type.ToString().Split('+')[0]}, {type.Assembly.GetName().Name}";
            return $"{type}, LuckyGameData";
            throw new Exception();
            //ype.
        }
        public static Type StringToType(string s,bool isGeneric=false){
            if(!isGeneric)
                return Type.GetType(s);
            throw new Exception();
        }
    }
}
/*
public static class SubjectPoolExtensions
{
    public static ISubject<T> CreateSubject<T>(this Component component, string key, bool isBroadcast = true)
    {
        var subject = SubjectPool.Instance.CreateSubject<T>(key);

        if (isBroadcast)
        {
            foreach (var com in UnityEngine.Object.FindObjectsOfType<Component>())
            {
                var listener = com as IObservableAddedListener<T>;
                if (listener != null) listener.OnObservableAdded(component, key, subject);
            }
        }

        return subject;
    }

    public static IObservable<T> FindObservable<T>(this Component component, string key)
    {
        return SubjectPool.Instance.FindSubject<T>(key);
    }

    public static bool RemoveSubject(this Component component, string key)
    {
        return SubjectPool.Instance.RemoveSubject(key);
    }
}
public interface IObservableAddedListener<T>
{
    void OnObservableAdded(Component sender, string key, IObservable<T> observable);
}
*/
}