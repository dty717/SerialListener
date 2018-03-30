using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialListener.data
{
    public class function
    {
        public string functionName;

        public List<int> arrange;

        public int param;

        public function(string name,List<int>list,int pa) {
            functionName = name;
            arrange = list;
            param = pa;
        }
    }
}
