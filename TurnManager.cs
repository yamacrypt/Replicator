using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=System.Random;
using UniRx;
using UniRxPool;
using System.Linq;
using Cysharp.Threading.Tasks;

public class TurnManager : MonoBehaviour
{
    [SerializeField]Button nextButton; 
    InitData initData;
    Dictionary<Vector2Int,IAtom> atomPosDictionary=new Dictionary<Vector2Int, IAtom>();
    // Start is called before the first frame update
    async void Start()
    {
        initData=await StreamPool.Instance.FindObservable<InitData>("initData").First();
        foreach(var atom in initData.atomList)
            atomPosDictionary[atom.position]=atom;
        moveManager=new MoveManager(initData,atomPosDictionary);
        reactionManager=new ReactionManager(initData,atomPosDictionary);
        nextButton.onClick.AddListener(Next);
    }
    MoveManager moveManager;
    ReactionManager reactionManager;
    void Next(){
        moveManager.RandomWalk();
        reactionManager.React();
    }
    
    void Reaction(){

    }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
