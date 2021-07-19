using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace RandomSelector{
public class WeightedRandomSelector<Item>:RandomSelector<Item> where Item:IFrequency
{
    public WeightedRandomSelector(IReadOnlyCollection<Item> inputs) : base(inputs)
    {
         this.temps= inputs.Select(ele=>new Temp(ele,ele.frequency)).ToArray();

    }
}
public class RandomSelector<Item>
{
    
    protected Temp[] temps;
    protected class Temp{
        public Item item{get;private set;}
        public int frequency{get;private set;}

            public Temp( Item item,int frequency)
            {
                this.frequency = frequency;
                this.item = item;
            }
        }
    public RandomSelector(IReadOnlyCollection<Item> inputs,IReadOnlyList<int> frequencyList=null)
    {
        if(frequencyList==null){
        this.temps= inputs.Select(ele=>new Temp(ele,1)).ToArray();
        }
        else{
            if(inputs.Count!=frequencyList.Count)throw new Exception("List Count not equal");
            this.temps= inputs.Zip(frequencyList,(ele,f)=>new Temp(ele,f)).ToArray(); // inputs.Select(ele=>new Temp(ele,1)).ToArray();
        }

    }
    public Item getItem(){
        return getItem(1).First();
    }
        
    public virtual List<Item> getItem(int takeCount){
        int[] range=new int[temps.Length];
        if(temps.Length==0)
            return new List<Item>();
        int sum=0;
        {
            int i=0;
            foreach (Temp item in temps)
            {
                range[i]=sum;
                //Debug.Log(sum);
                sum+=item.frequency;
                if(item.frequency<0){
                    throw new System.IndexOutOfRangeException("frequency must not be negative");
                }
                else if(item.frequency==0){
                    Debug.Log("zero frequency is not recommended");
                }
                i++;
            }
        }
        List<Item> resList=new List<Item>();
        //System.Random r = new System.Random(CreateRandomSeed());
        for(int i=0;i<takeCount;i++){
            if(sum<=0)
                break;
            //Debug.Log(string.Join(", ", range));
            int res=Randomizer.Instance.Mod(sum);//r.Next()%sum;//Math.Abs(System.BitConverter.ToInt32(randomByte,0))%sum;
            int index = UpperBound<int>(range, res );
            //Debug.Log(index);
            if(index-1<0 || index-1>=temps.Length){
                throw new Exception($"{index} is out of range index");
            }
            resList.Add(temps[index-1].item);
            int decreasedAmount=temps[index-1].frequency;
            sum-=decreasedAmount;
            for(int pos=0;pos<temps.Length;pos++){
                if(pos>=index){
                    range[pos]-=decreasedAmount;
                }
            }
        }
        return resList;
    }
    public int UpperBound<T>(T[] a, T v)
    { 
        return UpperBound(a, v, Comparer<T>.Default);
    }

    public  int UpperBound<T>(T[] a, T v, Comparer<T> cmp)
    {
        var l = 0;
        var r = a.Length - 1;
        while (l <= r)
        {
            var mid = l + (r - l) / 2;
            var res = cmp.Compare(a[mid], v);
            if (res <= 0) l = mid + 1;
            else r = mid - 1;
        }
        return l;
    }
}
public class Randomizer{
    static Randomizer instance;

      

    static public Randomizer Instance{get{
        if(instance==null){
            instance=new Randomizer();
        }
        return instance;
    }}
    static System.Random r;
    Randomizer()
    {
         r = new System.Random(CreateRandomSeed());
    }
    public int Range(int start,int end){
        int res;
        lock(r){
        res=r.Next();
        }
        res=start+res%(end-start+1);
        return res;
    }
    public int Mod(int mod){
        int res;
        lock(r){
        res=r.Next();
        }
        res%=mod;
        return res;
    }
    int CreateRandomSeed() {
    var bs = new byte[4];
    //Int32と同じサイズのバイト配列にランダムな値を設定する
    using (var rng = new System.Security.Cryptography.RNGCryptoServiceProvider()) {
        rng.GetBytes(bs);
    }
    //RNGCryptoServiceProviderで得たbit列をInt32型に変換してシード値とする。
    return BitConverter.ToInt32(bs, 0);
    }
}

public interface IFrequency{
    int frequency{get;}
}
public interface IGroup{
    int[] group{get;}
}
}