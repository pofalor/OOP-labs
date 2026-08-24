using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSemestr_3
{
    class CustomArray<T>
    {
        public T[] Items { get; set; }

        public CustomArray() 
        {
            Items = new T[0];
        } 

        public void Add(T item)
        {
            var helper = Items;
            Items = new T[helper.Length + 1];
            for(int i = 0; i < helper.Length; i++)
            {
                Items[i] = helper[i];
            }
            Items[Items.Length - 1] = item;
        }

        public void Clear()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = default(T);
            }
            Items = null;
        }
    }
}
