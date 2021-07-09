using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Part_1.PrimaryDal
{
    public class PrimaryDalList: PrimaryDalItem, IEnumerable
    {
        Hashtable _primaryList;
        List<PrimaryDalItem> _strongList;

		public PrimaryDalList()
		{
			_primaryList = new Hashtable();
			_strongList = new List<PrimaryDalItem>();
		}
		public IEnumerator GetEnumerator()
		{
			return new PrimaryDalListEnumerator(this);
		}

		public int ListCount
		{
			get { return _primaryList.Count; }
		}

		public void Add(object _item)
		{
			_primaryList.Add(Convert.ToString(ListCount), _item);
			_strongList.Add((PrimaryDalItem)_item);
		}

		public object this[string loc]
		{
			get { return (object)_primaryList[loc]; }
		}

		private class PrimaryDalListEnumerator : IEnumerator
		{
			PrimaryDalList eList;
			int location;
			public PrimaryDalListEnumerator(PrimaryDalList eList)
			{
				this.eList = eList;
				location = -1;
			}
			public bool
				MoveNext()
			{
				++location;
				return (location > eList.ListCount - 1) ? false : true;
			}

			//returns the currently selected object
			public object Current
			{
				get
				{
					return eList[(string)Convert.ToString(location)];
				}
			}

			//reset the indexer. first thing called during a foreach loop
			public void Reset()
			{
				location = -1;
			}
		}
	}
}
