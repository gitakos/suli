using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace _2024._04._15.wpfbeadando
{
    //ObservableCollection
    public class kep : Collection<FileInfo>
    {
        public kep(List<FileInfo>file) : base(file)
        {
       
        } 

        public string utvonal;
        public string datum;
        public string meret;
    }
}
