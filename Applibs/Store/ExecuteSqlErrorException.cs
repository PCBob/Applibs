
using System;
using System.Collections.Generic;

namespace Applibs.Store
{


    public class ExecuteSqlErrorException : Exception
    {
        public ExecuteSqlErrorException()
        {
         
        }

        public ExecuteSqlErrorException(string commandText, IDictionary<string, object> parameters, string message, Exception inner)
            : base(message, inner)
        {
            this.CommandText = commandText;
            this.Parameters = new Dictionary<string, object>().Copy(parameters);
        }

        public ExecuteSqlErrorException(string message)
            : base(message)
        {
        }

        public ExecuteSqlErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }


#if !NetCore
        protected ExecuteSqlErrorException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif

        public string CommandText { get; }

        public IDictionary<string, object> Parameters { get; }
    }
}