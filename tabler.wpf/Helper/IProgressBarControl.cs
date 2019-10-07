using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tabler.wpf.Helper
{
    public interface IProgressBarControl : IProgress<double>
    {
        void Reset();
        void Increase(int value);
        void Set(int value);
        void Decrease(int value);
        void SetMax(int max, bool doReset);
        void SetOperationName(string name);
    }
}
