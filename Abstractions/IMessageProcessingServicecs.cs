using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Abstractions
{
    public interface IMessageProcessingService
    {
        void Process(byte[] body);
    }
}
