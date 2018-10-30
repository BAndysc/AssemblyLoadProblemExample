using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Common
{
    public interface IProvider 
    {
        ContentControl GetView();
    }

    public class AutoRegisterAttribute : Attribute
    {
    }
}
