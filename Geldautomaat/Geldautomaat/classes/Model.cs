using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geldautomaat.classes
{
    public class Model
    {
        protected myDBconnection db;
        public Model ()
        {
            db = myDBconnection.Instance;
        }
    }
}
