using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectCompare
{
    public class CompareModel<T>
    {
        public bool Pass
        {
            set;
            get;
        }
        public T SourceObject
        {
            set;
            get;
        }
        public T TargetObject
        {
            set;
            get;
        }
        public List<string> ExemptProperties
        {
            set;
            get;
        }
        public List<string> SubObjects
        {
            set;
            get;
        }
        public Dictionary<string, string> MismatchedProperties
        {
            set;
            get;
        }
        public string ID
        {
            set;
            get;
        }
        public string HTMLReport
        {
            set;
            get;
        }

        public CompareModel()
        {
            ExemptProperties = new List<string>();
            MismatchedProperties = new Dictionary<string, string>();
            SubObjects = new List<string>();
        }
    }
}
