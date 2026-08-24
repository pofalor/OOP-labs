using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabSemestr_3
{
    public class CustomList<T> : IEnumerable<T>
    {
        public IList<T> Items { get; set; }

        public CustomList()
        {
            Items = new List<T>();
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public void Add(T value)
        {
            Items.Add(value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}
