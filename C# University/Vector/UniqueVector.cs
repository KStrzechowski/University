using System;

namespace Lab7a
{
    public class UniqueVector
    {
        private int[] tab;
        public readonly int Count;

        public UniqueVector()
        {
            tab = new int[0];
            Count = 0;
        }
        public UniqueVector(int[] t)
        {
            int length = t.Length;
            for(int i = 0; i < t.Length; i++)
                for(int j = 0; j < i; j++)
                    if(t[i] == t[j])
                    {
                        t[i] = -1;
                        length--;
                        break;
                    }
            tab = new int[length];
            int k = 0;
            for (int i = 0; i < t.Length; i++)
                if (t[i] != -1) 
                { 
                    tab[k] = t[i];
                    k++;
                }
            Count = length;
        }
        public UniqueVector Clone()
        {
            UniqueVector t = new UniqueVector(this.tab);
            return t;
        }
        public void Deconstruct(out UniqueVector t1, out UniqueVector t2)
        {
            int[] array1, array2;
            if (this.tab.Length % 2 == 0)
            {
                array1 = new int[tab.Length / 2];
                array2 = new int[tab.Length / 2];
            }
            else
            {
                array1 = new int[(tab.Length + 1) / 2];
                array2 = new int[(tab.Length - 1) / 2];
            }
            int i;
            for (i = 0; i < array1.Length; i++)
                array1[i] = tab[i];
          
            for (; i - array1.Length < array2.Length; i++)
                array2[i - array1.Length] = tab[i];

            t1 = new UniqueVector(array1);
            t2 = new UniqueVector(array2);
        }

        public int this[int i] //bez sprawdzania
        {
            get { return tab[i]; }
            set { tab[i] = value; }
        }

        public static bool operator ==(UniqueVector v1, UniqueVector v2)
        {
            if (v1.tab.Length != v2.tab.Length) { return false; }
            for(int i = 0; i < v1.tab.Length; i++)
                if(v1[i] != v2[i]) { return false; }
            return true;
        }
        public override bool Equals(object obj)
        {
            return this == (UniqueVector)obj;
        }
        public static bool operator !=(UniqueVector v1, UniqueVector v2)
        {
            if (v1.tab.Length != v2.tab.Length) { return true; }
            for (int i = 0; i < v1.tab.Length; i++)
                if (v1[i] != v2[i]) { return true; }
            return false;
        }
        public override int GetHashCode()
        {
            return tab.GetHashCode() ^ Count.GetHashCode();
        }

        public static explicit operator UniqueVector(int[] t)
        {
            return new UniqueVector(t);
        }

        public static implicit operator UniqueVector(int t)
        {
            int[] array = new int[1];
            array[0] = t;
            return new UniqueVector(array);
        }

        public static implicit operator int[](UniqueVector v)
        {
            int[] array = new int[v.Count];
            for (int i = 0; i < v.Count; i++)
                array[i] = v[i];
            return array;
        }
        public static UniqueVector operator +(UniqueVector v, int value)
        {
            for(int i = 0; i < v.Count; i++)
                if(v[i] == value) { return v; }
            int[] array = new int[v.Count + 1];
            for (int i = 0; i < v.Count; i++)
                array[i] = v[i];
            array[v.Count] = value;
            return new UniqueVector(array);
        }
        public static UniqueVector operator +(UniqueVector v1, UniqueVector v2)
        {
            int length = v1.Count + v2.Count;
            for (int i = 0; i < v2.Count; i++)
                for (int j = 0; j < v1.Count; j++)
                    if (v2[i] == v1[j])
                    {
                        v2[i] = -1;
                        length--;
                        break;
                    }
            int[] tab = new int[length];
            int k = 0;
            for (int i = 0; i < v1.Count + v2.Count; i++)
                if (i < v1.Count)
                {
                    tab[k] = v1[i];
                    k++;
                }
                else         
                    if(v2[i -v1.Count] != -1) 
                    { 
                        tab[k] = v2[i - v1.Count];
                        k++;
                    }

            return new UniqueVector(tab);
        }
        public static UniqueVector operator ++(UniqueVector v)
        {
            int[] array = new int[v.Count];
            for (int i = 0; i < v.Count; i++)
                array[i] = v[i] + 1;
            return new UniqueVector(array);
        }
        public static UniqueVector operator *(UniqueVector v1, UniqueVector v2)
        {
            int length = 0;
            int[] temp = new int[v1.Count + v1.Count];
            for (int i = 0; i < v1.Count + v2.Count; i++)
                temp[i] = -1;
            
            for (int i = 0; i < v2.Count; i++)
            {
                for (int j = 0; j < v1.Count; j++)
                    if (v2[i] == v1[j])
                    {
                        temp[i] = v2[i];
                        length++;
                        break;
                    }
            }
            if(length == 0) { return new UniqueVector(); }
            int[] tab = new int[length];
            int k = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if(temp[i] != -1) 
                {
                    tab[k] = v2[i];
                    k++;
                }
            }

            return new UniqueVector(tab);
        }
        public override string ToString()
        {
            string result = "[";
            result += string.Join(';', tab);
            result += "]";

            return result;
        }
    }
}
