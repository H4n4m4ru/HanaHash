using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HanaHash
{
    public class HashTable {
        private class Pair
    {
        private Pair _next;
        private byte[] _index = new byte[8];
        private int _value;
        //================
        public Pair(){
            ;
        }
        public Pair(byte[] index,int value) {
            for (int i = 0; i < 8; i++)
                this._index[i] = index[i];
            this._value=value;
        }
        public Pair Next
        {
            get
            {
                return this._next;
            }
            set
            {
                this._next = value;
            }
        }
        public byte[] Index{
          get{
               return this._index;
          }
        } 
        public int Value{
            get{
                return this._value;
            }
            set{
                this._value = value;
            }
        }
    }
        private RNGCryptoServiceProvider RNGBOT = new RNGCryptoServiceProvider();
        public byte[][][][] ZobristKeyz = new byte[12][][][];
        private Pair Root = new Pair();
        private Pair Last = new Pair();
        //============================
        public HashTable(){
            //Initialize the RandomBytestringMatrix;
            for (int i = 0; i < 12; i++){
                ZobristKeyz[i] = new byte[8][][];
                for (int j = 0; j < 8; j++){
                    ZobristKeyz[i][j] = new byte[8][];
                    for (int k = 0; k < 8; k++) {
                        ZobristKeyz[i][j][k] = new byte[8];
                        RNGBOT.GetBytes(ZobristKeyz[i][j][k]);
                    }
                }
            }
            Root.Next = null;
            this.Last = this.Root;
        }
        public byte[] Hash(int[][] OrgData){
            byte[] hashValue = new byte[8];

            for(int i=0;i<8;i++)
              for(int j=0;j<8;j++){
                   int First_Index= OrgData[i][j];
                   if (First_Index != 0) {
                       byte[] KeyValue = ZobristKeyz[First_Index - 6][i][j];
                       for (int k = 0; k < 8; k++)
                           hashValue[k] ^= KeyValue[k];
                   }
               }
           return hashValue;
        }    
        public void Add(byte[] index,int value){
            //Check does any pair's index equal argument's index
            for (Pair CurrentPair = Root.Next; CurrentPair != null; CurrentPair = CurrentPair.Next){
                int i = 0;
                for (; i < 8; i++) 
                    if (CurrentPair.Index[i] != index[i]) break;
                if (i == 8){
                    CurrentPair.Value = value;
                    return;
                }
            }
            //Create a new pair object
            this.Last.Next = new Pair(index,value);
            this.Last = this.Last.Next;
            this.Last.Next = null;
        }
        public int Search(byte[] index){
            for (Pair CurrentPair = Root.Next; CurrentPair != null; CurrentPair = CurrentPair.Next){
                int i = 0;
                for (; i < 8; i++)
                    if (CurrentPair.Index[i] != index[i]) break;
                if (i == 8) return CurrentPair.Value;
            }
            return -1;   //-1 can be any number which be thought as "false";
         }
    }
}
