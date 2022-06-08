using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class Productor
{
    public int resource;

    public void UpdateResource(int newresource)
    {
        resource += newresource;
    }
}

