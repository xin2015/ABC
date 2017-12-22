using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.BLL.CryptoTransverters
{
    public interface IEncrypt
    {
        byte[] Encrypt(byte[] inputBuffer);
        string Encrypt(string inString);
    }
}
